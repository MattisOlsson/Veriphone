using System;
using System.Web;
using EPiServer.Commerce.Order;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Plugins.Payment;

namespace Geta.Commerce.Payments.Verifone.HostedPages
{
    public class VerifoneHostedPagesPaymentGateway : AbstractPaymentGateway, IPaymentPlugin
    {
        public IOrderGroup OrderGroup { get; set; }

        public virtual bool ProcessPayment(IPayment payment, ref string message)
        {
            return ProcessPayment(OrderGroup, payment).IsSuccessful;
        }

        public override bool ProcessPayment(Payment payment, ref string message)
        {
            OrderGroup = payment.Parent.Parent;

            return ProcessPayment(OrderGroup, payment).IsSuccessful;
        }

        public PaymentProcessingResult ProcessPayment(IOrderGroup orderGroup, IPayment payment)
        {
            if (payment.TransactionType != TransactionType.Authorization.ToString() ||
                payment.Status != PaymentStatus.Pending.ToString())
            {
                return PaymentProcessingResult.CreateSuccessfulResult(string.Empty);
            }

            var settings = ServiceLocator.Current.GetInstance<IVerifoneSettings>();
            if (settings == null)
                throw new ArgumentException($"Configure a default instance for {nameof(IVerifoneSettings)}", nameof(settings));

            var urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
            var redirectUrl = urlResolver.GetUrl(settings.PaymentRedirectPage);

            if (!string.IsNullOrEmpty(settings.PaymentRedirectAction))
            {
                redirectUrl = VirtualPathUtility.AppendTrailingSlash(redirectUrl);
                redirectUrl += settings.PaymentRedirectAction;
            }

            return PaymentProcessingResult.CreateSuccessfulResult("Redirection is required", redirectUrl);
        }
    }
}