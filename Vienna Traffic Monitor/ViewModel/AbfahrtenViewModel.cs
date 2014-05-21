﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtmFramework.ViewModel;
using ViennaTrafficMonitor.Deserializer;
using ViennaTrafficMonitor.Model;
using ViennaTrafficMonitor.Mapper;
using ViennaTrafficMonitor.Filter;
using ViennaTrafficMonitor.Filter;
using System.Threading;
using System.Net.Http;

namespace ViennaTrafficMonitor.ViewModel {

    public class AbfahrtenViewModel : AbstractViewModel {

        private FilterCollection<VtmResponse> _verkehrsmittelFilter;

        private ICollection<VtmResponse> _Response;
        private ICollection<VtmResponse> Response {
            set {
                _Response = value;
                RaisePropertyChangedEvent("Abfahrten");
            }
        }

        private Timer _Timer;

        public int Intervall { get; set; }

        private Timer _TimerCurrentTime;
        public string CurrentTime { get { return DateTime.Now.ToString("HH:mm:ss"); } }


        public ICollection<VtmResponse> Abfahrten {
            get {
                return _verkehrsmittelFilter.Filter(_Response);
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

        private readonly ISet<int> _Rbls;

        public AbfahrtenViewModel(IHaltestelle haltestelle)
            : base() {
            Haltestelle = haltestelle;
            ISteigMapper sm = SteigMapperFactory.Instance;
            List<ISteig> steige = sm.FindByHaltestelle(_Haltestelle.Id);
            _Rbls = new HashSet<int>(from steig in steige
                                     where steig.Rbl > 0
                                     select steig.Rbl);
            _InitializeFilters();
            Intervall = 60000;
            _Timer = new Timer(_GetResponse, null, 0, Intervall);
            _TimerCurrentTime = new Timer((object state) => {
                RaisePropertyChangedEvent("CurrentTime");
            }, null, 0, 60000);
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

        private void _InitializeFilters() {
            _verkehrsmittelFilter = new FilterCollection<VtmResponse>();
            _verkehrsmittelFilter.Add("MetroFilter", new AbfahrtenFilter(EVerkehrsmittel.Metro, false));
            _verkehrsmittelFilter.Add("CityBusFilter", new AbfahrtenFilter(EVerkehrsmittel.CityBus, false));
            _verkehrsmittelFilter.Add("NachtBusFilter", new AbfahrtenFilter(EVerkehrsmittel.NachtBus, false));
            _verkehrsmittelFilter.Add("SbahnFilter", new AbfahrtenFilter(EVerkehrsmittel.SBahn, false));
            _verkehrsmittelFilter.Add("TramFilter", new AbfahrtenFilter(EVerkehrsmittel.Tram, false));
            _verkehrsmittelFilter.Add("TramWlbFilter", new AbfahrtenFilter(EVerkehrsmittel.TramWlb, false));
            _verkehrsmittelFilter.Add("OrderByAbfahrt", new OrderByTimeRealAbfahrtenFilter(true));
        }
    }
}
