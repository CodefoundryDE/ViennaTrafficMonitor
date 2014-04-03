using System;
namespace ViennaTrafficMonitor.Model {

    interface ILinie {
        string Bezeichnung { get; set; }
        bool Echtzeit { get; set; }
        int Id { get; set; }
        int Reihenfolge { get; set; }
        EVerkehrsmittel Verkehrsmittel { get; set; }
    }

}
