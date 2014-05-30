using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Mapper;

namespace ViennaTrafficMonitor.ViewModel {

    public static class HauptfensterViewModelFactory {

        public static HauptfensterViewModel Instance { get { return _createInstance(); } }

        private static HauptfensterViewModel _createInstance() {
            IHaltestellenMapper mapper = HaltestellenMapperFactory.Instance;
            return new HauptfensterViewModel(mapper);
        }

    }

}
