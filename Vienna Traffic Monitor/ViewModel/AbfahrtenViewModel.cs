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

namespace ViennaTrafficMonitor.ViewModel {

    public class AbfahrtenViewModel  : AbstractViewModel {
        private static IDictionary<string, AbstractAbfahrtenFilter> _Filters = new Dictionary<string, AbstractAbfahrtenFilter>();

        public static IDictionary<string, AbstractAbfahrtenFilter> Filters {
            get { return _Filters; }
            set { _Filters = value; }
        }

        private IList<VtmResponse> _Response;
        private IList<VtmResponse> Response {
            get { return _Response; }
            set {
                _Response = value;
                RaisePropertyChangedEvent("Abfahrten");
                RaisePropertyChangedEvent("Messages");
            }
        }

        private Func<IList<VtmResponse>> _Filter;
        public IList<VtmResponse> Abfahrten {
            get { return _Filter(); }
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
            _Rbls = from steig in sm.FindByHaltestelle(_Haltestelle.Id)
                    select steig.Rbl;
            _GetResponse();
            _Filter = () => {
                //zunächst leerer Filter;
                return _Response;
            };
        }

        private async void _GetResponse() {
            _Response = await RblRequesterProxy.GetProxyResponseAsync(_Rbls);
        }

        public static void AddFilter(AbstractAbfahrtenFilter filter) {
            Filters.Add(filter.FilterName, filter);
        }
    }
}
