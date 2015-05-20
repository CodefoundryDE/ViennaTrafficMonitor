using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.CsvImport.Parser;
using ViennaTrafficMonitor.Model;
using VtmFramework.Error.Exceptions;

namespace ViennaTrafficMonitor.Mapper
{

    public static class LinienMapperFactory
    {

        private const string Csvdir = "Ressources\\Csv\\";

        private static volatile ILinienMapper _instance;
        private static readonly object SyncRoot = new Object();

        public static ILinienMapper Instance
        {
            get
            {
                if (_instance != null) return _instance;
                try
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null) _instance = _createInstance();
                    }
                }
                catch (VtmParsingException ex)
                {
                    throw new InvalidOperationException("Bei dem Versuch, die benötigten Daten aus den CSV-Dateien der Wiener Linien zu initialisieren trat ein Fehler auf" +
                                                        " Die Datei " + ex.Path + " ist möglicherweise nicht vorhanden oder in Bearbeitung." +
                                                        "Bitte stellen Sie sicher, dass die Datei vorhanden und in keinem anderen Programm geöffnet ist!", ex);
                }
                return _instance;
            }

        }

        private static ILinienMapper _createInstance()
        {
            var dict = LinienParser.ReadFile(Csvdir + "wienerlinien-ogd-linien.csv");
            return new LinienMapper(dict);
        }
    }

}
