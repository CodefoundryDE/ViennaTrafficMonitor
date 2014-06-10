using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.CsvImport.Parser;
using ViennaTrafficMonitor.Model;
using VtmFramework.Error.Exceptions;
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

        private static ILinienMapper _createInstance() {
            try {
                ConcurrentDictionary<int, ILinie> dict = LinienParser.ReadFile(CSVDIR + "wienerlinien-ogd-linien.csv");
                return new LinienMapper(dict);
            } catch (VtmParsingException ex) {
                throw ex;
            }
        }
    }

}
