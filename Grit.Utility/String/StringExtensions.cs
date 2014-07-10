using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Grit.Utility.String
{
    public static class StringExtensions
    {
        private static Regex _regexCamel = new Regex("[a-z][A-Z]");

        public static string ToSentenceCase(this string str)
        {
            return _regexCamel.Replace(str, m => m.Value[0] + " " + char.ToLower(m.Value[1]));
        }
        public static string ToDotString(this string str)
        {
            return _regexCamel.Replace(str, m => m.Value[0] + "." + m.Value[1]).ToLower();
        }
    }
}
