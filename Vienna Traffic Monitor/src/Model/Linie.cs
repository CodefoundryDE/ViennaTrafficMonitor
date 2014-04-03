
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViennaTrafficMonitor.Model {

    public enum EVerkehrsmittel { Metro, SBahn, Tram, NachtBus, CityBus, TramWlb };

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

    }

}
