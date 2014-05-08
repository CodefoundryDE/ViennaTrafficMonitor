using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtmFramework.ViewModel;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Maps.MapControl.WPF.Core;
using System.Windows.Input;
using System.Windows;
using ViennaTrafficMonitor.Mapper;
using ViennaTrafficMonitor.Model;
using System.Windows.Media;

namespace ViennaTrafficMonitor.ViewModel {

    public class MapViewModel : AbstractViewModel {

        private IHaltestellenMapper _HaltestellenMapper;
        private ILinienMapper _LinienMapper;
        private Map _map;

        public Map MapControl {
            get { return _map; }
            set {
                _map = value;
                RaisePropertyChangedEvent("MapControl");
            }
        }

        public MapViewModel(IHaltestellenMapper haltestellenMapper, ILinienMapper linienMapper) {
            _HaltestellenMapper = haltestellenMapper;
            _LinienMapper = linienMapper;
            MapControl = new Map();
            // Startpunkt: Wien Stephansdom
            MapControl.Center = new Location(48.208333, 16.372778);
            MapControl.ZoomLevel = 13.0;
            //_drawHaltestellen();
            _drawLinien();
        }

        private void _drawHaltestellen() {
            IDictionary<int, Point> haltestellen = _HaltestellenMapper.GetAllCoordinates();
            foreach (KeyValuePair<int, Point> kvp in haltestellen) {
                Pushpin pin = new Pushpin();
                Location location = new Location(kvp.Value.X, kvp.Value.Y);
                pin.Tag = kvp.Key;
                pin.Location = location;
                MapControl.Children.Add(pin);
            }
        }

        private void _drawLinien() {
            Dictionary<ILinie, List<ISteig>> dict = _LinienMapper.GetSteigeOrdered();
            foreach (KeyValuePair<ILinie, List<ISteig>> kvp in dict) {
                MapPolyline polyline = new MapPolyline();
                polyline.Stroke = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                polyline.StrokeThickness = 5;
                LocationCollection locations = new LocationCollection();
                foreach (ISteig steig in kvp.Value) {
                    Location location = new Location(steig.Location.X, steig.Location.Y);
                    locations.Add(location);
                }
                polyline.Locations = locations;
                MapControl.Children.Add(polyline);
            }
        }
    }

}
