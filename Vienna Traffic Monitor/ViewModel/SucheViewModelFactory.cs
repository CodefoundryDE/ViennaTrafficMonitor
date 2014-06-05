using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Mapper;

namespace ViennaTrafficMonitor.ViewModel {

    public static class SucheViewModelFactory {

        public static SucheViewModel Instance { get { return _createInstance(); } }

        private static SucheViewModel _createInstance() {
            IHaltestellenMapper haltestellenMapper = HaltestellenMapperFactory.Instance;
            return new SucheViewModel(haltestellenMapper);
        }

    }

}
