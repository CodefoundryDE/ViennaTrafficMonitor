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
using VtmFramework.Error;

namespace ViennaTrafficMonitor.ViewModel
{

    public class AbfahrtenViewModel : AbstractViewModel
    {

        #region Properties/Variablen
        public event EventHandler ConnectionLost;

        private FilterCollection<VtmResponse> _verkehrsmittelFilter;

        private ISet<EVerkehrsmittel> _verkehrsmittel;

        private ICollection<VtmResponse> _response;
        private ICollection<VtmResponse> Response
        {
            get { return _response; }
            set
            {
                _response = value;
                RaisePropertyChangedEvent("Abfahrten");
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private Timer _timer;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private Timer _timerCurrentTime;

        private const int ResultCount = 6;

        public int Intervall { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public string CurrentTime { get { return DateTime.Now.ToString("HH:mm:ss"); } }

        public ICollection<VtmResponse> Abfahrten
        {
            get
            {
                ICollection<VtmResponse> response = _verkehrsmittelFilter.Filter(Response);
                if (response.Count == ResultCount)
                {
                    return response;
                }
                else
                {
                    while (response.Count < ResultCount)
                    {
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

        private IHaltestelle _haltestelle;
        public IHaltestelle Haltestelle
        {
            get { return _haltestelle; }
            private set
            {
                _haltestelle = value;
                RaisePropertyChangedEvent("Haltestelle");
            }
        }

        private ISet<int> _rbls;
        #endregion

        public AbfahrtenViewModel(IHaltestelle haltestelle)
        {
            Haltestelle = haltestelle;
            _InitializeRbls();
            _InitializeVerkehrsmittel();
            _InitializeFilters();
            _StartRequestIntervall();
        }

        #region Initialisierung
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Mobility", "CA1601:DoNotUseTimersThatPreventPowerStateChanges")]
        private void _StartRequestIntervall()
        {
            Intervall = 10000;
            _timer = new Timer(_GetResponse, null, 0, Intervall);
            _timerCurrentTime = new Timer((object state) =>
            {
                RaisePropertyChangedEvent("CurrentTime");
            }, null, 0, 500);
        }

        private void _InitializeRbls()
        {
            var sm = SteigMapperFactory.Instance;
            ICollection<ISteig> steige = sm.FindByHaltestelle(_haltestelle.Id);
            _rbls = new HashSet<int>(from steig in steige
                                     where steig.Rbl > 0
                                     select steig.Rbl);
        }

        private async void _GetResponse(object state)
        {
            bool error = false;
            try
            {
                Response = await RblRequesterProxy.GetProxyResponseAsync(_rbls);
            }
            catch (HttpRequestException)
            {
                // Im catch-Block ist kein await erlaubt - das kommt erst mit C# 6.0
                error = true;
            }
            catch
            {
                error = false;
            }
            if (!error) return;
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            var result = await RaiseError("Fehler", "Es konnte keine Anfrage an die API der Wiener Linien gestellt werden.", VtmFramework.Error.EErrorButtons.RetryCancel);
            switch (result)
            {
                case EErrorResult.Retry:
                {
                    _GetResponse(state);
                    _timer.Change(0, Intervall);
                    break;
                }
                default:
                {
                    var handler = ConnectionLost;
                    if (handler != null)
                    {
                        handler(this, new EventArgs());
                    }
                    break;
                }
            }
        }


        private void _InitializeVerkehrsmittel()
        {
            var lm = LinienMapperFactory.Instance;
            var linien = lm.FindByHaltestelle(Haltestelle.Id);
            _verkehrsmittel = new HashSet<EVerkehrsmittel>();
            foreach (var linie in linien)
            {
                _verkehrsmittel.Add(linie.Verkehrsmittel);
            }
        }

        private void _InitializeFilters()
        {
            _verkehrsmittelFilter = new FilterCollection<VtmResponse>
            {
                {"MetroFilter", new AbfahrtenFilter(EVerkehrsmittel.Metro, false)},
                {"CityBusFilter", new AbfahrtenFilter(EVerkehrsmittel.CityBus, false)},
                {"NachtBusFilter", new AbfahrtenFilter(EVerkehrsmittel.NachtBus, false)},
                {"SbahnFilter", new AbfahrtenFilter(EVerkehrsmittel.SBahn, false)},
                {"TramFilter", new AbfahrtenFilter(EVerkehrsmittel.Tram, false)},
                {"TramVrtFilter", new AbfahrtenFilter(EVerkehrsmittel.TramVrt, false)},
                {"OrderByAbfahrt", new OrderAbfahrtenFilter(EAbfahrtenOrder.TimeRealAsc, ResultCount, true)}
            };
        }
        #endregion

        #region ButtonUbahn
        public bool ButtonMetroActive
        {
            get { return !_verkehrsmittelFilter["MetroFilter"].Active; }
            set
            {
                _verkehrsmittelFilter["MetroFilter"].Active = !value;
                RaisePropertyChangedEvent("Abfahrten");
            }
        }

        public bool ButtonMetroVisible
        {
            get { return _verkehrsmittel.Contains(EVerkehrsmittel.Metro); }
        }
        #endregion

        #region ButtonTram
        public bool ButtonTramActive
        {
            get { return !_verkehrsmittelFilter["TramFilter"].Active; }
            set
            {
                _verkehrsmittelFilter["TramFilter"].Active = !value;
                RaisePropertyChangedEvent("Abfahrten");
            }
        }

        public bool ButtonTramVisible
        {
            get { return _verkehrsmittel.Contains(EVerkehrsmittel.Tram); }
        }
        #endregion

        #region ButtonTramVRT
        public bool ButtonTramVrtActive
        {
            get { return !_verkehrsmittelFilter["TramVrtFilter"].Active; }
            set
            {
                _verkehrsmittelFilter["TramVrtFilter"].Active = !value;
                RaisePropertyChangedEvent("Abfahrten");
            }
        }

        public bool ButtonTramVrtVisible
        {
            get { return _verkehrsmittel.Contains(EVerkehrsmittel.TramVrt); }
        }
        #endregion

        #region ButtonCityBus
        public bool ButtonCityBusActive
        {
            get { return !_verkehrsmittelFilter["CityBusFilter"].Active; }
            set
            {
                _verkehrsmittelFilter["CityBusFilter"].Active = !value;
                RaisePropertyChangedEvent("Abfahrten");
            }
        }

        public bool ButtonCityBusVisible
        {
            get { return _verkehrsmittel.Contains(EVerkehrsmittel.CityBus); }
        }
        #endregion

        #region ButtonNachtBus
        public bool ButtonNachtBusActive
        {
            get { return !_verkehrsmittelFilter["NachtBusFilter"].Active; }
            set
            {
                _verkehrsmittelFilter["NachtBusFilter"].Active = !value;
                RaisePropertyChangedEvent("Abfahrten");
            }
        }

        public bool ButtonNachtBusVisible
        {
            get { return _verkehrsmittel.Contains(EVerkehrsmittel.NachtBus); }
        }
        #endregion
    }
}