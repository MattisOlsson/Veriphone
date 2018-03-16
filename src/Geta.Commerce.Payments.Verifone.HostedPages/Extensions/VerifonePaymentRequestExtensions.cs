using EPiServer.ServiceLocation;
using Geta.Commerce.Payments.Verifone.HostedPages.Models;
using Mediachase.Commerce.Orders;

namespace Geta.Commerce.Payments.Verifone.HostedPages.Extensions
{
    public static class VerifonePaymentRequestExtensions
    {
        public static void Initialize(this VerifonePaymentRequest request, OrderGroup orderGroup)
        {
            var paymentService = ServiceLocator.Current.GetInstance<IVerifonePaymentService>();
            paymentService.InitializePaymentRequest(request, orderGroup);
        }

        public static string GetPaymentUrl(this VerifonePaymentRequest payment)
        {
            var paymentService = ServiceLocator.Current.GetInstance<IVerifonePaymentService>();
            return paymentService.GetPaymentUrl(payment);
        }
    }
}