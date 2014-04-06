﻿using System;
namespace ViennaTrafficMonitor.Model {

    public interface IHaltestelle {
        int Diva { get; set; }
        int Id { get; set; }
        System.Windows.Point Location { get; set; }
        string Name { get; set; }
    }

}
