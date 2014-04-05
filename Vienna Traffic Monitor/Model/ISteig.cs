using System;
using System.Windows;

namespace ViennaTrafficMonitor.Model {

    public interface ISteig {
        int Bereich { get; set; }
        int HaltestellenId { get; set; }
        int Id { get; set; }
        int LinienId { get; set; }
        Point Location { get; set; }
        string Name { get; set; }
        int Rbl { get; set; }
        int Reihenfolge { get; set; }
        ERichtung Richtung { get; set; }
    }

}
