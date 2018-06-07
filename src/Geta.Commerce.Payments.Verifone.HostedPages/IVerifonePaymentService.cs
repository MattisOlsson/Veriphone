using System.Globalization;
using EPiServer.Commerce.Order;
using Geta.Commerce.Payments.Verifone.HostedPages.Models;

namespace Geta.Commerce.Payments.Verifone.HostedPages
{
    public interface IVerifonePaymentService
    {
        string GetPaymentLocale(CultureInfo culture);
        string GetPaymentUrl(VerifonePaymentRequest payment);
        void InitializePaymentRequest(VerifonePaymentRequest payment, IOrderGroup orderGroup);
        StatusCode ValidateSuccessReponse(PaymentSuccessResponse response);
    }
}