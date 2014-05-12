using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Filter.AbfahrtenFilter;
using ViennaTrafficMonitor.Mapper;

namespace ViennaTrafficMonitor.ViewModel {
    public static class AbfahrtenViewModelFactory {
        public static AbfahrtenViewModel GetInstance(int haltestellenId) {
            IHaltestellenMapper hm = HaltestellenMapperFactory.Instance;
            return new AbfahrtenViewModel(hm.Find(haltestellenId));
        }
    }
}
