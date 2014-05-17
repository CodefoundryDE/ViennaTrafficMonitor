using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using VtmFramework.Library;

namespace VtmFramework.View {

    /// <summary>
    /// Interaktionslogik für SplitFlapDisplay.xaml
    /// </summary>
    public partial class SplitFlapDisplay : UserControl {

        #region DependencyProperty PanelCount
        public static readonly DependencyProperty PanelCountProperty = DependencyProperty.Register("PanelCount", typeof(int), typeof(SplitFlapDisplay), new FrameworkPropertyMetadata(1, OnPanelCountChanged));

        public int PanelCount {
            get { return (int)this.GetValue(PanelCountProperty); }
            set {
                this.SetValue(PanelCountProperty, value);
                _createPanels();
            }
        }

        private static void OnPanelCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((SplitFlapDisplay)d).PanelCount = (int)e.NewValue;
        }
        #endregion

        #region DependencyProperty Text
        public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached("Text", typeof(string), typeof(SplitFlapDisplay), new FrameworkPropertyMetadata(OnTextChanged));

        public string Text {
            get { return (string)this.GetValue(TextProperty); }
            set {
                this.SetValue(TextProperty, value);
            }
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            SplitFlapDisplay display = (SplitFlapDisplay)d;
            display.OnTextChanged();
        }

        //private static object OnTextUpdated(DependencyObject d, object baseValue) {
        //    SplitFlapDisplay display = (SplitFlapDisplay)d;
        //    display.OnTextChanged();
        //    return baseValue;
        //}
        #endregion

        private Timer _timer;

        public List<SplitFlapPanel> Panels { get; private set; }

        private char[] _charsCurrent;
        private char[] _charsFinal;

        public SplitFlapDisplay() {
            InitializeComponent();
            Panels = new List<SplitFlapPanel>();
            _timer = new Timer(_tick);
        }

        private void _createPanels() {
            for (int i = 0; i < 5; i++) {
                SplitFlapPanel panel = new SplitFlapPanel();
                panel.Content = "A";
                panel.Width = 50;
                panel.BorderThickness = new Thickness(1);
                panel.BorderBrush = new SolidColorBrush(Colors.Black);
                Panels.Add(panel);
                MainPanel.Children.Add(panel);
            }
            MainPanel.UpdateLayout();
        }

        private void OnTextChanged() {
            string text = Text == null ? "" : Text;
            text = text.PadRight(PanelCount, ' ');
            if (_charsCurrent == null) {
                _charsCurrent = text.ToCharArray(0, PanelCount);
            }
            _charsFinal = text.ToCharArray(0, PanelCount);

            for (int i = 0; i < PanelCount; i++) {
                Panels[i].Content = _charsCurrent[i].ToString();
            }
            _timer.Change(0, 200);
        }

        private void _tick(object state) {
            bool action = false;
            Parallel.For(0, _charsCurrent.Length, (int i) => {
                if (_charsCurrent[i] != _charsFinal[i]) {
                    _charsCurrent[i] = StrLib.AsciiInc(_charsCurrent[i], ' ', 'z');
                    object[] parameters = new object[] { Panels[i], _charsCurrent[i] };
                    Panels[i].Dispatcher.BeginInvoke(new updateDelegate(updatePanel), DispatcherPriority.Normal, parameters);  //Content = _chars[i].ToString();
                    action = true;
                }
            });
            if (!action) _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private delegate void updateDelegate(SplitFlapPanel panel, char character);
        private void updatePanel(SplitFlapPanel panel, char character) {
            panel.Content = character.ToString();
        }

    }

}
