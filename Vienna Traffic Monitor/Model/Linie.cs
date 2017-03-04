
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViennaTrafficMonitor.Model
{

    public enum EVerkehrsmittel { NoInfo, Metro, SBahn, Tram, NachtBus, CityBus, TramVrt, TramWlb };

    public class Linie : ILinie
    {

        private static IReadOnlyDictionary<string, EVerkehrsmittel> VerkehrsmittelMapping = new Dictionary<string, EVerkehrsmittel>
        {
            { string.Empty, EVerkehrsmittel.NoInfo },
            { "ptTram", EVerkehrsmittel.Tram },
            { "ptBusCity", EVerkehrsmittel.CityBus },
            { "ptBusNight", EVerkehrsmittel.NachtBus },
            { "ptTrainS", EVerkehrsmittel.SBahn },
            { "ptMetro", EVerkehrsmittel.Metro },
            { "ptTramVRT", EVerkehrsmittel.TramVrt },
            { "ptTramWLB", EVerkehrsmittel.TramWlb },
        };

        public int Id { get; set; }
        public string Bezeichnung { get; set; }
        public int Reihenfolge { get; set; }
        public bool Echtzeit { get; set; }
        public EVerkehrsmittel Verkehrsmittel { get; set; }

        public Linie()
        {
            this.Id = 0;
            this.Bezeichnung = "";
            this.Reihenfolge = 0;
            this.Echtzeit = false;
            this.Verkehrsmittel = EVerkehrsmittel.Metro;
        }

        public Linie(int id, string bezeichnung, int reihenfolge, bool echtzeit, EVerkehrsmittel verkehrsmittel)
        {
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

        public static EVerkehrsmittel VerkehrsmittelConverter(String type)
        {

            if (VerkehrsmittelMapping.ContainsKey(type))
            {
                return VerkehrsmittelMapping[type];
            }
            return EVerkehrsmittel.NoInfo;
        }



    }

}
