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
using VtmFramework.Command;
using ViennaTrafficMonitor.Filter;

namespace ViennaTrafficMonitor.ViewModel {

    public class MapViewModel : AbstractViewModel {

        private ILinienMapper _LinienMapper;

        private IDictionary<ILinie, List<IHaltestelle>> _linien;

        private FilterCollection<KeyValuePair<ILinie, List<IHaltestelle>>> _linienFilter;

        private IFilter<KeyValuePair<ILinie, List<IHaltestelle>>> _ubahnFilter;

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
            _linienFilter = new FilterCollection<KeyValuePair<ILinie, List<IHaltestelle>>>();

            _ubahnFilter = new GenericFilter<KeyValuePair<ILinie, List<IHaltestelle>>>(
                (ICollection<KeyValuePair<ILinie, List<IHaltestelle>>> collection) => {
                    var query = from linie in collection
                                where linie.Key.Verkehrsmittel != EVerkehrsmittel.Metro
                                select linie;
                    return query.ToDictionary(x => x.Key, x => x.Value);
                });
            _ubahnFilter.Active = false;
            _linienFilter.Add(_ubahnFilter);

            MapControl = new Map();
            // Startpunkt: Wien Stephansdom
            MapControl.Center = new Location(48.208333, 16.372778);
            MapControl.ZoomLevel = 13.0;

            _drawLinien();
        }

        private void _drawHaltestelle(IHaltestelle haltestelle) {
            Pushpin pin = new Pushpin();
            Location location = new Location(haltestelle.Location.X, haltestelle.Location.Y);
            pin.Tag = haltestelle.Id;
            pin.Location = location;
            MapControl.Children.Add(pin);
        }

        private void _drawLinien() {
            MapControl.Children.Clear();
            ICollection<KeyValuePair<ILinie, List<IHaltestelle>>> dict = _linienFilter.Filter(_linien);
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
                    _drawHaltestelle(haltestelle);
                }
                polyline.Locations = locations;
                MapControl.Children.Add(polyline);
            }
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
    }

}
