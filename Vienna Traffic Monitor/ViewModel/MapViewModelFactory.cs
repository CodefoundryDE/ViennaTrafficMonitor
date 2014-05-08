using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Mapper;

namespace ViennaTrafficMonitor.ViewModel {

    static class MapViewModelFactory {

        public static MapViewModel GetInstance() {
            MapViewModel vm = new MapViewModel(HaltestellenMapperFactory.Instance, LinienMapperFactory.Instance);
            // API-Key
            vm.MapControl.CredentialsProvider = new ApplicationIdCredentialsProvider(ViennaTrafficMonitor.Properties.Resources.BingApplicationId);
            return vm;
        }

    }

}
