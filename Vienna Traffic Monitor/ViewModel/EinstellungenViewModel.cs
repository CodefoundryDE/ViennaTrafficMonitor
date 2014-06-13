using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using VtmFramework.Command;
using VtmFramework.Logging;
using VtmFramework.ViewModel;
using System.Windows.Forms;

namespace ViennaTrafficMonitor.ViewModel {

    public class EinstellungenViewModel : AbstractViewModel {

        public EinstellungenViewModel() {
            IsChecked = false;
        }

        #region Info
        public event EventHandler Info;

        private void OnInfo() {
            EventHandler handler = Info;
            if (handler != null) {
                handler(this, new EventArgs());
            }
            // Einstellungen schließen
            IsChecked = false;
        }

        public ICommand InfoCommand {
            get { return new DelegateCommand(OnInfo); }
        }
        #endregion

        #region Beenden
        public event EventHandler Beenden;

        private void OnBeenden() {
            EventHandler handler = Beenden;
            if (handler != null) {
                handler(this, new EventArgs());
            }
        }

        public ICommand BeendenCommand {
            get { return new DelegateCommand(OnBeenden); }
        }
        #endregion

        public string Theme {
            get { return Properties.Settings.Default.Theme; }
            set {
                Properties.Settings.Default.Theme = value;
                Properties.Settings.Default.Save();
                _changeDictionary();
            }
        }

        public static bool Dummy {
            get { return Properties.Settings.Default.DummyRequester; }
            set {
                Properties.Settings.Default.DummyRequester = value;
                Properties.Settings.Default.Save();
            }
        }

        public int Monitor {
            get { return Properties.Settings.Default.Monitor; }
            set {
                Properties.Settings.Default.Monitor = value;
                Properties.Settings.Default.Save();
                _changeMonitor();
            }
        }

        private bool _isChecked;
        public bool IsChecked {
            get { return _isChecked; }
            set {
                _isChecked = value;
                RaisePropertyChangedEvent("IsChecked");
            }
        }

        public static ICollection<string> AvailableDictionaries {
            get { return _getDictionaries(); }
        }


        private static ICollection<string> _getDictionaries() {
            ICollection<string> dict = new List<string>();
            dict.Add("Light");
            dict.Add("Dark");
            return dict;
        }

        private void _changeDictionary() {
            //var uri = new Uri("pack://siteoforigin:,,,/Themes/" + Theme + ".xaml", UriKind.RelativeOrAbsolute);
            //ResourceDictionary dict = new ResourceDictionary() { Source = uri };
            ResourceDictionary dict;
            try {
                // Das letzte Theme (falls vorhanden) entfernen
                System.Windows.Application.Current.Resources.MergedDictionaries.RemoveAt(1);
            } catch (ArgumentOutOfRangeException) { }
            using (var fs = new FileStream("Themes/" + Theme + ".xaml", FileMode.Open, FileAccess.Read, FileShare.Read)) {
                dict = (ResourceDictionary)XamlReader.Load(fs);
                // Neues Theme hinzufügen
            }
            // Neues Theme hinzufügen
            System.Windows.Application.Current.Resources.MergedDictionaries.Add(dict);
        }

        public static ICollection<string> AvailableMonitors {
            get { return _getMonitors(); }
        }

        private static ICollection<string> _getMonitors() {
            ICollection<string> screens = new List<string>();

            Screen[] allScreens = Screen.AllScreens;

            for (int i = 0; i < allScreens.Length; i++) {
                if (allScreens[i].Primary) {
                    screens.Add("Hauptbildschirm");
                } else {
                    screens.Add("Monitor " + (i + 1));
                }
            }

            return screens;
        }

        private void _changeMonitor() {
            Screen screen;
            try {
                screen = Screen.AllScreens[Monitor];
            } catch (IndexOutOfRangeException e) {
                // Ausweichen auf den Hauptbildschirm
                screen = Screen.PrimaryScreen;
                Monitor = 0;
                IVtmLogger logger = VtmLoggerFactory.GetInstance();
                logger.Warning(e.Message);
            }
            Window MainWindow = System.Windows.Application.Current.MainWindow;
            MainWindow.WindowState = WindowState.Normal;
            MainWindow.Top = screen.WorkingArea.Top;
            MainWindow.Left = screen.WorkingArea.Left;
            MainWindow.WindowState = WindowState.Maximized;
            MainWindow.WindowStyle = WindowStyle.None;
        }

    }

}
