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

namespace ViennaTrafficMonitor.CsvImport.Parser
{
    public static class LinienParser
    {

        public static ConcurrentDictionary<int, ILinie> ReadFile(String filePath)
        {
            FileHelperEngine<LinieRecord> engine = new FileHelperEngine<LinieRecord>();
            ConcurrentDictionary<int, ILinie> linien = new ConcurrentDictionary<int, ILinie>();

            engine.ErrorManager.ErrorMode = ErrorMode.SaveAndContinue;
            engine.Encoding = Encoding.UTF8;
            LinieRecord[] res = engine.ReadFile(filePath);

            foreach (LinieRecord linie in res)
            {
                //Übernehmen der eingelesenen Daten in das Programm-Model:
                ILinie transport = new Linie();
                transport.Bezeichnung = linie.Bezeichnung;
                transport.Echtzeit = linie.Echtzeit;
                transport.Id = linie.Id;
                transport.Reihenfolge = linie.Reihenfolge;
                transport.Verkehrsmittel = Linie.VerkehrsmittelConverter(linie.Verkehrsmittel);

                //Schreiben des Models in Collection für den Rückgabewert:
                linien.AddOrUpdate(transport.Id, transport, (key, oldValue) => transport);
            }
            return (linien);
        }
    }
}
