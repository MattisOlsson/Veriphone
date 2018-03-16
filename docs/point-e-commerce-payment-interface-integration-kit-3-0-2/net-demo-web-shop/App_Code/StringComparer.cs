using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

/// <summary>
/// Custom String sorter to work around .NET sort bug which ignores special
/// characters like '-'.
/// </summary>
public class StringComparer : IComparer
{
    /** The sort order. */
    private String SORT_ORDER = "01234567890_=";

    public int Compare(object x, object y)
    {
        if ((x is String) && (y is String))
        {
            String xx = ((String)x).Replace("-", "#");
            String yy = ((String)y).Replace("-", "#");
            return ((String)xx).CompareTo(yy);
        }
        return -1;
    }
}