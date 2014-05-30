using Microsoft.Maps.MapControl.WPF;
using Microsoft.Maps.MapControl.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViennaTrafficMonitor.Mapper;
using ViennaTrafficMonitor.Properties;

namespace ViennaTrafficMonitor.ViewModel {

    static class MapViewModelFactory {

        public static MapViewModel Instance {
            get { return _createInstance(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Objekte verwerfen, bevor Bereich verloren geht")]
        private static MapViewModel _createInstance() {
            CredentialsProvider credentialsProvider = new ApplicationIdCredentialsProvider(Resources.BingApplicationId);
            MapViewModel vm = new MapViewModel(credentialsProvider, LinienMapperFactory.Instance);
            return vm;
        }

    }

}
