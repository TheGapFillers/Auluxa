using System;
using System.Collections.Generic;
using System.Linq;

namespace Auluxa.WebApp.Tools
{
    public static class ArrayExtensions
    {
        public static IEnumerable<string> SplitAndTrim(this string stringToSplit, params char[] separator) =>
            stringToSplit?
            .Split(separator, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim()) ?? new List<string>().AsEnumerable();

        public static IEnumerable<string> SplitAndTrim(this string stringToSplit, params string[] separator) =>
            stringToSplit?
            .Split(separator, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim()) ?? new List<string>().AsEnumerable();
    }
}