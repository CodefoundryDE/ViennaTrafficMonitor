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
using VtmFramework.ViewModel;

namespace VtmFramework.View {
    /// <summary>
    /// Interaktionslogik für ErrorView.xaml
    /// </summary>
    public partial class ErrorView : UserControl {
        public ErrorView() {
            InitializeComponent();
            Grid.Height = SystemParameters.FullPrimaryScreenHeight / 4;
            if (((ErrorViewModel)DataContext).Visible == false) {
                ((ErrorViewModel)DataContext).Visible = true;
                ((ErrorViewModel)DataContext).Visible = false;
            }
        }
    }
}
