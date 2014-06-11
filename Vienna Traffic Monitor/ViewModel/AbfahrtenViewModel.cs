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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private Timer _Timer;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private Timer _TimerCurrentTime;

        private int _resultCount = 6;

        public int Intervall { get; set; }

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
                            "noType",
                            new List<TrafficInfo>()));
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
            }, null, 0, 500);
        }

        private void _InitializeRbls() {
            ISteigMapper sm = SteigMapperFactory.Instance;
            ICollection<ISteig> steige = sm.FindByHaltestelle(_Haltestelle.Id);
            _Rbls = new HashSet<int>(from steig in steige
                                     where steig.Rbl > 0
                                     select steig.Rbl);
        }

        private async void _GetResponse(object state) {
            bool error = false;
            try {
                Response = await RblRequesterProxy.GetProxyResponseAsync(_Rbls);
            } catch (HttpRequestException) {
                // Im catch-Block ist kein await erlaubt - das kommt erst mit C# 6.0
                error = true;
            } catch (Exception) {
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
            _verkehrsmittelFilter.Add("OrderByAbfahrt", new OrderAbfahrtenFilter(EAbfahrtenOrder.TimeRealAsc, _resultCount, true));
        }
        #endregion

        #region ButtonUbahn
        public bool ButtonMetroActive {
            get { return !_verkehrsmittelFilter["MetroFilter"].Active; }
            set { _verkehrsmittelFilter["MetroFilter"].Active = !value; }
        }

        public bool ButtonMetroVisible {
            get { return _verkehrsmittel.Contains(EVerkehrsmittel.Metro); }
        }
        #endregion

        #region ButtonTram
        public bool ButtonTramActive {
            get { return !_verkehrsmittelFilter["TramFilter"].Active; }
            set { _verkehrsmittelFilter["TramFilter"].Active = !value; }
        }

        public bool ButtonTramVisible {
            get { return _verkehrsmittel.Contains(EVerkehrsmittel.Tram); }
        }
        #endregion

        #region ButtonTramWLB
        public bool ButtonTramWlbActive {
            get { return !_verkehrsmittelFilter["TramWlbFilter"].Active; }
            set { _verkehrsmittelFilter["TramWlbFilter"].Active = !value; }
        }

        public bool ButtonTramWlbVisible {
            get { return _verkehrsmittel.Contains(EVerkehrsmittel.TramWlb); }
        }
        #endregion

        #region ButtonCityBus
        public bool ButtonCityBusActive {
            get { return !_verkehrsmittelFilter["CityBusFilter"].Active; }
            set { _verkehrsmittelFilter["CityBusFilter"].Active = !value; }
        }

        public bool ButtonCityBusVisible {
            get { return _verkehrsmittel.Contains(EVerkehrsmittel.CityBus); }
        }
        #endregion

        #region ButtonNachtBus
        public bool ButtonNachtBusActive {
            get { return !_verkehrsmittelFilter["NachtBusFilter"].Active; }
            set { _verkehrsmittelFilter["NachtBusFilter"].Active = !value; }
        }

        public bool ButtonNachtBusVisible {
            get { return _verkehrsmittel.Contains(EVerkehrsmittel.NachtBus); }
        }
        #endregion

    }
}