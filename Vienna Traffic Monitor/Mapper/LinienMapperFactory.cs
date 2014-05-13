using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.CsvImport.Parser;
using ViennaTrafficMonitor.Model;
using VtmFramework.Factory;

namespace ViennaTrafficMonitor.Mapper {

    public sealed class LinienMapperFactory {

        private const string CSVDIR = "Ressources\\Csv\\";

        private static volatile ILinienMapper instance = null;
        private static object syncRoot = new Object();

        private LinienMapperFactory() { }

        public static ILinienMapper Instance {
            get {
                if (instance == null) {
                    lock (syncRoot) {
                        if (instance == null) instance = _createInstance();
                    }
                }
                return instance;
            }
        }

        private static ILinienMapper _createInstance() {
            ConcurrentDictionary<int, ILinie> dict = LinienParser.ReadFile(CSVDIR + "wienerlinien-ogd-linien.csv");
            return new LinienMapper(dict);
        }
    }

}
