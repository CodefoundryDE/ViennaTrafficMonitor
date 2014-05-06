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
        private Response _Response;

        private Func<ICollection<Departure>> _Filter;

        private Response Response {
            get { return _Response; }
            set {
                _Response = value;
                RaisePropertyChangedEvent("Abfahrten");
                RaisePropertyChangedEvent("Messages");
            }
        }

        public ICollection<Departure> Abfahrten {
            get { return _Filter(); }
        }

        public Message Message {
            get { return _Response.Message; }
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
            _Filter = () => {
                var lines = from monitor in Response.Data.Monitors
                            select monitor.Lines;
            };
        }

        private async void _GetResponse() {
            _Response = await RblRequester.GetResponseAsync(_Rbls);
        }
    }
}
