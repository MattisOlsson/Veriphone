using System;
using System.Globalization;
using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Geta.Commerce.Payments.Verifone.HostedPages.Extensions;
using Geta.Commerce.Payments.Verifone.HostedPages.Models;
using Mediachase.Commerce.Orders;

namespace Geta.Commerce.Payments.Verifone.HostedPages.Mvc.Controllers
{
    public abstract class PaymentSuccessControllerBase<T> : ContentController<T> where T: IContent, IRoutable
    {
        [HttpPost]
        public virtual ActionResult Index(VerifonePaymentRequest response)
        {
            return View();
        }

        /// <summary>
        /// Validates the success response against the saved order.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        protected virtual StatusCode ValidateResponse(PaymentSuccessResponse response)
        {
            PurchaseOrder purchaseOrder = PurchaseOrder.LoadByOrderGroupId(int.Parse(response.OrderNumber));

            if (purchaseOrder == null)
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

            if (response.OrderTimestamp.Equals(purchaseOrder.GetString(MetadataConstants.OrderTimestamp), StringComparison.InvariantCulture) == false)
            {
                return StatusCode.OrderTimestampMismatch;
            }

            if (response.OrderGrossAmount.Equals(purchaseOrder.Total.ToVerifoneAmountString()) == false)
            {
                return StatusCode.OrderGrossAmountMismatch;
            }

            if (response.OrderCurrencyCode.Equals(purchaseOrder.GetString(MetadataConstants.OrderCurrencyCode)) == false)
            {
                return StatusCode.OrderCurrencyCodeMismatch;
            }

            // TODO: Validate signature-one and signature-two

            return StatusCode.OK;
        }
    }
}