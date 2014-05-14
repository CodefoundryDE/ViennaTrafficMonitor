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
    /// Interaktionslogik für Suche.xaml
    /// </summary>
    public partial class Suche : UserControl {
        public Suche() {
            InitializeComponent();
        }

        private void SearchField_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Down && ListBox.Items.Count > 0) {
                ListBox.SelectedIndex = 0;
                ((ListBoxItem) ListBox.ItemContainerGenerator.ContainerFromItem(ListBox.SelectedItem)).Focus();
                e.Handled = true;
            }
        }

        private void ListBox_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Up && ListBox.SelectedIndex == 0) {
                SearchField.Focus();
                e.Handled = true;
            }
        }

    }
}
