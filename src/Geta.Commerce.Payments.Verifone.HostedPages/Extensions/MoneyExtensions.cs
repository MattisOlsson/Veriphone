using Mediachase.Commerce;

namespace Geta.Commerce.Payments.Verifone.HostedPages.Extensions
{
    public static class MoneyExtensions
    {
        public static string ToVerifoneAmountString(this Money money)
        {
            return money.Amount.ToVerifoneAmountString();
        }
    }
}