using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VtmFramework.ViewModel;
using Microsoft.Maps.MapControl.WPF;
using System.Windows.Input;
using ViennaTrafficMonitor.Mapper;
using ViennaTrafficMonitor.Model;
using System.Windows.Media;
using VtmFramework.Command;
using ViennaTrafficMonitor.Filter;
using ViennaTrafficMonitor.Filter.MapFilter;

namespace ViennaTrafficMonitor.ViewModel {

    public class MapViewModel : AbstractViewModel {

        private ILinienMapper _LinienMapper;

        private IDictionary<ILinie, List<IHaltestelle>> _linien;

        private FilterCollection<KeyValuePair<ILinie, List<IHaltestelle>>> _filterCollection;

        private IFilter<KeyValuePair<ILinie, List<IHaltestelle>>> _ubahnFilter;
        private IFilter<KeyValuePair<ILinie, List<IHaltestelle>>> _sbahnFilter;

        private Map _map;
        public Map MapControl {
            get { return _map; }
            set {
                _map = value;
                RaisePropertyChangedEvent("MapControl");
            }
        }

        public MapViewModel(ILinienMapper linienMapper) {
            _LinienMapper = linienMapper;
            _linien = _LinienMapper.HaltestellenOrdered;
            _filterCollection = new FilterCollection<KeyValuePair<ILinie, List<IHaltestelle>>>();

            _ubahnFilter = new MapFilter(EVerkehrsmittel.Metro, false);
            _filterCollection.Add(_ubahnFilter);
            _sbahnFilter = new MapFilter(EVerkehrsmittel.SBahn, false);
            _filterCollection.Add(_sbahnFilter);

            MapControl = new Map();
            // Startpunkt: Wien Stephansdom
            MapControl.Center = new Location(48.208333, 16.372778);
            MapControl.ZoomLevel = 13.0;

            MapControl.ViewChangeEnd += _mapViewChangeEnd;

            _drawLinien();
        }

        private void _mapViewChangeEnd(object sender, MapEventArgs e) {
            if (MapControl.ZoomLevel < 15) {
                MapControl.ZoomLevel = 15;
            }
            RaisePropertyChangedEvent("MapControl");
        }

        private void _drawHaltestellen(IEnumerable<IHaltestelle> haltestellen) {
            foreach (IHaltestelle haltestelle in haltestellen) {
                Pushpin pin = new Pushpin();
                Location location = new Location(haltestelle.Location.X, haltestelle.Location.Y);
                pin.Tag = haltestelle.Id;
                pin.Location = location;
                MapControl.Children.Add(pin);
            }
        }

        private void _drawLinien() {
            MapControl.Children.Clear();
            ICollection<KeyValuePair<ILinie, List<IHaltestelle>>> dict = _filterCollection.Filter(_linien);
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

            var query = from kvp in dict select kvp.Value;
            IEnumerable<IHaltestelle> haltestellen = new List<IHaltestelle>();
            foreach (List<IHaltestelle> list in query) {
                haltestellen = haltestellen.Union<IHaltestelle>(list);
            }
            _drawHaltestellen(haltestellen.Distinct());
        }

        private static Color _getColorByLine(string line) {
            switch (line) {
                case "U1": return Colors.Red;
                case "U2": return Colors.Purple;
                case "U3": return Colors.Orange;
                case "U4": return Colors.Green;
                case "U6": return Colors.Brown;
                default: return Colors.Black;
            }
        }

        #region ButtonUBahn
        public ICommand ButtonUBahnCommand {
            get { return new AwaitableDelegateCommand(_ubahn); }
        }
        private async Task _ubahn() {
            _ubahnFilter.Active = _ubahnFilter.Active ? false : true;
            RaisePropertyChangedEvent("ButtonUBahnOpacity");
            _drawLinien();
        }

        public double ButtonUBahnOpacity {
            get { return _ubahnFilter.Active ? 0.5 : 0.8; }
        }
        #endregion

        #region ButtonSBahn
        public ICommand ButtonSBahnCommand {
            get { return new AwaitableDelegateCommand(_sbahn); }
        }
        private async Task _sbahn() {
            _sbahnFilter.Active = _sbahnFilter.Active ? false : true;
            RaisePropertyChangedEvent("ButtonSBahnOpacity");
            _drawLinien();
        }

        public double ButtonSBahnOpacity {
            get { return _sbahnFilter.Active ? 0.5 : 0.8; }
        }
        #endregion
    }

}
