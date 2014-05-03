using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtmFramework.Library {

    public static class StrLib {

        /// <summary>
        /// Fügt 2 Strings und einen Trenner dazwischen zusammen. 
        /// Der Trenner wird nur eingebaut, wenn beide Strings nicht leer sind.
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <param name="separator">Trenner</param>
        /// <returns></returns>
        public static string StrCat(string str1, string str2, string separator) {
            if (str1 == null) str1 = "";
            if (str2 == null) str2 = "";
            if (separator == null) separator = "";
            string temp1 = str1.Trim();
            string temp2 = str2.Trim();

            StringBuilder result = new StringBuilder();
            result.Append(temp1);
            if (!String.IsNullOrEmpty(temp1) && !String.IsNullOrEmpty(temp2)) result.Append(separator);
            result.Append(temp2);
            return result.ToString();
        }

    }

}
