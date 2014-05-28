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

        /// <summary>
        /// Diese Methode zählt einen Buchstaben nach oben. Angebbar sind Ober- und Untergrenze.
        /// </summary>
        /// <param name="character">Char, der hochgezählt werden soll.</param>
        /// <param name="min">Untergrenze</param>
        /// <param name="max">Obergrenze</param>
        /// <exception cref="ArgumentException">Wird geworfen wenn min > max</exception>
        /// <exception cref="ArgumentOutOfRangeException">Wird geworfen wenn character nicht zwischen min und max</exception>
        /// <returns>Char um 1 inkrementiert</returns>
        public static char AsciiInc(char character, char min, char max) {
            if (min > max) throw new
                ArgumentException("Der Parameter 'min' ist größer als der Parameter 'max'.");
            if (character < min) return min;
            if (character > max) return max;

            int diff = max - min + 1;
            return (char)(min + ((character - min + 1) % diff));
        }

        /// <summary>
        /// Filtert alle Umlaute aus einem Text und ersetzt sie, z.B. 'Ä' => 'Ae'
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string UmlautFilter(string text) {
            if (text == null || String.IsNullOrWhiteSpace(text)) {
                return "";
            } else {
                return text
                    .Replace("Ä", "Ae")
                    .Replace("Ö", "Oe")
                    .Replace("Ü", "Ue")
                    .Replace("ä", "ae")
                    .Replace("ö", "oe")
                    .Replace("ü", "ue")
                    .Replace("ß", "ss");
            }
        }

    }

}
