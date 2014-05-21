using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VtmFramework.ViewModel;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Maps.MapControl.WPF.Core;
using System.Windows.Input;
using ViennaTrafficMonitor.Mapper;
using ViennaTrafficMonitor.Model;
using System.Windows.Media;
using VtmFramework.Command;
using ViennaTrafficMonitor.Filter;
using ViennaTrafficMonitor.Filter.MapFilter;
using System;
using System.Collections.ObjectModel;

namespace ViennaTrafficMonitor.ViewModel {

    public class MapViewModel : AbstractViewModel {

        private ILinienMapper _LinienMapper;

        private IDictionary<ILinie, List<IHaltestelle>> _linien;

        private FilterCollection<KeyValuePair<ILinie, List<IHaltestelle>>> _filterCollection;

        private CredentialsProvider _credentialsProvider;
        public CredentialsProvider CredentialsProvider {
            get { return _credentialsProvider; }
            private set {
                _credentialsProvider = value;
                RaisePropertyChangedEvent("CredentialsProvider");
            }
        }

        private Location _center;
        public Location Center {
            get { return _center; }
            set {
                _center = value;
                RaisePropertyChangedEvent("Center");
            }
        }

        private double _zoomLevel;
        public double ZoomLevel {
            get { return _zoomLevel; }
            set {
                _zoomLevel = value;
                RaisePropertyChangedEvent("ZoomLevel");
            }
        }

        private ICollection<Location> _pushpins;
        public ICollection<Location> Pushpins {
            get { return _pushpins; }
            private set {
                _pushpins = value;
                RaisePropertyChangedEvent("Pushpins");
            }
        }

        private ICollection<MapPolyline> _polylines;
        public ICollection<MapPolyline> PolyLines {
            get { return _polylines; }
            private set {
                _polylines = value;
                RaisePropertyChangedEvent("Polylines");
            }
        }

        public MapViewModel(CredentialsProvider credentialsProvider, ILinienMapper linienMapper) {
            if (credentialsProvider == null)
                throw new ArgumentNullException("credentialsProvider");
            if (linienMapper == null)
                throw new ArgumentNullException("linienMapper");

            CredentialsProvider = credentialsProvider;
            _LinienMapper = linienMapper;

            Pushpins = new ObservableCollection<Location>();
            PolyLines = new ObservableCollection<MapPolyline>();



            _linien = _LinienMapper.HaltestellenOrdered;

            _initFilters();

            // Startpunkt: Wien Stephansdom
            Center = new Location(48.208333, 16.372778);
            ZoomLevel = 13.0;

            // TODO
            //MapControl.ViewChangeEnd += _mapViewChangeEnd;

            _drawLinien();
        }

        private void _initFilters() {
            _filterCollection = new FilterCollection<KeyValuePair<ILinie, List<IHaltestelle>>>();

            _filterCollection.Add("Metro", new MapFilter(EVerkehrsmittel.Metro));
            _filterCollection.Add("SBahn", new MapFilter(EVerkehrsmittel.SBahn));
            _filterCollection.Add("Tram", new MapFilter(EVerkehrsmittel.Tram));
            _filterCollection.Add("TramWlb", new MapFilter(EVerkehrsmittel.TramWlb));
            _filterCollection.Add("CityBus", new MapFilter(EVerkehrsmittel.CityBus));
            _filterCollection.Add("NachtBus", new MapFilter(EVerkehrsmittel.NachtBus));
        }

        private void _mapViewChangeEnd(object sender, MapEventArgs e) {
            if (ZoomLevel < 15) {
                //MapControl.ZoomLevel = 15;
            }
        }

        private void _drawHaltestellen(IEnumerable<IHaltestelle> haltestellen) {
            Pushpins.Clear();
            foreach (IHaltestelle haltestelle in haltestellen) {
                Pushpin pin = new Pushpin();
                Location location = new Location(haltestelle.Location.X, haltestelle.Location.Y);
                pin.Tag = haltestelle.Id;
                pin.Location = location;
                pin.CommandBindings.Add(new CommandBinding());
                Pushpins.Add(location);
            }
            RaisePropertyChangedEvent("Pushpins");
        }

        private void _drawLinien() {
            PolyLines.Clear();
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
                PolyLines.Add(polyline);
            }

            var query = from kvp in dict select kvp.Value;
            IEnumerable<IHaltestelle> haltestellen = new List<IHaltestelle>();
            foreach (List<IHaltestelle> list in query) {
                haltestellen = haltestellen.Union<IHaltestelle>(list);
            }
            _drawHaltestellen(haltestellen.Distinct());
            RaisePropertyChangedEvent("Polylines");
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

        #region ButtonMetro
        public ICommand ButtonMetroCommand {
            get { return new AwaitableDelegateCommand(_metro); }
        }
        private async Task _metro() {
            _filterButton("Metro");
        }

        public bool ButtonMetroActive {
            get { return !_filterCollection["Metro"].Active; }
        }
        #endregion

        #region ButtonSBahn
        public ICommand ButtonSBahnCommand {
            get { return new AwaitableDelegateCommand(_sbahn); }
        }
        private async Task _sbahn() {
            _filterButton("SBahn");
        }

        public bool ButtonSBahnActive {
            get { return !_filterCollection["SBahn"].Active; }
        }
        #endregion

        #region ButtonTram
        public ICommand ButtonTramCommand {
            get { return new AwaitableDelegateCommand(_tram); }
        }
        private async Task _tram() {
            _filterButton("Tram");
        }

        public bool ButtonTramActive {
            get { return !_filterCollection["Tram"].Active; }
        }
        #endregion

        #region ButtonTramWlb
        public ICommand ButtonTramWlbCommand {
            get { return new AwaitableDelegateCommand(_tramwlb); }
        }
        private async Task _tramwlb() {
            _filterButton("TramWlb");
        }

        public bool ButtonTramWlbActive {
            get { return !_filterCollection["TramWlb"].Active; }
        }
        #endregion

        #region ButtonCityBus
        public ICommand ButtonCityBusCommand {
            get { return new AwaitableDelegateCommand(_citybus); }
        }
        private async Task _citybus() {
            _filterButton("CityBus");
        }

        public bool ButtonCityBusActive {
            get { return !_filterCollection["CityBus"].Active; }
        }
        #endregion

        #region ButtonNachtBus
        public ICommand ButtonNachtBusCommand {
            get { return new AwaitableDelegateCommand(_nachtbus); }
        }
        private async Task _nachtbus() {
            _filterButton("NachtBus");
        }

        public bool ButtonNachtBusActive {
            get { return !_filterCollection["NachtBus"].Active; }
        }
        #endregion

        private void _filterButton(string name) {
            _filterCollection[name].Active = _filterCollection[name].Active ? false : true;
            RaisePropertyChangedEvent("Button" + name + "Active");
            _drawLinien();
        }
    }

}
