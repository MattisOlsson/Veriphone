using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CustomStringComparer
/// </summary>
public class PointStringComparer : IComparer<String>
{
    // the sort order
    const String SORT_ORDER = "0123456789-_abcdefghijklmnopqrstuvwxyz";

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
            else
            {
                return characterDiff;
            }
        }

        if (x.Length > y.Length)
        {
            return 1;
        }
        else if (x.Length < y.Length)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

}