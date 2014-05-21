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

namespace ViennaTrafficMonitor.View {
    /// <summary>
    /// Interaktionslogik für AbfahrtControl.xaml
    /// </summary>
    public partial class AbfahrtControl : UserControl {

        #region DependencyProperties
        public static readonly DependencyProperty AbfahrtProperty = DependencyProperty.Register("Abfahrt", typeof(string), typeof(AbfahrtControl), new FrameworkPropertyMetadata("", OnAbfahrtPropertyChanged));
        public string Abfahrt {
            get { return base.GetValue(AbfahrtProperty) as string; }
            set {
                base.SetValue(AbfahrtProperty, value);
            }
        }
        private static void OnAbfahrtPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e) {
            AbfahrtControl control = source as AbfahrtControl;
            control.OnAbfahrtPropertyChanged();
        }
        private void OnAbfahrtPropertyChanged() {
            LAbfahrt.Content = Abfahrt;
        }

        public static readonly DependencyProperty LineNameProperty = DependencyProperty.Register("LineName", typeof(string), typeof(AbfahrtControl), new FrameworkPropertyMetadata("", OnLinePropertyChanged));
        public string LineName {
            get { return base.GetValue(LineNameProperty) as string; }
            set { base.SetValue(LineNameProperty, value); }
        }

        private static void OnLinePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e) {
            AbfahrtControl control = source as AbfahrtControl;
            control.OnLinePropertyChanged();
        }
        private void OnLinePropertyChanged() {
            LLinie.Content = LineName;
        }

        public static readonly DependencyProperty TowardsProperty = DependencyProperty.Register("Towards", typeof(string), typeof(AbfahrtControl), new FrameworkPropertyMetadata("", OnTowardsPropertyChanged));
        public string Towards {
            get { return base.GetValue(TowardsProperty) as string; }
            set { base.SetValue(TowardsProperty, value); }
        }
        private static void OnTowardsPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e) {
            AbfahrtControl control = source as AbfahrtControl;
            control.OnTowardsPropertyChanged();
        }
        private void OnTowardsPropertyChanged() {
            SFDRichtung.Text = Towards;
        }

        public static readonly DependencyProperty GleisProperty = DependencyProperty.Register("Gleis", typeof(string), typeof(AbfahrtControl), new FrameworkPropertyMetadata("", OnGleisPropertyChanged));
        public string Gleis {
            get { return base.GetValue(GleisProperty) as string; }
            set { base.SetValue(GleisProperty, value); }
        }

        private static void OnGleisPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e) {
            AbfahrtControl control = source as AbfahrtControl;
            control.OnGleisPropertyChanged();
        }
        private void OnGleisPropertyChanged() {
            LGleis.Content = Gleis;
        }

        public static readonly DependencyProperty VerspaetungProperty = DependencyProperty.Register("Verspaetung", typeof(string), typeof(AbfahrtControl), new FrameworkPropertyMetadata("", OnVerspaetungPropertyChanged));
        public string Verspaetung {
            get { return base.GetValue(VerspaetungProperty) as string; }
            set { base.SetValue(VerspaetungProperty, value); }
        }
        private static void OnVerspaetungPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e) {
            AbfahrtControl control = source as AbfahrtControl;
            control.OnVerspaetungPropertyChanged();
        }
        private void OnVerspaetungPropertyChanged() {
            LVerspaetung.Content = Verspaetung;
        }
        #endregion

        public AbfahrtControl() {
            InitializeComponent();
        }
    }
}
