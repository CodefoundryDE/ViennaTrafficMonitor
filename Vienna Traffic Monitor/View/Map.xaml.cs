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
using Microsoft.Maps.MapControl.WPF;

namespace ViennaTrafficMonitor.View {
    /// <summary>
    /// Interaktionslogik für Map.xaml
    /// </summary>
    public partial class Map : UserControl {

        #region Dependency Properties
        public static readonly DependencyProperty PushpinsProperty = DependencyProperty.Register("Pushpins", typeof(IEnumerable<Pushpin>), typeof(Map), new FrameworkPropertyMetadata(null, OnPushpinsChanged));
        public IEnumerable<Pushpin> Pushpins {
            get { return (IEnumerable<Pushpin>)this.GetValue(PushpinsProperty); }
            set { this.SetValue(PushpinsProperty, value); }
        }
        private static void OnPushpinsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            Map map = (Map)d;
            map.OnChildrenChanged();
        }
        #endregion

        public Map() {
            InitializeComponent();
        }

        private void OnChildrenChanged() {

        }

    }
}
