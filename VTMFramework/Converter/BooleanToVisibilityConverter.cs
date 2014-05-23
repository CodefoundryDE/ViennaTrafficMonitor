using System.Windows.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VtmFramework.Converter {
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : IValueConverter {



        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            bool boolValue;
            if (value == null) {
                boolValue = false;
            } else boolValue = (bool)value;

            if (!boolValue) return Visibility.Collapsed;
            return Visibility.Visible;

            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
