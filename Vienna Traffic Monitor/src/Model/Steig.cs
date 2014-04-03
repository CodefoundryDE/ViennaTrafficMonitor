using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ViennaTrafficMonitor.Model {

    public enum ERichtung { Hin, Rueck };

    public class Steig : ISteig {

        public int Id { get; set; }
        public int LinienId { get; set; }
        public int HaltestellenId { get; set; }
        public ERichtung Richtung { get; set; }
        public int Reihenfolge { get; set; }
        public int Rbl { get; set; }
        public int Bereich { get; set; }
        public string Name { get; set; }
        public Point Location { get; set; }

        public Steig() {
            this.Id = 0;
            this.LinienId = 0;
            this.HaltestellenId = 0;
            this.Richtung = ERichtung.Hin;
            this.Reihenfolge = 0;
            this.Rbl = 0;
            this.Bereich = 0;
            this.Name = "";
            this.Location = new Point();
        }

        public Steig(int id, int linienid, int haltestellenid, ERichtung richtung, int reihenfolge, int rbl, int bereich, string name, Point location) {
            this.Id = id;
            this.LinienId = linienid;
            this.HaltestellenId = haltestellenid;
            this.Richtung = richtung;
            this.Reihenfolge = reihenfolge;
            this.Rbl = rbl;
            this.Bereich = bereich;
            this.Name = name;
            this.Location = location;
        }

    }

}
