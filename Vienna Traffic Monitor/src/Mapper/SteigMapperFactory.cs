using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using ViennaTrafficMonitor.Model;
using VtmFramework.Factory;

namespace ViennaTrafficMonitor.Mapper {

    public sealed class SteigMapperFactory {

        private static volatile ISteigMapper instance = null;
        private static object syncRoot = new Object();

        private SteigMapperFactory() { }

        public static ISteigMapper Instance {
            get {
                if (instance == null) {
                    lock (syncRoot) {
                        if (instance == null) instance = new SteigMapper(new List<ISteig>());
                    }
                }
                return instance;
            }
        }

    }

}
