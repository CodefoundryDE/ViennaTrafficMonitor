using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Model;

namespace ViennaTrafficMonitor.Mapper {

    public sealed class HaltestellenMapperFactory {

        private static volatile IHaltestellenMapper instance = null;
        private static object syncRoot = new Object();

        private HaltestellenMapperFactory() { }

        public static IHaltestellenMapper Instance {
            get {
                if (instance == null) {
                    lock (syncRoot) {
                        if (instance == null) instance = _createInstance();
                    }
                }
                return instance;
            }
        }

        private static IHaltestellenMapper _createInstance() {
            return new HaltestellenMapper(new ConcurrentDictionary<int, IHaltestelle>());
        }

    }

}
