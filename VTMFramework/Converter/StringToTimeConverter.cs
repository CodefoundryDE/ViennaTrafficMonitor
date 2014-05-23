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
            string text = (string)value;
            if (text == null || String.IsNullOrWhiteSpace(text)) return "00:00";
            DateTime datetime = DateTime.ParseExact(text, "yyyy-MM-dd'T'HH:mm:ss.fffzz'00'", CultureInfo.InvariantCulture);
            return datetime.ToString("HH:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return null;
        }
    }
}
