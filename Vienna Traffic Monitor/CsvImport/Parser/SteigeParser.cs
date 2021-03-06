﻿using FileHelpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViennaTrafficMonitor.Model;
using VtmFramework.Error.Exceptions;

namespace ViennaTrafficMonitor.CsvImport.Parser
{
    public static class SteigeParser
    {

        public static ConcurrentDictionary<int, ISteig> ReadFile(String filePath)
        {
            FileHelperEngine<SteigRecord> engine = new FileHelperEngine<SteigRecord>();
            ConcurrentDictionary<int, ISteig> steige = new ConcurrentDictionary<int, ISteig>();

            try {
                engine.ErrorManager.ErrorMode = ErrorMode.SaveAndContinue;
                engine.Encoding = Encoding.UTF8;
                SteigRecord[] res = engine.ReadFile(filePath);

                foreach (SteigRecord steig in res) {
                    //Übernehmen der eingelesenen Daten in das Programm-Model:
                    ISteig transport = new Steig();
                    Point stLoc = new Point(steig.XKoord, steig.YKoord);

                    transport.Id = steig.Id;
                    transport.Bereich = steig.Bereich;
                    transport.HaltestellenId = steig.HaltestellenId;
                    transport.LinienId = steig.LinienId;
                    transport.Rbl = steig.Rbl;
                    transport.Reihenfolge = steig.Reihenfolge;
                    transport.Name = steig.Name;
                    transport.Location = stLoc;
                    switch (steig.Richtung) {
                        case "H": {
                                transport.Richtung = ERichtung.Hin;
                                break;
                            }
                        case "R": {
                                transport.Richtung = ERichtung.Rueck;
                                break;
                            }
                    }

                    //Schreiben des Models in Collection für den Rückgabewert:
                    steige.AddOrUpdate(transport.Id, transport, (key, oldValue) => transport);
                }
            } catch (Exception ex) {
                //Dokument konnte nicht geparst werden (Nicht vorhanden/geöffnet)
                throw new VtmParsingException("Beim Versuch die Steige zu parsen ist ein Fehler aufgetreten!", filePath, ex);
            }
            return (steige);
        }
    }
}
