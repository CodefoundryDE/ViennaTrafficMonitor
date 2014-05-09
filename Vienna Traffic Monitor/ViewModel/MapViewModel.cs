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

            _drawHaltestellen();
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
            Dictionary<ILinie, List<IHaltestelle>> dict = _LinienMapper.GetHaltestellenOrdered();
            foreach (KeyValuePair<ILinie, List<IHaltestelle>> kvp in dict) {
                MapPolyline polyline = new MapPolyline();
                polyline.Stroke = new SolidColorBrush(_getColorByLine(kvp.Key.Bezeichnung));
                polyline.StrokeThickness = 5;
                polyline.StrokeLineJoin = PenLineJoin.Round;
                polyline.StrokeEndLineCap = PenLineCap.Round;
                polyline.StrokeMiterLimit = 0.5;
                LocationCollection locations = new LocationCollection();
                foreach (IHaltestelle haltestelle in kvp.Value) {
                    Location location = new Location(haltestelle.Location.X, haltestelle.Location.Y);
                    locations.Add(location);
                }
                polyline.Locations = locations;
                MapControl.Children.Add(polyline);
            }
        }

        private Color _getColorByLine(string line) {
            switch (line) {
                case "U1": return Colors.Red;
                case "U2": return Colors.Purple;
                case "U3": return Colors.Orange;
                case "U4": return Colors.Green;
                case "U6": return Colors.Brown;
                default: return Colors.Black;
            }
        }

    }

}
