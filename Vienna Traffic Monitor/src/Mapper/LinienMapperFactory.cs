using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Model;
using VtmFramework.Factory;

namespace ViennaTrafficMonitor.Mapper {

    public sealed class LinienMapperFactory {

        private static volatile ILinienMapper instance = null;
        private static object syncRoot = new Object();

        private LinienMapperFactory() { }

        public static ILinienMapper Instance {
            get {
                if (instance == null) {
                    lock (syncRoot) {
                        if (instance == null) instance = new LinienMapper();
                    }
                }
                return instance;
            }
        }
    }

}
