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
            }
        }

        private static void OnPanelCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((SplitFlapDisplay)d).OnPanelCountChanged();
        }
        #endregion

        #region DependencyProperty Text
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(SplitFlapDisplay), new FrameworkPropertyMetadata(OnTextChanged));

        public string Text {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            SplitFlapDisplay display = (SplitFlapDisplay)d;
            display.OnTextChanged();
        }
        #endregion

        #region DependencyProperty Animated
        public static readonly DependencyProperty AnimatedProperty = DependencyProperty.Register("Animated", typeof(bool), typeof(SplitFlapDisplay), new FrameworkPropertyMetadata(true));

        public bool Animated {
            get { return (bool)this.GetValue(AnimatedProperty); }
            set { this.SetValue(AnimatedProperty, value); }
        }
        #endregion

        #region StartChar
        public static readonly DependencyProperty StartCharProperty = DependencyProperty.Register("StartChar", typeof(char), typeof(SplitFlapDisplay), new FrameworkPropertyMetadata('0'));

        public char StartChar {
            get { // Dieser Getter ist Threadsafe.
                try {
                    return (char)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return this.GetValue(StartCharProperty); },
                       StartCharProperty);
                } catch {
                    return (char)StartCharProperty.DefaultMetadata.DefaultValue;
                }
            }
            set { this.SetValue(StartCharProperty, value); }
        }
        #endregion

        #region EndChar
        public static readonly DependencyProperty EndCharProperty = DependencyProperty.Register("EndChar", typeof(char), typeof(SplitFlapDisplay), new FrameworkPropertyMetadata('Z'));

        public char EndChar {
            get { // Dieser Getter ist Threadsafe.
                try {
                    return (char)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(EndCharProperty); },
                       EndCharProperty);
                } catch {
                    return (char)EndCharProperty.DefaultMetadata.DefaultValue;
                }
            }
            set { this.SetValue(EndCharProperty, value); }
        }
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

        /// <summary>
        /// Wird ausgeführt wenn sich die Anzahl der Panels ändert.
        /// Erstellt eine Anzahl an Panels und legt sie auf das StackPanel.
        /// </summary>
        private void OnPanelCountChanged() {
            Panels.Clear();
            for (int i = 0; i < PanelCount; i++) {
                SplitFlapPanel panel = new SplitFlapPanel();
                panel.Content = ' ';
                panel.Width = 40;
                panel.BorderThickness = new Thickness(1);
                panel.BorderBrush = new SolidColorBrush(Colors.Black);
                Panels.Add(panel);
                MainPanel.Children.Add(panel);
            }
            MainPanel.UpdateLayout();
        }

        /// <summary>
        /// Wird aufgerufen wenn sich der Text ändert.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Mobility", "CA1601:DoNotUseTimersThatPreventPowerStateChanges")]
        private void OnTextChanged() {
            string text = String.IsNullOrWhiteSpace(Text) ? "" : Text;
            text = StrLib.UmlautFilter(text).ToUpper().PadRight(PanelCount, ' ');

            _charsFinal = text.ToCharArray(0, PanelCount);
            if (_charsCurrent == null || !Animated) {
                _charsCurrent = (char[])_charsFinal.Clone(); //text.ToCharArray(0, PanelCount);
            }

            for (int i = 0; i < PanelCount; i++) {
                object[] parameters = new object[] { i, _charsCurrent[i] };
                Panels[i].Dispatcher.BeginInvoke(new updateDelegate(updatePanel), DispatcherPriority.Normal, parameters);
            }

            _timer.Change(0, 150);
        }

        private void _tick(object state) {
            bool action = false;
            for (int i = 0; i < _charsCurrent.Length; i++) {
                if (_charsCurrent[i] != _charsFinal[i]) {
                    // Wenn das Zeichen außerhalb des animierten Bereichs liegt, sofort hinspringen
                    if (_charsFinal[i] < StartChar || _charsFinal[i] > EndChar) 
                        _charsCurrent[i] = _charsFinal[i];
                    else
                        _charsCurrent[i] = StrLib.AsciiInc(_charsCurrent[i], StartChar, EndChar);
                    object[] parameters = new object[] { i, _charsCurrent[i] };
                    Panels[i].Dispatcher.BeginInvoke(new updateDelegate(updatePanel), DispatcherPriority.Normal, parameters);
                    action = true;
                }
            }
            if (!action) _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private delegate void updateDelegate(int panel, char character);
        private void updatePanel(int panel, char character) {
            Panels[panel].Content = character.ToString();
        }

    }

}
