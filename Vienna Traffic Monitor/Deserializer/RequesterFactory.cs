using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViennaTrafficMonitor.Deserializer {

    public static class RequesterFactory {

        public static IRequester Instance { get { return _createInstance(); } }

        private static IRequester _createInstance() {
            if (ViennaTrafficMonitor.Properties.Settings.Default.DummyRequester) {
                return new DummyRequester();
            }
            return new RblRequester();
        }

    }
}
