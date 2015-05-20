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

        public List<ContentControl> Panels { get; private set; }

        private char[] _charsCurrent;
        private char[] _charsFinal;

        public SplitFlapDisplay()
        {
            InitializeComponent();
            Panels = new List<ContentControl>();
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
                ContentControl panel = new ModernPanel();
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

            for (int i = 0; i < _charsCurrent.Length; i++)
            {
                _charsCurrent[i] = _charsFinal[i];
                _display(i, _charsCurrent[i]);
            }
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
