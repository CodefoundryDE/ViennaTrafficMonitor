using System.Collections.Generic;
using System.Linq;
using VtmFramework.ViewModel;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Maps.MapControl.WPF.Core;
using ViennaTrafficMonitor.Mapper;
using ViennaTrafficMonitor.Model;
using System.Windows.Media;
using VtmFramework.Command;
using ViennaTrafficMonitor.Filter;
using System;
using System.Collections.ObjectModel;
using ViennaTrafficMonitor.Events;

namespace ViennaTrafficMonitor.ViewModel {

    public class VtmPushpin {
        public IHaltestelle Haltestelle { get; set; }
        public Location Location { get; set; }
    }

    public class MapViewModel : AbstractViewModel {

        public event EventHandler ConnectionLost;

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

        public ICollection<VtmPushpin> Pushpins { get; private set; }
        public ICollection<MapPolyline> PolyLines { get; private set; }

        public MapViewModel(CredentialsProvider credentialsProvider, ILinienMapper linienMapper) {
            if (credentialsProvider == null)
                throw new ArgumentNullException("credentialsProvider");
            if (linienMapper == null)
                throw new ArgumentNullException("linienMapper");

            CredentialsProvider = credentialsProvider;
            _LinienMapper = linienMapper;

            Pushpins = new ObservableCollection<VtmPushpin>();
            PolyLines = new ObservableCollection<MapPolyline>();

            _linien = _LinienMapper.HaltestellenOrdered;

            _initFilters();

            // Startpunkt: Wien Stephansdom
            Center = new Location(48.208333, 16.372778);
            ZoomLevel = 13.0;
        }

        private void _initFilters() {
            _filterCollection = new FilterCollection<KeyValuePair<ILinie, List<IHaltestelle>>>();

            _filterCollection.Add("Metro", new MapFilter(EVerkehrsmittel.Metro));
            _filterCollection.Add("SBahn", new MapFilter(EVerkehrsmittel.SBahn));
            _filterCollection.Add("Tram", new MapFilter(EVerkehrsmittel.Tram));
            _filterCollection.Add("TramVrt", new MapFilter(EVerkehrsmittel.TramVrt));
            _filterCollection.Add("TramWlb", new MapFilter(EVerkehrsmittel.TramWlb));
            _filterCollection.Add("CityBus", new MapFilter(EVerkehrsmittel.CityBus));
            _filterCollection.Add("NachtBus", new MapFilter(EVerkehrsmittel.NachtBus));
        }

        private void _drawHaltestellen(IEnumerable<IHaltestelle> haltestellen) {
            Pushpins.Clear();
            foreach (IHaltestelle haltestelle in haltestellen) {
                Location location = new Location(haltestelle.Location.X, haltestelle.Location.Y);
                Pushpins.Add(new VtmPushpin() { Haltestelle = haltestelle, Location = location });
            }
        }

        private void _drawLinien() {
            PolyLines.Clear();
            ICollection<KeyValuePair<ILinie, List<IHaltestelle>>> dict = _filterCollection.Filter(_linien);
            foreach (KeyValuePair<ILinie, List<IHaltestelle>> kvp in dict) {
                MapPolyline polyline = new MapPolyline();
                polyline.Stroke = new SolidColorBrush(_getColorByLine(kvp.Key.Bezeichnung));
                polyline.StrokeThickness = 5;
                polyline.StrokeLineJoin = PenLineJoin.Round;
                polyline.StrokeStartLineCap = PenLineCap.Round;
                polyline.StrokeEndLineCap = PenLineCap.Round;
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
            foreach (ICollection<IHaltestelle> list in query) {
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

        #region Filter
        public bool ButtonMetroActive {
            get { return !_filterCollection["Metro"].Active; }
            set { _filterButton("Metro", !value); }
        }

        public bool ButtonTramActive {
            get { return !_filterCollection["Tram"].Active; }
            set { _filterButton("Tram", !value); }
        }

        public bool ButtonTramVrtActive {
            get { return !_filterCollection["TramVrt"].Active; }
            set { _filterButton("TramVrt", !value); }
        }

        public bool ButtonTramWlbActive
        {
            get { return !_filterCollection["TramWlb"].Active; }
            set { _filterButton("TramWlb", !value); }
        }

        public bool ButtonCityBusActive {
            get { return !_filterCollection["CityBus"].Active; }
            set { _filterButton("CityBus", !value); }
        }

        public bool ButtonNachtBusActive {
            get { return !_filterCollection["NachtBus"].Active; }
            set { _filterButton("NachtBus", !value); }
        }
        #endregion

        private void _filterButton(string name, bool active) {
            _filterCollection[name].Active = active;
            RaisePropertyChangedEvent("Button" + name + "Active");
            _drawLinien();
        }

        #region Pushpin-Click
        public DelegateCommand PushpinClickCommand {
            get { return new DelegateCommand(_pushpinClick); }
        }

        private void _pushpinClick(object parameter) {
            if (parameter != null) {
                int haltestelle = (int)parameter;
                EventHandler<SucheEventArgs> handler = HaltestelleSelected;
                if (handler != null) {
                    handler(this, new SucheEventArgs(haltestelle));
                }
            }
        }
        public event EventHandler<SucheEventArgs> HaltestelleSelected;
        #endregion

    }

}
