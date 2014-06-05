using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.CsvImport.Parser;
using ViennaTrafficMonitor.Model;
using VtmFramework.Error.Exceptions;

namespace ViennaTrafficMonitor.Mapper {

    public sealed class HaltestellenMapperFactory {

        private const string CSVDIR = "Ressources\\Csv\\";

        private static volatile IHaltestellenMapper instance = null;
        private static object syncRoot = new Object();

        private HaltestellenMapperFactory() { }

        public static IHaltestellenMapper Instance {
            get {
                if (instance == null) {
                    try {
                        lock (syncRoot) {
                            if (instance == null) instance = _createInstance();
                        }
                    } catch (VtmParsingException ex) {
                        throw new InvalidOperationException("Bei dem Versuch, die benötigten Daten aus den CSV-Dateien der Wiener Linien zu initialisieren trat ein Fehler auf" +
                   " Die Datei " + ex.Path + " ist möglicherweise nicht vorhanden oder in Bearbeitung." +
                   "Bitte stellen Sie sicher, dass die Datei vorhanden und in keinem anderen Programm geöffnet ist!", ex);
                    }
                }
                return instance;
            }
        }

        private static IHaltestellenMapper _createInstance() {
            try {
                ConcurrentDictionary<int, IHaltestelle> dict = HaltestellenParser.ReadFile(CSVDIR + "wienerlinien-ogd-haltestellen.csv");
                return new HaltestellenMapper(dict, LinienMapperFactory.Instance);
            } catch (VtmParsingException ex) {
                throw ex;
            }
        }

    }

}
