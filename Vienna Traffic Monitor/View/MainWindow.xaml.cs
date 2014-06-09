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
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViennaTrafficMonitor.ViewModel;

namespace ViennaTrafficMonitor.View {
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(OnGuiLoaded);

        }

        private void OnGuiLoaded(object sender, RoutedEventArgs e) {
            ((MainViewModel)this.DataContext).initializeApp();
        }

    }
}
