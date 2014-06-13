using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
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
            _setMonitor();
            ((MainViewModel)this.DataContext).initializeApp();
        }

        private void _setMonitor() {
            Screen screen;
            try {
                screen = Screen.AllScreens[Properties.Settings.Default.Monitor];
            } catch (IndexOutOfRangeException) {
                // Ausweichen auf den Hauptbildschirm
                screen = Screen.PrimaryScreen;
                Properties.Settings.Default.Monitor = 0;
                Properties.Settings.Default.Save();
            }
            this.WindowState = WindowState.Normal;
            this.Top = screen.WorkingArea.Top;
            this.Left = screen.WorkingArea.Left;
            this.WindowState = WindowState.Maximized;
            this.WindowStyle = WindowStyle.None;
        }

    }
}
