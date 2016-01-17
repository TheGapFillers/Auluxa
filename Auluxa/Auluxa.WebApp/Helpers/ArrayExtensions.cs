using System;
using System.Collections.Generic;
using System.Linq;

namespace Auluxa.WebApp.Helpers
{
    public static class ArrayExtensions
    {
        public static IEnumerable<string> SplitAndTrim(this string stringToSplit, params char[] separator)
        {
            if (stringToSplit == null)
                return new List<string>().AsEnumerable();

            return stringToSplit.Split(separator, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());
        }
    }
}
