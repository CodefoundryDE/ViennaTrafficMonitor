using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtmFramework.ViewModel;
using ViennaTrafficMonitor.Deserializer;
using ViennaTrafficMonitor.Model;
using ViennaTrafficMonitor.Mapper;

namespace ViennaTrafficMonitor.ViewModel {
    
    public class AbfahrtenViewModel : AbstractViewModel {
        private IList<VtmResponse> _Response;

        private Func<ICollection<Departure>> _Filter;

        private IList<VtmResponse> Response {
            get { return _Response; }
            set {
                _Response = value;
                RaisePropertyChangedEvent("Abfahrten");
            }
        }

        public ICollection<Departure> Abfahrten {
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


        public AbfahrtenViewModel(int HaltestellenId)
            : base() {
            IHaltestellenMapper hm = HaltestellenMapperFactory.Instance;
            Haltestelle = hm.Find(HaltestellenId);
            ISteigMapper sm = SteigMapperFactory.Instance;
            _Rbls = from steig in sm.FindByHaltestelle(HaltestellenId)
                    select steig.Rbl;
            _GetResponse();
        }

        private async void _GetResponse() {
            _Response = await RblRequesterProxy.GetProxyResponseAsync(_Rbls);
        }
    }
}
