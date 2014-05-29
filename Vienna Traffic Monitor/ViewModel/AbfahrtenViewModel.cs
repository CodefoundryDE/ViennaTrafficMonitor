using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtmFramework.ViewModel;
using ViennaTrafficMonitor.Deserializer;
using ViennaTrafficMonitor.Model;
using ViennaTrafficMonitor.Mapper;
using ViennaTrafficMonitor.Filter;
using System.Threading;
using VtmFramework.Command;
using System.Windows.Input;
using System.Windows;
using System.Net.Http;

namespace ViennaTrafficMonitor.ViewModel {

    public class AbfahrtenViewModel : AbstractViewModel {

        #region Properties/Variablen

        private FilterCollection<VtmResponse> _verkehrsmittelFilter;

        private ISet<EVerkehrsmittel> _verkehrsmittel;

        private ICollection<VtmResponse> _Response;
        private ICollection<VtmResponse> Response {
            set {
                _Response = value;
                RaisePropertyChangedEvent("Abfahrten");
            }
        }

        private Timer _Timer;

        private int _resultCount = 6;

        public int Intervall { get; set; }

        private Timer _TimerCurrentTime;
        public string CurrentTime { get { return DateTime.Now.ToString("HH:mm:ss"); } }

        public ICollection<VtmResponse> Abfahrten {
            get {
                ICollection<VtmResponse> response =  _verkehrsmittelFilter.Filter(_Response);
                if (response.Count == _resultCount) {
                    return response;
                } else {
                   while (response.Count < _resultCount) {
                        response.Add(new VtmResponse(
                            new Line(),
                            new Departure(new DepartureTime()),
                            new LocationStop(),
                            new List<TrafficInfoCategory>(),
                            new List<TrafficInfoCategoryGroup>(),
                            "noType"));
                    }
                    return response;
                }
            }
        }

        private IHaltestelle _Haltestelle;
        public IHaltestelle Haltestelle {
            get { return _Haltestelle; }
            private set {
                _Haltestelle = value;
                RaisePropertyChangedEvent("Haltestelle");
            }
        }

        private ISet<int> _Rbls;

        #endregion

        public AbfahrtenViewModel(IHaltestelle haltestelle)
            : base() {
            Haltestelle = haltestelle;
            _InitializeRbls();
            _InitializeVerkehrsmittel();
            _InitializeFilters();
            _StartRequestIntervall();
        }

        #region Initialisierung
        private void _StartRequestIntervall() {
            Intervall = 10000;
            _Timer = new Timer(_GetResponse, null, 0, Intervall);
            _TimerCurrentTime = new Timer((object state) => {
                RaisePropertyChangedEvent("CurrentTime");
            }, null, 0, 1000);
        }

        private void _InitializeRbls() {
            ISteigMapper sm = SteigMapperFactory.Instance;
            List<ISteig> steige = sm.FindByHaltestelle(_Haltestelle.Id);
            _Rbls = new HashSet<int>(from steig in steige
                                     where steig.Rbl > 0
                                     select steig.Rbl);
        }

        private async void _GetResponse(object state) {
            bool error = false;
            try {
                Response = await RblRequesterProxy.GetProxyResponseAsync(_Rbls);
            } catch (HttpRequestException e) {
                // Im catch-Block ist kein await erlaubt - das kommt erst mit C# 6.0
                error = true;
            } catch (Exception e) {
                error = false;
            }
            if (error) await RaiseError("Fehler", "Es konnte keine Anfrage an die API der Wiener Linien gestellt werden.", VtmFramework.Error.EErrorButtons.RetryCancel);
        }

        private void _InitializeVerkehrsmittel() {
            ILinienMapper lm = LinienMapperFactory.Instance;
            ISet<ILinie> linien = lm.FindByHaltestelle(Haltestelle.Id);
            _verkehrsmittel = new HashSet<EVerkehrsmittel>();
            foreach (ILinie linie in linien) {
                _verkehrsmittel.Add(linie.Verkehrsmittel);
            }
        }

        private void _InitializeFilters() {
            _verkehrsmittelFilter = new FilterCollection<VtmResponse>();
            _verkehrsmittelFilter.Add("MetroFilter", new AbfahrtenFilter(EVerkehrsmittel.Metro, false));
            _verkehrsmittelFilter.Add("CityBusFilter", new AbfahrtenFilter(EVerkehrsmittel.CityBus, false));
            _verkehrsmittelFilter.Add("NachtBusFilter", new AbfahrtenFilter(EVerkehrsmittel.NachtBus, false));
            _verkehrsmittelFilter.Add("SbahnFilter", new AbfahrtenFilter(EVerkehrsmittel.SBahn, false));
            _verkehrsmittelFilter.Add("TramFilter", new AbfahrtenFilter(EVerkehrsmittel.Tram, false));
            _verkehrsmittelFilter.Add("TramWlbFilter", new AbfahrtenFilter(EVerkehrsmittel.TramWlb, false));
            _verkehrsmittelFilter.Add("OrderByAbfahrt", new OrderAbfahrtenFilter(EAbfahrtenOrder.TimePlannedAsc, _resultCount, true));
        }
        #endregion

        #region ButtonUbahn

        public ICommand ButtonUBahnCommand {
            get { return new AwaitableDelegateCommand(_ubahn); }
        }

        private async Task _ubahn() {
            _switchFilterActive("MetroFilter");
            RaisePropertyChangedEvent("ButtonUBahnOpacity");
        }
        public double ButtonUBahnOpacity {
            get { return _getOpacity("MetroFilter");
            }
        }
        public Visibility ButtonUBahnVisibility {
            get { return _verkehrsmittel.Contains(EVerkehrsmittel.Metro) ? Visibility.Visible : Visibility.Collapsed; }
        }

        #endregion

        #region ButtonTram

        public ICommand ButtonTramCommand {
            get { return new AwaitableDelegateCommand(_tram); }
        }

        private async Task _tram() {
            _switchFilterActive("TramFilter");
            RaisePropertyChangedEvent("ButtonTramOpacity");
        }
        public double ButtonTramOpacity {
            get { return _getOpacity("TramFilter"); }
        }
        public Visibility ButtonTramVisibility {
            get { return _verkehrsmittel.Contains(EVerkehrsmittel.Tram) ? Visibility.Visible : Visibility.Collapsed; }
        }

        #endregion

        #region ButtonTramWLB

        public ICommand ButtonTramWlbCommand {
            get { return new AwaitableDelegateCommand(_tramWlb); }
        }

        private async Task _tramWlb() {
            _switchFilterActive("TramWlbFilter");
            RaisePropertyChangedEvent("ButtonTramWlbOpacity");
        }
        public double ButtonTramWlbOpacity {
            get { return _getOpacity("TramWlbFilter"); }
        }
        public Visibility ButtonTramWlbVisibility {
            get { return _verkehrsmittel.Contains(EVerkehrsmittel.TramWlb) ? Visibility.Visible : Visibility.Collapsed; }
        }

        #endregion

        #region ButtonCityBus

        public ICommand ButtonCityBusCommand {
            get { return new AwaitableDelegateCommand(_cityBus); }
        }

        private async Task _cityBus() {
            _switchFilterActive("CityBusFilter");
            RaisePropertyChangedEvent("ButtonCityBusOpacity");
        }
        public double ButtonCityBusOpacity {
            get { return _getOpacity("CityBusFilter"); }
        }
        public Visibility ButtonCityBusVisibility {
            get { return _verkehrsmittel.Contains(EVerkehrsmittel.CityBus) ? Visibility.Visible : Visibility.Collapsed; }
        }

        #endregion

        #region ButtonNachtBus

        public ICommand ButtonNachtBusCommand {
            get { return new AwaitableDelegateCommand(_nachtBus); }
        }

        private async Task _nachtBus() {
            _switchFilterActive("NachtBusFilter");
            RaisePropertyChangedEvent("ButtonNachtBusOpacity");
        }
        public double ButtonNachtBusOpacity {
            get { return _getOpacity("NachtBusFilter"); }
        }
        public Visibility ButtonNachtBusVisibility {
            get { return _verkehrsmittel.Contains(EVerkehrsmittel.NachtBus) ? Visibility.Visible : Visibility.Collapsed; }
        }

        #endregion

        private void _switchFilterActive(string filterName) {
            IFilter<VtmResponse> filter;
            _verkehrsmittelFilter.TryGetValue(filterName, out filter);
            filter.Active = filter.Active ? false : true;
            RaisePropertyChangedEvent("Abfahrten");
        }

        private double _getOpacity(string filterName) {
            IFilter<VtmResponse> filter;
            _verkehrsmittelFilter.TryGetValue(filterName, out filter);
            return filter.ButtonOpacity;
        }
    }
}