using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;
using FileHelpers.RunTime;
using ViennaTrafficMonitor.Model;
using ViennaTrafficMonitor.CsvImport.Record;
using System.Collections.Concurrent;
using System.Windows;

namespace ViennaTrafficMonitor.CsvImport.Parser
{
    public static class HaltestellenParser
    {

        public static ConcurrentDictionary<int, IHaltestelle> ReadFile(String filePath)
        {
            FileHelperEngine<HaltestelleRecord> engine = new FileHelperEngine<HaltestelleRecord>();
            ConcurrentDictionary<int, IHaltestelle> haltestellen = new ConcurrentDictionary<int, IHaltestelle>();

            engine.ErrorManager.ErrorMode = ErrorMode.SaveAndContinue;
            engine.Encoding = Encoding.UTF8;
            HaltestelleRecord[] res = engine.ReadFile(filePath);

            foreach (HaltestelleRecord haltestelle in res)
            {
                //Übernehmen der eingelesenen Daten in das Programm-Model:
                IHaltestelle transport = new Haltestelle();
                Point HsLoc = new Point(haltestelle.XKoord, haltestelle.YKoord);

                transport.Diva = haltestelle.Diva;
                transport.Id = haltestelle.Id;
                transport.Location = HsLoc;
                transport.Name = haltestelle.Name;

                //Schreiben des Models in Collection für den Rückgabewert:
                haltestellen.AddOrUpdate(transport.Id, transport, (key, oldValue) => transport);
            }
            return (haltestellen);
        }
    }
}