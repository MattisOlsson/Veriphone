using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using EPiServer.Globalization;
using EPiServer.ServiceLocation;
using Geta.Commerce.Payments.Verifone.HostedPages.Extensions;
using Geta.Commerce.Payments.Verifone.HostedPages.Models;
using Geta.Verifone;
using Geta.Verifone.Extensions;
using Geta.Verifone.Security;
using Mediachase.Commerce;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Orders.Dto;
using Mediachase.Commerce.Orders.Managers;
using HashAlgorithm = Geta.Verifone.Security.HashAlgorithm;

namespace Geta.Commerce.Payments.Verifone.HostedPages
{
    [ServiceConfiguration(typeof(IVerifonePaymentService))]
    public class DefaultVerifonePaymentService : IVerifonePaymentService
    {
        protected readonly IMarket CurrentMarket;
        protected readonly OrderContext OrderContext;

        public DefaultVerifonePaymentService(ICurrentMarket currentMarket)
        {
            if (currentMarket == null) throw new ArgumentNullException("currentMarket");
            CurrentMarket = currentMarket.GetCurrentMarket();
            OrderContext = OrderContext.Current;
        }

        #region Public Methods

        public virtual string GetPaymentLocale(CultureInfo culture)
        {
            switch (culture.Name)
            {
                case "no":
                case "nb":
                case "nn":
                case "nb-NO":
                case "nn-NO":
                    return "no_NO";
                case "sv":
                case "sv-SE":
                    return "sv_SE";
                case "sv-FI":
                    return "sv_FI";
                case "fi-FI":
                case "fi":
                    return "fi_FI";
                case "da-DK":
                case "da":
                    return "dk_DK";
                default:
                    return "en_GB";
            }
        }

        public virtual string GetPaymentUrl(VerifonePaymentRequest payment)
        {
            PaymentMethodDto paymentMethodDto = GetPaymentMethodDto(payment);

            if (paymentMethodDto == null)
            {
                return null;
            }

            var isProduction = paymentMethodDto.IsVerifoneProduction();

            if (isProduction)
            {
                return Task.Run(() => GetOnlinePaymentNodeUrl(paymentMethodDto)).Result;
            }

            return paymentMethodDto.GetPaymentTestUrl();
        }

        protected virtual async Task<string> GetOnlinePaymentNodeUrl(PaymentMethodDto paymentMethod)
        {
            string node1Url = paymentMethod.GetPaymentNode1Url();
            string node2Url = paymentMethod.GetPaymentNode2Url();

            if (string.IsNullOrWhiteSpace(node1Url) && string.IsNullOrWhiteSpace(node2Url))
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(node2Url))
            {
                return node1Url;
            }

            if (string.IsNullOrWhiteSpace(node1Url))
            {
                return node2Url;
            }

            string onlineNodeUrl = null;

            using (var client = new HttpClient { Timeout = TimeSpan.FromSeconds(3)})
            {
                try
                {
                    using (HttpResponseMessage response = await client.GetAsync(node1Url))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();

                            if (string.IsNullOrEmpty(content))
                            {
                                onlineNodeUrl = node1Url;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // Node 1 is most likely offline
                }

                if (onlineNodeUrl == null)
                {
                    try
                    {
                        using (HttpResponseMessage response = await client.GetAsync(node2Url))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string content = await response.Content.ReadAsStringAsync();

                                if (string.IsNullOrEmpty(content))
                                {
                                    onlineNodeUrl = node2Url;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // Node 1 is most likely offline
                    }
                }
            }

            return onlineNodeUrl ?? node1Url; // Fallback to node 1.
        }

        /// <summary>
        /// Initialize a payment request from an <see cref="OrderGroup"/> instance. Adds order number, amount, timestamp, buyer information etc.
        /// </summary>
        /// <param name="payment">The <see cref="VerifonePaymentRequest"/> instance to initialize</param>
        /// <param name="orderGroup"><see cref="OrderGroup"/></param>
        public virtual void InitializePaymentRequest(VerifonePaymentRequest payment, OrderGroup orderGroup)
        {
            OrderAddress billingAddress = FindBillingAddress(payment, orderGroup);
            OrderAddress shipmentAddress = FindShippingAddress(payment, orderGroup);

            payment.OrderTimestamp = orderGroup.Created.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            payment.MerchantAgreementCode = this.GetMerchantAgreementCode(payment);
            payment.PaymentLocale = this.GetPaymentLocale(ContentLanguage.PreferredCulture);
            payment.OrderNumber = orderGroup.OrderGroupId.ToString(CultureInfo.InvariantCulture.NumberFormat);

            payment.OrderCurrencyCode = this.IsProduction(payment) 
                ? Iso4217Lookup.LookupByCode(CurrentMarket.DefaultCurrency.CurrencyCode).Number.ToString()
                : "978";

            payment.OrderGrossAmount = orderGroup.Total.ToVerifoneAmountString();
            payment.OrderNetAmount = (orderGroup.Total - orderGroup.TaxTotal).ToVerifoneAmountString();
            payment.OrderVatAmount = orderGroup.TaxTotal.ToVerifoneAmountString();

            payment.BuyerFirstName = billingAddress != null
                ? billingAddress.FirstName
                : null;

            payment.BuyerLastName = billingAddress != null
                ? billingAddress.LastName
                : null;

            payment.OrderVatPercentage = "0";
            payment.PaymentMethodCode = "";
            payment.SavedPaymentMethodId = "";
            payment.StyleCode = "";
            payment.RecurringPayment = "0";
            payment.DeferredPayment = "0";
            payment.SavePaymentMethod = "0";
            payment.SkipConfirmationPage = "0";

            string phoneNumber = billingAddress != null
                ? billingAddress.DaytimePhoneNumber ?? billingAddress.EveningPhoneNumber
                : null;

            if (string.IsNullOrWhiteSpace(phoneNumber) == false)
                payment.BuyerPhoneNumber = phoneNumber;

            payment.BuyerEmailAddress = billingAddress != null
                ? billingAddress.Email ?? orderGroup.CustomerName
                : string.Empty;

            if (payment.BuyerEmailAddress.IndexOf('@') < 0)
            {
                payment.BuyerEmailAddress = null;
            }

            payment.DeliveryAddressLineOne = shipmentAddress != null
                ? shipmentAddress.Line1
                : null;

            if (shipmentAddress != null && string.IsNullOrWhiteSpace(shipmentAddress.Line2) == false)
                payment.DeliveryAddressLineTwo = shipmentAddress.Line2;

            payment.DeliveryAddressPostalCode = shipmentAddress != null
                ? shipmentAddress.PostalCode
                : null;

            payment.DeliveryAddressCity = shipmentAddress != null
                ? shipmentAddress.City
                : null;

            payment.DeliveryAddressCountryCode = "246";

            ApplyPaymentMethodConfiguration(payment);
        }

        protected virtual OrderAddress FindShippingAddress(VerifonePaymentRequest payment, OrderGroup orderGroup)
        {
            OrderForm orderForm = FindCorrectOrderForm(payment.PaymentMethodId, orderGroup.OrderForms);
            Shipment shipment = orderForm.Shipments.FirstOrDefault();

            if (shipment != null)
            {
                OrderAddress shipmentAddress = orderGroup.OrderAddresses.FirstOrDefault(x => x.Name == shipment.ShippingAddressId);

                if (shipmentAddress != null)
                {
                    return shipmentAddress;
                }
            }

            return FindBillingAddress(payment, orderGroup);
        }
        
        protected virtual OrderAddress FindBillingAddress(VerifonePaymentRequest payment, OrderGroup orderGroup)
        {
            OrderForm orderForm = FindCorrectOrderForm(payment.PaymentMethodId, orderGroup.OrderForms);

            OrderAddress billingAddress = orderGroup.OrderAddresses.FirstOrDefault(x => x.Name == orderForm.BillingAddressId);

            if (billingAddress == null)
            {
                billingAddress = orderGroup.OrderAddresses.FirstOrDefault();
            }

            return billingAddress;
        }

        protected virtual OrderForm FindCorrectOrderForm(Guid paymentMethodId, OrderFormCollection orderForms)
        {
            foreach (OrderForm orderForm in orderForms)
            {
                Payment payment = orderForm.Payments.FirstOrDefault(x => x.PaymentMethodId == paymentMethodId);

                if (payment != null)
                {
                    return orderForm;
                }
            }

            return orderForms.First();
        }

        /// <summary>
        /// Validates a successful payment against the order.
        /// </summary>
        /// <param name="response">The success response posted from Verifone <see cref="PaymentSuccessResponse"/></param>
        /// <returns><see cref="StatusCode"/></returns>
        public virtual StatusCode ValidateSuccessReponse(PaymentSuccessResponse response)
        {
            OrderGroup order = this.OrderContext.GetCart(int.Parse(response.OrderNumber));

            if (order == null)
            {
                return StatusCode.OrderNotFound;
            }

            if (string.IsNullOrWhiteSpace(response.TransactionNumber))
            {
                return StatusCode.TransactionNumberMissing;
            }

            if (string.IsNullOrWhiteSpace(response.InterfaceVersion))
            {
                return StatusCode.InterfaceVersionMissing;
            }

            if (string.IsNullOrWhiteSpace(response.OrderCurrencyCode))
            {
                return StatusCode.OrderCurrencyCodeMissing;
            }

            if (string.IsNullOrWhiteSpace(response.OrderGrossAmount))
            {
                return StatusCode.OrderGrossAmountMissing;
            }

            if (string.IsNullOrWhiteSpace(response.SoftwareVersion))
            {
                return StatusCode.SoftwareVersionMissing;
            }

            if (string.IsNullOrWhiteSpace(response.OrderTimestamp))
            {
                return StatusCode.OrderTimestampMissing;
            }

            if (response.OrderTimestamp.Equals(order.Created.ToVerifoneDateString(), StringComparison.InvariantCulture) == false)
            {
                return StatusCode.OrderTimestampMismatch;
            }

            if (response.OrderGrossAmount.Equals(order.Total.ToVerifoneAmountString()) == false)
            {
                return StatusCode.OrderGrossAmountMismatch;
            }

            // TODO: Find a way to verify this both for test and production. Not used at the moment to simplify testing but needs to be implemented before release.

            //if (response.OrderCurrencyCode.Equals(Iso4217Lookup.LookupByCode(order.BillingCurrency).Number.ToString(CultureInfo.InvariantCulture)) == false)
            //{
            //    return StatusCode.OrderCurrencyCodeMismatch;
            //}

            if (this.VerifySignatures(response) == false)
            {
                return StatusCode.SignatureInvalid;
            }

            return StatusCode.OK;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Applies payment method parameters based on an initialized <see cref="VerifonePaymentRequest"/> with a valid PaymentMethodId property value.
        /// </summary>
        /// <param name="payment"><see cref="VerifonePaymentRequest"/> instance.</param>
        protected virtual void ApplyPaymentMethodConfiguration(VerifonePaymentRequest payment)
        {
            payment.InterfaceVersion = "3";
            payment.ShortCancelUrl = GetConfigurationValue(payment, VerifoneConstants.Configuration.CancelUrl, "/error").ToExternalUrl();
            payment.ShortErrorUrl = GetConfigurationValue(payment, VerifoneConstants.Configuration.ErrorUrl, "/error").ToExternalUrl();
            payment.ShortExpiredUrl = GetConfigurationValue(payment, VerifoneConstants.Configuration.ExpiredUrl, "/error").ToExternalUrl();
            payment.ShortRejectedUrl = GetConfigurationValue(payment, VerifoneConstants.Configuration.RejectedUrl, "/error").ToExternalUrl();
            payment.ShortSuccessUrl = GetConfigurationValue(payment, VerifoneConstants.Configuration.SuccessUrl, "/success").ToExternalUrl();
            payment.Software = GetConfigurationValue(payment, VerifoneConstants.Configuration.WebShopName, "My web shop");
            payment.SoftwareVersion = "1.0.0";
        }

        protected virtual string GetMerchantAgreementCode(VerifonePaymentRequest payment)
        {
            var paymentMethodDto = GetPaymentMethodDto(payment);

            return paymentMethodDto != null
                ? paymentMethodDto.GetMerchantAgreementCode()
                : "demo-agreement-code";
        }

        protected virtual string GetConfigurationValue(VerifonePaymentRequest payment, string parameterName, string defaultValue = null)
        {
            PaymentMethodDto paymentMethodDto = GetPaymentMethodDto(payment);

            return paymentMethodDto != null
                ? paymentMethodDto.GetParameter(parameterName, defaultValue)
                : defaultValue;
        }

        protected virtual bool IsProduction(VerifonePaymentRequest payment)
        {
            PaymentMethodDto paymentMethodDto = GetPaymentMethodDto(payment);

            return paymentMethodDto.GetParameter(VerifoneConstants.Configuration.IsProduction, "false") == "true";
        }

        protected virtual PaymentMethodDto GetPaymentMethodDto(VerifonePaymentRequest payment)
        {
            if (payment.PaymentMethodId != Guid.Empty)
            {
                PaymentMethodDto paymentMethodDto = PaymentManager.GetPaymentMethod(payment.PaymentMethodId);
                return paymentMethodDto;
            }

            return null;
        }

        protected virtual bool VerifySignatures(PaymentSuccessResponse response)
        {
            SortedDictionary<string, string> parameters = response.GetParameters();

            string signatureOne = response.GetParameterValue(VerifoneConstants.ParameterName.SignatureOne);
            string signatureTwo = response.GetParameterValue(VerifoneConstants.ParameterName.SignatureTwo);

            if (string.IsNullOrWhiteSpace(signatureOne) || string.IsNullOrWhiteSpace(signatureTwo))
            {
                return false;
            }

            SortedDictionary<string, string> parametersCopy = new SortedDictionary<string, string>(parameters);

            parametersCopy.Remove(VerifoneConstants.ParameterName.SignatureOne);
            parametersCopy.Remove(VerifoneConstants.ParameterName.SignatureTwo);

            string content = PointSignatureUtil.FormatParameters(parametersCopy);
            RSA publicKey = PointCertificateUtil.GetPointPublicKey();
            //X509Certificate2 certificate = PointCertificateUtil.GetPointCertificate();

            return PointSignatureUtil.VerifySignature(publicKey, signatureOne, content, HashAlgorithm.SHA1)
                    && PointSignatureUtil.VerifySignature(publicKey, signatureTwo, content, HashAlgorithm.SHA512);
        }

        #endregion
    }
}