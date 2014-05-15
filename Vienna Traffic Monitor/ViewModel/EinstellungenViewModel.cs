using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VtmFramework.ViewModel;

namespace ViennaTrafficMonitor.ViewModel {

    public class EinstellungenViewModel : AbstractViewModel {

        public string Theme {
            get { return Properties.Settings.Default.Theme; }
            set {
                Properties.Settings.Default.Theme = value;
                Properties.Settings.Default.Save();
                _changeDictionary();
            }
        }

        public bool Dummy {
            get { return Properties.Settings.Default.DummyRequester; }
            set {
                Properties.Settings.Default.DummyRequester = value;
                Properties.Settings.Default.Save();
            }
        }



        public ICollection<string> AvailableDictionaries {
            get { return _getDictionaries(); }
        }


        private ICollection<string> _getDictionaries() {
            ICollection<string> dict = new List<string>();
            dict.Add("Light");
            dict.Add("Dark");
            return dict;
        }

        private void _changeDictionary() {
            var uri = new Uri("pack://siteoforigin:,,,/Themes/" + Theme + ".xaml", UriKind.RelativeOrAbsolute);
            ResourceDictionary dict = new ResourceDictionary() { Source = uri };
            try {
                // Das letzte Theme (falls vorhanden) entfernen
                Application.Current.Resources.MergedDictionaries.RemoveAt(1);
            } catch (Exception) { }
            // Neues Theme hinzufügen
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }

    }

}
