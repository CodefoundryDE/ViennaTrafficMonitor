﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ViennaTrafficMonitor.Model {

    public class Haltestelle : IHaltestelle {

        public int Id { get; set; }
        public int Diva { get; set; }
        public string Name { get; set; }
        public Point Location { get; set; }

        public Haltestelle() {
            this.Id = 0;
            this.Diva = 0;
            this.Name = "";
            this.Location = new Point();
        }

        public Haltestelle(int id, int diva, string name, Point location) {
            this.Id = id;
            this.Diva = diva;
            this.Name = name;
            this.Location = location;
        }

        public override bool Equals(Object obj) {
            Haltestelle halt = obj as Haltestelle;
            if (halt == null) {
                return false;
            }
            return (halt).Id.Equals(this.Id);
        }

        public override int GetHashCode() {
            return Id.GetHashCode();
        }

        public override string ToString() {
            return this.Name;
        }
    }

}
