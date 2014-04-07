using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;
using FileHelpers.RunTime;
using ViennaTrafficMonitor.Model;
using ViennaTrafficMonitor.CsvImport.Record;
using System.Collections.ObjectModel;

namespace ViennaTrafficMonitor.CsvImport.Parser {
    public static class LinienParser {

        public static ICollection<ILinie> ReadFile(String filePath) {
            FileHelperEngine<LinieRecord> engine = new FileHelperEngine<LinieRecord>();
            ICollection<ILinie> linien = new Collection<ILinie> ();
            
                

            engine.ErrorManager.ErrorMode = ErrorMode.SaveAndContinue;

            LinieRecord[] res = engine.ReadFile(filePath);

            foreach (LinieRecord linie in res) {
                //Übernehmen der eingelesenen Daten in das Programm-Model:
                ILinie transport = new Linie();
                transport.Bezeichnung = linie.Bezeichnung;
                transport.Echtzeit = linie.Echtzeit;
                transport.Id = linie.Id;
                transport.Reihenfolge = linie.Reihenfolge;
                switch (linie.Verkehrsmittel) {
                    case "ptTram": {
                        transport.Verkehrsmittel = EVerkehrsmittel.Tram;
                        break;
                        }
                    case "ptBusCity": {
                            transport.Verkehrsmittel = EVerkehrsmittel.CityBus;
                            break;
                        }
                    case "ptBusNight": {
                        transport.Verkehrsmittel = EVerkehrsmittel.NachtBus;
                        break;
                        }
                    case "ptTrainS": {
                        transport.Verkehrsmittel = EVerkehrsmittel.SBahn;
                        break;
                        }
                    case "ptMetro": {
                        transport.Verkehrsmittel = EVerkehrsmittel.Metro;
                        break;
                        }
                    case "ptTramWLB": {
                        transport.Verkehrsmittel = EVerkehrsmittel.TramWlb;
                        break;
                        }

                }

                //Schreiben des Models in Collection für den Rückgabewert:
                linien.Add(transport);
            }
            return (linien);
        }
    }
}
