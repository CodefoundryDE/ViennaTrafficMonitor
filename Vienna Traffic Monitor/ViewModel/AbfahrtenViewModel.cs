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

        private readonly IEnumerable<int> _Rbls;

        public AbfahrtenViewModel(IHaltestelle haltestelle)
            : base() {
            Haltestelle = haltestelle;
            ISteigMapper sm = SteigMapperFactory.Instance;
            List<ISteig> steige = sm.FindByHaltestelle(_Haltestelle.Id);
            _Rbls = from steig in steige
                    select steig.Rbl;
            _InitializeFilters();
            _GetResponse().Wait();
        }

        private async Task<bool> _GetResponse() {
            _Response = await RblRequesterProxy.GetProxyResponseAsync(_Rbls);
            return true;
        }

        private void _InitializeFilters() {
            _verkehrsmittelFilter = new FilterCollection<VtmResponse>();
            _verkehrsmittelFilter.Add("MetroFilter", new AbfahrtenFilter(EVerkehrsmittel.Metro));
            _verkehrsmittelFilter.Add("CityBusFilter", new AbfahrtenFilter(EVerkehrsmittel.CityBus));
            _verkehrsmittelFilter.Add("NachtBusFilter", new AbfahrtenFilter(EVerkehrsmittel.NachtBus));
            _verkehrsmittelFilter.Add("SbahnFilter", new AbfahrtenFilter(EVerkehrsmittel.SBahn));
            _verkehrsmittelFilter.Add("TramFilter", new AbfahrtenFilter(EVerkehrsmittel.Tram));
            _verkehrsmittelFilter.Add("TramWlbFilter", new AbfahrtenFilter(EVerkehrsmittel.TramWlb));
        }
    }
}
