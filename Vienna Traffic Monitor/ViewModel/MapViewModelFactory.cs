using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Mapper;

namespace ViennaTrafficMonitor.ViewModel {

    static class MapViewModelFactory {

        public static MapViewModel Instance {
            get { return _createInstance(); }
        }
        

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Objekte verwerfen, bevor Bereich verloren geht")]
        public static MapViewModel _createInstance() {
            MapViewModel vm = new MapViewModel(LinienMapperFactory.Instance);
            // API-Key
            vm.MapControl.CredentialsProvider = new ApplicationIdCredentialsProvider(ViennaTrafficMonitor.Properties.Resources.BingApplicationId);
            return vm;
        }

    }

}
