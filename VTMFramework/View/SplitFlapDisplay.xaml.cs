using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(SplitFlapDisplay), new FrameworkPropertyMetadata(OnTextChanged, OnTextUpdated));

        public string Text {
            get { return (string)this.GetValue(TextProperty); }
            set {
                this.SetValue(TextProperty, value);
            }
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            SplitFlapDisplay display = (SplitFlapDisplay)d;
            display.Text = (string)e.NewValue;
            display.OnTextChanged();
        }

        private static object OnTextUpdated(DependencyObject d, object baseValue) {
            SplitFlapDisplay display = (SplitFlapDisplay)d;
            display.OnTextChanged();
            return baseValue;
        }
        #endregion

        public List<SplitFlapPanel> Panels;

        private char[] _chars;

        public SplitFlapDisplay() {
            InitializeComponent();
            Panels = new List<SplitFlapPanel>();
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
            _chars = text.ToCharArray(0, PanelCount);
            for (int i = 0; i < PanelCount; i++) {
                Panels[i].Content = _chars[i].ToString();
            }
        }

    }

}
