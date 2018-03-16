using System;
using System.Collections.Generic;

namespace Geta.Verifone.Security
{
    /// <summary>
    /// Summary description for CustomStringComparer
    /// </summary>
    public class PointStringComparer : IComparer<string>
    {
        // the sort order
        const string SORT_ORDER = "0123456789-_abcdefghijklmnopqrstuvwxyz";

        public int Compare(string x, string y)
        {
            for (int i = 0; i < x.Length; i++)
            {
                if (y.Length == i)
                {
                    break;
                }
                int characterDiff = (SORT_ORDER.IndexOf(x[i]) - SORT_ORDER.IndexOf(y[i]));

                if (characterDiff == 0)
                {
                    continue;
                }

                return characterDiff;
            }

            if (x.Length > y.Length)
            {
                return 1;
            }

            if (x.Length < y.Length)
            {
                return -1;
            }

            return 0;
        }

    }
}