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
            if (AbfahrtenViewModel.Filters == null) {
                _InitializeFilters();
            }
            IHaltestellenMapper hm = HaltestellenMapperFactory.Instance;
            return new AbfahrtenViewModel(hm.Find(haltestellenId));
        }
        private static void _InitializeFilters() {
            //Hinzufügen aller bekannten Abfahrtsfilter zum AbfahrtenViewModel:
            AbfahrtenViewModel.AddFilter (new MetroFilter(false));
            AbfahrtenViewModel.AddFilter (new SBahnFilter(false));
            AbfahrtenViewModel.AddFilter (new TramFilter(false));
            AbfahrtenViewModel.AddFilter(new BusFilter(false));
        }
    }
}
