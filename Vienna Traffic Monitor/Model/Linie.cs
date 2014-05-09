
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViennaTrafficMonitor.Model {

    public enum EVerkehrsmittel { NoInfo, Metro, SBahn, Tram, NachtBus, CityBus, TramWlb };

    public class Linie : ILinie {

        public int Id { get; set; }
        public string Bezeichnung { get; set; }
        public int Reihenfolge { get; set; }
        public bool Echtzeit { get; set; }
        public EVerkehrsmittel Verkehrsmittel { get; set; }

        public Linie() {
            this.Id = 0;
            this.Bezeichnung = "";
            this.Reihenfolge = 0;
            this.Echtzeit = false;
            this.Verkehrsmittel = EVerkehrsmittel.Metro;
        }

        public Linie(int id, string bezeichnung, int reihenfolge, bool echtzeit, EVerkehrsmittel verkehrsmittel) {
            this.Id = id;
            this.Bezeichnung = bezeichnung;
            this.Reihenfolge = reihenfolge;
            this.Echtzeit = echtzeit;
            this.Verkehrsmittel = verkehrsmittel;
        }

        public override bool Equals(Object obj)
        {
            Linie line = obj as Linie;
            if (line == null)
            {
                return false;
            }
            return (line).Id.Equals(this.Id);
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static EVerkehrsmittel VerkehrsmittelConverter(String type) {
            EVerkehrsmittel verkehrsmittel;
            switch (type) {
                case "ptTram": {
                        verkehrsmittel = EVerkehrsmittel.Tram;
                        break;
                    }
                case "ptBusCity": {
                        verkehrsmittel = EVerkehrsmittel.CityBus;
                        break;
                    }
                case "ptBusNight": {
                        verkehrsmittel = EVerkehrsmittel.NachtBus;
                        break;
                    }
                case "ptTrainS": {
                        verkehrsmittel = EVerkehrsmittel.SBahn;
                        break;
                    }
                case "ptMetro": {
                        verkehrsmittel = EVerkehrsmittel.Metro;
                        break;
                    }
                case "ptTramWLB": {
                        verkehrsmittel = EVerkehrsmittel.TramWlb;
                        break;
                    }
                default: {
                    verkehrsmittel = EVerkehrsmittel.NoInfo;
                    break;
                    }

            }
            return verkehrsmittel;
        }

        

    }

}
