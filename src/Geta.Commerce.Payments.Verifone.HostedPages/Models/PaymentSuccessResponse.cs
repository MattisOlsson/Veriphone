using System;
using System.Linq;
using System.Web.Mvc;
using EPiServer.ServiceLocation;
using Geta.Commerce.Payments.Verifone.HostedPages.Mvc;
using Geta.Verifone;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Website;

namespace Geta.Commerce.Payments.Verifone.HostedPages.Models
{
    [ModelBinder(typeof(VerifoneModelBinder))]
    public class PaymentSuccessResponse : VerifoneParameterModelBase, IPaymentOption
    {
        public Guid PaymentMethodId { get; set; }

        [BindAlias(VerifoneConstants.ParameterName.TransactionNumber)]
        public string TransactionNumber
        {
            get { return GetParameterValue(VerifoneConstants.ParameterName.TransactionNumber); }
            set { _parameters[VerifoneConstants.ParameterName.TransactionNumber] = value; }
        }

        [BindAlias(VerifoneConstants.ParameterName.PaymentMethodCodeResponse)]
        public string PaymentMethodCode
        {
            get { return GetParameterValue(VerifoneConstants.ParameterName.PaymentMethodCodeResponse); }
            set { _parameters[VerifoneConstants.ParameterName.PaymentMethodCodeResponse] = value; }
        }

        [BindAlias(VerifoneConstants.ParameterName.OrderNumber)]
        public string OrderNumber
        {
            get { return GetParameterValue(VerifoneConstants.ParameterName.OrderNumber); }
            set { _parameters[VerifoneConstants.ParameterName.OrderNumber] = value; }
        }

        [BindAlias(VerifoneConstants.ParameterName.OrderNote)]
        public string OrderNote
        {
            get { return GetParameterValue(VerifoneConstants.ParameterName.OrderNote); }
            set { _parameters[VerifoneConstants.ParameterName.OrderNote] = value; }
        }

        [BindAlias(VerifoneConstants.ParameterName.OrderTimestamp)]
        public string OrderTimestamp
        {
            get { return GetParameterValue(VerifoneConstants.ParameterName.OrderTimestamp); }
            set { _parameters[VerifoneConstants.ParameterName.OrderTimestamp] = value; }
        }

        [BindAlias(VerifoneConstants.ParameterName.OrderCurrencyCode)]
        public string OrderCurrencyCode
        {
            get { return GetParameterValue(VerifoneConstants.ParameterName.OrderCurrencyCode); }
            set { _parameters[VerifoneConstants.ParameterName.OrderCurrencyCode] = value; }
        }

        [BindAlias(VerifoneConstants.ParameterName.OrderGrossAmount)]
        public string OrderGrossAmount
        {
            get { return GetParameterValue(VerifoneConstants.ParameterName.OrderGrossAmount); }
            set { _parameters[VerifoneConstants.ParameterName.OrderGrossAmount] = value; }
        }

        [BindAlias(VerifoneConstants.ParameterName.SoftwareVersion)]
        public string SoftwareVersion
        {
            get { return GetParameterValue(VerifoneConstants.ParameterName.SoftwareVersion); }
            set { _parameters[VerifoneConstants.ParameterName.SoftwareVersion] = value; }
        }

        [BindAlias(VerifoneConstants.ParameterName.InterfaceVersion)]
        public string InterfaceVersion
        {
            get { return GetParameterValue(VerifoneConstants.ParameterName.InterfaceVersion); }
            set { _parameters[VerifoneConstants.ParameterName.InterfaceVersion] = value; }
        }

        [BindAlias(VerifoneConstants.ParameterName.ReferenceNumber)]
        public string ReferenceNumber
        {
            get { return GetParameterValue(VerifoneConstants.ParameterName.ReferenceNumber); }
            set { _parameters[VerifoneConstants.ParameterName.ReferenceNumber] = value; }
        }

        [BindAlias(VerifoneConstants.ParameterName.SignatureOne)]
        public string SignatureOne
        {
            get { return GetParameterValue(VerifoneConstants.ParameterName.SignatureOne); }
            set { _parameters[VerifoneConstants.ParameterName.SignatureOne] = value; }
        }

        [BindAlias(VerifoneConstants.ParameterName.SignatureTwo)]
        public string SignatureTwo
        {
            get { return GetParameterValue(VerifoneConstants.ParameterName.SignatureTwo); }
            set { _parameters[VerifoneConstants.ParameterName.SignatureTwo] = value; }
        }

        [BindAlias(VerifoneConstants.ParameterName.Token)]
        public string Token
        {
            get { return GetParameterValue(VerifoneConstants.ParameterName.Token); }
            set { _parameters[VerifoneConstants.ParameterName.Token] = value; }
        }

        [BindAlias(VerifoneConstants.ParameterName.FilingCode)]
        public string FilingCode
        {
            get { return GetParameterValue(VerifoneConstants.ParameterName.FilingCode); }
            set { _parameters[VerifoneConstants.ParameterName.FilingCode] = value; }
        }

        [BindAlias(VerifoneConstants.ParameterName.SocialSecurityNumber)]
        public string SocialSecurityNumber
        {
            get { return GetParameterValue(VerifoneConstants.ParameterName.SocialSecurityNumber); }
            set { _parameters[VerifoneConstants.ParameterName.SocialSecurityNumber] = value; }
        }

        [BindAlias(VerifoneConstants.ParameterName.BuyerEmailAddress)]
        public string BuyerEmailAddress
        {
            get { return GetParameterValue(VerifoneConstants.ParameterName.BuyerEmailAddress); }
            set { _parameters[VerifoneConstants.ParameterName.BuyerEmailAddress] = value; }
        }

        [BindAlias(VerifoneConstants.ParameterName.BuyerPhoneNumber)]
        public string BuyerPhoneNumber
        {
            get { return GetParameterValue(VerifoneConstants.ParameterName.BuyerPhoneNumber); }
            set { _parameters[VerifoneConstants.ParameterName.BuyerPhoneNumber] = value; }
        }

        [BindAlias(VerifoneConstants.ParameterName.DeliveryAddressCity)]
        public string DeliveryAddressCity
        {
            get { return GetParameterValue(VerifoneConstants.ParameterName.DeliveryAddressCity); }
            set { _parameters[VerifoneConstants.ParameterName.DeliveryAddressCity] = value; }
        }

        [BindAlias(VerifoneConstants.ParameterName.DeliveryAddressLineOne)]
        public string DeliveryAddressLineOne
        {
            get { return GetParameterValue(VerifoneConstants.ParameterName.DeliveryAddressLineOne); }
            set { _parameters[VerifoneConstants.ParameterName.DeliveryAddressLineOne] = value; }
        }

        [BindAlias(VerifoneConstants.ParameterName.DeliveryAddressLineTwo)]
        public string DeliveryAddressLineTwo
        {
            get { return GetParameterValue(VerifoneConstants.ParameterName.DeliveryAddressLineTwo); }
            set { _parameters[VerifoneConstants.ParameterName.DeliveryAddressLineTwo] = value; }
        }

        [BindAlias(VerifoneConstants.ParameterName.DeliveryAddressPostalCode)]
        public string DeliveryAddressPostalCode
        {
            get { return GetParameterValue(VerifoneConstants.ParameterName.DeliveryAddressPostalCode); }
            set { _parameters[VerifoneConstants.ParameterName.DeliveryAddressPostalCode] = value; }
        }

        [BindAlias(VerifoneConstants.ParameterName.DeliveryAddressCountryCode)]
        public string DeliveryAddressCountryCode
        {
            get { return GetParameterValue(VerifoneConstants.ParameterName.DeliveryAddressCountryCode); }
            set { _parameters[VerifoneConstants.ParameterName.DeliveryAddressCountryCode] = value; }
        }

        [BindAlias(VerifoneConstants.ParameterName.CardExpectedValidity)]
        public string CardExpectedValidity
        {
            get { return GetParameterValue(VerifoneConstants.ParameterName.CardExpectedValidity); }
            set { _parameters[VerifoneConstants.ParameterName.CardExpectedValidity] = value; }
        }

        public bool ValidateData()
        {
            var verifonePaymentService = ServiceLocator.Current.GetInstance<IVerifonePaymentService>();
            return verifonePaymentService.ValidateSuccessReponse(this) == StatusCode.OK;
        }

        public Payment PreProcess(OrderForm form)
        {
            if (form == null) throw new ArgumentNullException("form");

            if (!ValidateData())
                return null;

            var orderAddress = form.Parent.OrderAddresses.FirstOrDefault();

            var payment = new OtherPayment
            {
                PaymentMethodId = PaymentMethodId,
                PaymentMethodName = "VerifoneHostedPages",
                OrderFormId = form.OrderFormId,
                OrderGroupId = form.OrderGroupId,
                Amount = form.Total,
                Status = PaymentStatus.Pending.ToString(),
                PaymentType = PaymentType.Other,
                TransactionType = TransactionType.Authorization.ToString(),
                BillingAddressId = form.BillingAddressId,
                CustomerName = orderAddress != null
                    ? string.Format("{0} {1}", orderAddress.FirstName, orderAddress.LastName)
                    : form.Parent.CustomerName
            };

            return payment;
        }

        public bool PostProcess(OrderForm orderForm)
        {
            var payment = orderForm.Payments.FirstOrDefault(x => x.PaymentMethodId == PaymentMethodId);

            if (payment == null)
            {
                return false;
            }

            payment.TransactionID = this.TransactionNumber;
            payment.SetMetaField(MetadataConstants.FilingCode, this.FilingCode);
            payment.SetMetaField(MetadataConstants.ReferenceNumber, this.ReferenceNumber ?? string.Empty);
            payment.SetMetaField(MetadataConstants.PaymentMethodCode, this.PaymentMethodCode);
            payment.Status = PaymentStatus.Processed.ToString();
            return true;
        }
    }
}