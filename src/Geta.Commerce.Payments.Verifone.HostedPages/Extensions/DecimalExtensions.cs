using System;
using System.Globalization;

namespace Geta.Commerce.Payments.Verifone.HostedPages.Extensions
{
    public static class DecimalExtensions
    {
        public static string ToVerifoneAmountString(this decimal number)
        {
            return Math.Round(number, 2).ToString(CultureInfo.InvariantCulture).Replace(".", "");
        }
    }
}