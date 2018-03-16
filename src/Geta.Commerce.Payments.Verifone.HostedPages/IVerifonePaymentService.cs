using System.Globalization;
using Geta.Commerce.Payments.Verifone.HostedPages.Models;
using Mediachase.Commerce.Orders;

namespace Geta.Commerce.Payments.Verifone.HostedPages
{
    public interface IVerifonePaymentService
    {
        string GetPaymentLocale(CultureInfo culture);
        string GetPaymentUrl(VerifonePaymentRequest payment);
        void InitializePaymentRequest(VerifonePaymentRequest payment, OrderGroup orderGroup);
        StatusCode ValidateSuccessReponse(PaymentSuccessResponse response);
    }
}