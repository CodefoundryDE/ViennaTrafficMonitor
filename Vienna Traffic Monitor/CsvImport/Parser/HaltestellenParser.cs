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
using System.IO;
using VtmFramework.Error.Exceptions;

namespace ViennaTrafficMonitor.CsvImport.Parser
{
    public static class HaltestellenParser
    {

        public static ConcurrentDictionary<int, IHaltestelle> ReadFile(String filePath)
        {
            FileHelperEngine<HaltestelleRecord> engine = new FileHelperEngine<HaltestelleRecord>();
            ConcurrentDictionary<int, IHaltestelle> haltestellen = new ConcurrentDictionary<int, IHaltestelle>();

            try {
                engine.ErrorManager.ErrorMode = ErrorMode.SaveAndContinue;
                engine.Encoding = Encoding.UTF8;
                HaltestelleRecord[] res = engine.ReadFile(filePath);

                foreach (HaltestelleRecord haltestelle in res) {
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
            } catch (Exception ex) {
                //Dokument konnte nicht geparst werden (Nicht vorhanden/geöffnet)
                throw new VtmParsingException("Beim Versuch die Haltestellen zu parsen ist ein Fehler aufgetreten!", filePath, ex);
            }
            return (haltestellen);
        }
    }
}