using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using VtmFramework.Library;

namespace VtmFramework.View
{

    /// <summary>
    /// Interaktionslogik für SplitFlapDisplay.xaml
    /// </summary>
    public partial class SplitFlapDisplay : UserControl
    {

        #region DependencyProperty PanelCount
        public static readonly DependencyProperty PanelCountProperty = DependencyProperty.Register("PanelCount", typeof(int), typeof(SplitFlapDisplay), new FrameworkPropertyMetadata(0, OnPanelCountChanged));

        public int PanelCount
        {
            get { return (int)this.GetValue(PanelCountProperty); }
            set
            {
                this.SetValue(PanelCountProperty, value);
            }
        }

        private static void OnPanelCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SplitFlapDisplay)d).OnPanelCountChanged();
        }
        #endregion

        #region DependencyProperty Text
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(SplitFlapDisplay), new FrameworkPropertyMetadata(OnTextChanged));

        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SplitFlapDisplay display = (SplitFlapDisplay)d;
            display.OnTextChanged();
        }
        #endregion

        #region DependencyProperty Animated
        public static readonly DependencyProperty AnimatedProperty = DependencyProperty.Register("Animated", typeof(bool), typeof(SplitFlapDisplay), new FrameworkPropertyMetadata(true));

        public bool Animated
        {
            get
            { // Dieser Getter ist Threadsafe.
                try
                {
                    return (bool)this.Dispatcher.Invoke(
                        DispatcherPriority.Normal,
                        (DispatcherOperationCallback)delegate { return GetValue(AnimatedProperty); },
                        AnimatedProperty);
                }
                catch
                {
                    return (bool)AnimatedProperty.DefaultMetadata.DefaultValue;
                }
            }
            set { this.SetValue(AnimatedProperty, value); }
        }
        #endregion

        #region StartChar
        public static readonly DependencyProperty StartCharProperty = DependencyProperty.Register("StartChar", typeof(char), typeof(SplitFlapDisplay), new FrameworkPropertyMetadata('0'));

        public char StartChar
        {
            get
            { // Dieser Getter ist Threadsafe.
                try
                {
                    return (char)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return this.GetValue(StartCharProperty); },
                       StartCharProperty);
                }
                catch
                {
                    return (char)StartCharProperty.DefaultMetadata.DefaultValue;
                }
            }
            set { this.SetValue(StartCharProperty, value); }
        }
        #endregion

        #region EndChar
        public static readonly DependencyProperty EndCharProperty = DependencyProperty.Register("EndChar", typeof(char), typeof(SplitFlapDisplay), new FrameworkPropertyMetadata('Z'));

        public char EndChar
        {
            get
            { // Dieser Getter ist Threadsafe.
                try
                {
                    return (char)this.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Normal,
                       (DispatcherOperationCallback)delegate { return GetValue(EndCharProperty); },
                       EndCharProperty);
                }
                catch
                {
                    return (char)EndCharProperty.DefaultMetadata.DefaultValue;
                }
            }
            set { this.SetValue(EndCharProperty, value); }
        }
        #endregion

        private readonly Timer _timer;

        public List<SplitFlapPanel> Panels { get; private set; }

        private char[] _charsCurrent;
        private char[] _charsFinal;

        public SplitFlapDisplay()
        {
            InitializeComponent();
            Panels = new List<SplitFlapPanel>();
            _timer = new Timer(_tick);
        }

        /// <summary>
        /// Wird ausgeführt wenn sich die Anzahl der Panels ändert.
        /// Erstellt eine Anzahl an Panels und legt sie auf das StackPanel.
        /// </summary>
        private void OnPanelCountChanged()
        {
            Panels.Clear();
            for (int i = 0; i < PanelCount; i++)
            {
                var panel = new SplitFlapPanel
                {
                    Content = ' ',
                    Width = 40,
                    BorderThickness = new Thickness(1),
                    BorderBrush = new SolidColorBrush(Colors.Black)
                };
                Panels.Add(panel);
                MainPanel.Children.Add(panel);
            }

            // Das Char-Array mit Leerzeichen füllen
            _charsCurrent = (new String(' ', PanelCount)).ToCharArray(0, PanelCount);
            for (int i = 0; i < PanelCount; i++)
            {
                _display(i, _charsCurrent[i]);
            }

            MainPanel.UpdateLayout();
        }

        /// <summary>
        /// Wird aufgerufen wenn sich der Text ändert.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Mobility", "CA1601:DoNotUseTimersThatPreventPowerStateChanges")]
        private void OnTextChanged()
        {
            string text = String.IsNullOrWhiteSpace(Text) ? string.Empty : Text;
            text = StrLib.UmlautFilter(text).ToUpper().PadRight(PanelCount, ' ');

            _charsFinal = text.ToCharArray(0, PanelCount);

            _timer.Change(0, 150);
        }

        private void _tick(object state)
        {
            bool action = false;
            for (int i = 0; i < _charsCurrent.Length; i++)
            {
                if (_charsCurrent[i] == _charsFinal[i]) continue;
                // Wenn das Zeichen außerhalb des animierten Bereichs liegt, sofort hinspringen
                // Oder, wenn das Panel nicht das Alphabet durchspringen soll
                if (_charsFinal[i] < StartChar || _charsFinal[i] > EndChar || !Animated)
                    _charsCurrent[i] = _charsFinal[i];
                else
                    _charsCurrent[i] = StrLib.AsciiInc(_charsCurrent[i], StartChar, EndChar);
                _display(i, _charsCurrent[i]);
                action = true;
            }
            if (!action) _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void _display(int panel, char character)
        {
            Panels[panel].Dispatcher.BeginInvoke(new UpdateDelegate(UpdatePanel), DispatcherPriority.Normal, panel, character);
        }

        private delegate void UpdateDelegate(int panel, char character);
        private void UpdatePanel(int panel, char character)
        {
            Panels[panel].Content = character.ToString();
        }

    }

}
