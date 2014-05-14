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
using ViennaTrafficMonitor.Filter.AbfahrtenFilter;
using System.Threading;

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
            Intervall = 1000;
            _Timer = new Timer(_GetResponse, null, 0, Intervall);
        }

        private async void _GetResponse(object state) {
            Response = await RblRequesterProxy.GetProxyResponseAsync(_Rbls);
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
