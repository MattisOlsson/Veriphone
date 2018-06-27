using System;
using System.Globalization;

namespace Geta.Commerce.Payments.Verifone.HostedPages.Extensions
{
    public static class DecimalExtensions
    {
        public static string ToVerifoneAmountString(this decimal number)
        {
            var rounded = Math.Round(number, 2);
            var veriphoneString = rounded.ToString(CultureInfo.InvariantCulture);

            if (veriphoneString.IndexOf('.') == -1)
            {
                veriphoneString += "00";
            }

            return veriphoneString.Replace(".", "");
        }
    }
}