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
            string temp1 = str1.Trim();
            string temp2 = str2.Trim();

            StringBuilder result = new StringBuilder();
            result.Append(temp1);
            if (!temp1.Equals("") && !temp2.Equals("")) result.Append(separator);
            result.Append(temp2);
            return result.ToString();
        }

    }

}
