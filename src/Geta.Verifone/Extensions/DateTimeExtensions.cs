using System;
using System.Globalization;

namespace Geta.Verifone.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToVerifoneDateString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }
    }
}