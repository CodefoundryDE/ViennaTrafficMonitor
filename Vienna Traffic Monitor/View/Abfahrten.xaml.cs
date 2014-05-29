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
using VtmFramework.Converter;

namespace ViennaTrafficMonitor.View {

    /// <summary>
    /// Interaktionslogik für Abfahrten.xaml
    /// </summary>
    public partial class Abfahrten : UserControl {

        private const int CONTROLCOUNT = 6;

        private UIElementCollection _abfahrtControls;

        public Abfahrten() {
            InitializeComponent();

            _abfahrtControls = new UIElementCollection(this, this);
            _createAbfahrtControls();
        }

        private void _createAbfahrtControls() {
            for (int i = 0; i < CONTROLCOUNT; i++) {
                AbfahrtControl control = new AbfahrtControl();

                string index = "Abfahrten[" + i.ToString() + "]";

                // Abfahrtszeit
                control.SetBinding(AbfahrtControl.AbfahrtProperty, new Binding(index + ".Departure.DepartureTime.TimePlanned") {
                    Converter = new StringToTimeConverter()
                });
                // Linie
                control.SetBinding(AbfahrtControl.LineNameProperty, new Binding(index + ".Line.Name"));
                // Richtung
                control.SetBinding(AbfahrtControl.TowardsProperty, new Binding(index + ".Line.Towards"));
                // Gleis
                control.SetBinding(AbfahrtControl.GleisProperty, new Binding(index + ".Line.Platform"));
                // Verspätung
                control.SetBinding(AbfahrtControl.VerspaetungProperty, new Binding(index + ".Departure.DepartureTime.Countdown"));

                AbfahrtPanel.Children.Add(control);                
            }
        }

    }

}
