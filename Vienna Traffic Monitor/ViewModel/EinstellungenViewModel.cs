using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VtmFramework.ViewModel;

namespace ViennaTrafficMonitor.ViewModel {

    public class EinstellungenViewModel : AbstractViewModel {

        public ResourceDictionary Theme {
            get { return Properties.Settings.Default.Theme; }
            set {
                Properties.Settings.Default.Theme = value;
                Properties.Settings.Default.Save();
            }
        }

        public bool Dummy {
            get { return Properties.Settings.Default.DummyRequester; }
            set {
                Properties.Settings.Default.DummyRequester = value;
                Properties.Settings.Default.Save();
            }
        }



        public IDictionary<string, ResourceDictionary> AvailableDictionaries {
            get { return _getDictionaries(); }
            //set { myVar = value; }
        }


        private IDictionary<string, ResourceDictionary> _getDictionaries() {
            IDictionary<string, ResourceDictionary> dict = new Dictionary<string, ResourceDictionary>();
            var light = new Uri("pack://siteoforigin:,,,/Themes/Light.xaml", UriKind.RelativeOrAbsolute);
            var dark = new Uri("pack://siteoforigin:,,,/Themes/Dark.xaml", UriKind.RelativeOrAbsolute);
            dict.Add("Light", new ResourceDictionary() { Source = light });
            dict.Add("Dark", new ResourceDictionary() { Source = dark });
            return dict;
        }

    }

}
