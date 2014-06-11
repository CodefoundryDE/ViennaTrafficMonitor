using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using ViennaTrafficMonitor.CsvImport.Parser;
using ViennaTrafficMonitor.Model;
using VtmFramework.Error.Exceptions;

namespace ViennaTrafficMonitor.Mapper {

    public sealed class SteigMapperFactory {

        private const string CSVDIR = "Ressources\\Csv\\";

        private static volatile ISteigMapper instance = null;
        private static object syncRoot = new Object();

        private SteigMapperFactory() { }

        public static ISteigMapper Instance {
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

        private static ISteigMapper _createInstance() {
            try {
                ConcurrentDictionary<int, ISteig> dict = SteigeParser.ReadFile(CSVDIR + "wienerlinien-ogd-steige.csv");
                return new SteigMapper(dict);
            } catch (VtmParsingException ex) {
                throw ex;
            }
        }

    }

}
