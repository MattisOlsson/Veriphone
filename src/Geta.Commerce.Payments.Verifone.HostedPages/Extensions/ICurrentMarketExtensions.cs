using System;
using System.Linq;
using Mediachase.Commerce;
using Mediachase.Commerce.Orders.Dto;
using Mediachase.Commerce.Orders.Managers;

namespace Geta.Commerce.Payments.Verifone.HostedPages.Extensions
{
    public static class ICurrentMarketExtensions
    {
        public static PaymentMethodDto GetVerifonePaymentMethod(this ICurrentMarket currentMarket, string currentTwoLetterIsoLanguageName)
        {
            Guid paymentMethodId = currentMarket.GetVerifonePaymentMethodId(currentTwoLetterIsoLanguageName);

            return PaymentManager.GetPaymentMethod(paymentMethodId);
        }
        public static Guid GetVerifonePaymentMethodId(this ICurrentMarket currentMarket, string currentTwoLetterIsoLanguageName)
        {
            var gatewayType = typeof(VerifoneHostedPagesPaymentGateway);
            var gatewayClass = gatewayType.FullName + ", " + gatewayType.Assembly.GetName().Name;
            var marketId = currentMarket.GetCurrentMarket().MarketId;
            var methods = PaymentManager.GetPaymentMethodsByMarket(marketId.Value).PaymentMethod.Where(c => c.IsActive);

            var verifoneRow = methods.
                Where(paymentRow => currentTwoLetterIsoLanguageName.Equals(paymentRow.LanguageId, StringComparison.OrdinalIgnoreCase)).
                Where(paymentRow => paymentRow.ClassName == gatewayClass).
                OrderBy(paymentRow => paymentRow.Ordering).ToList().FirstOrDefault();

            if (verifoneRow == null)
            {
                string error = string.Format("Missing Verifone provider settings for current language: {0} and market: {1}. Please add missing configuration to Commerce Manager.", currentTwoLetterIsoLanguageName, marketId);
                throw new Exception(error);
            }

            return verifoneRow.PaymentMethodId;
        }
    }
}