using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace VtmFramework.Converter {

    [ValueConversion(typeof(bool), typeof(double))]
    public class BoolToOpacityConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            bool boolvalue = (bool)value;
            if (boolvalue) {
                return 1.0;
            } else {
                return 0.5;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return null;
        }

    }

}
