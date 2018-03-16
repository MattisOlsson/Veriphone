using System;
using System.Collections;

namespace Geta.Verifone.Security
{
    /// <summary>
    /// Custom String sorter to work around .NET sort bug which ignores special
    /// characters like '-'.
    /// </summary>
    public class StringComparer : IComparer
    {
        /** The sort order. */
        private string SORT_ORDER = "01234567890_=";

        public int Compare(object x, object y)
        {
            if ((x is string) && (y is string))
            {
                string xx = ((string)x).Replace("-", "#");
                string yy = ((string)y).Replace("-", "#");
                return xx.CompareTo(yy);
            }
            return -1;
        }
    }
}