using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace VtmFramework.Converter {

    [ValueConversion(typeof(string), typeof(string))]
    public class StringToTimeConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            string input = (string)value;
            if (String.IsNullOrEmpty(input)) {
                return string.Empty;
            }
            var datetime = DateTime.ParseExact(input, "yyyy-MM-dd'T'HH:mm:ss.fffzz'00'", CultureInfo.InvariantCulture);
            return datetime.ToString("HH:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return null;
        }
    }
}
