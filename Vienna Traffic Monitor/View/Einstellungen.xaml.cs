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
    /// Interaktionslogik für Einstellungen.xaml
    /// </summary>
    public partial class Einstellungen : UserControl {
        public Einstellungen() {
            InitializeComponent();
            DockPanel.Height = SystemParameters.FullPrimaryScreenHeight / 8;
            CheckboxEinstellungen.IsChecked = true;
            CheckboxEinstellungen.IsChecked = false;
        }
    }
}
