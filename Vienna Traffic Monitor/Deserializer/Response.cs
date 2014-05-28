﻿using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViennaTrafficMonitor.Deserializer {

    [GeneratedCodeAttribute("JSON to C#", "1.0")]
    public class Response {
        public Data Data { get; set; }
        public Message Message { get; set; }
    }

    [GeneratedCodeAttribute("JSON to C#", "1.0")]
    public class Geometry {
        public string Type { get; set; }
        public List<double> Coordinates { get; set; }
    }

    [GeneratedCodeAttribute("JSON to C#", "1.0")]
    public class Attributes {
        public int Rbl { get; set; }
    }

    [GeneratedCodeAttribute("JSON to C#", "1.0")]
    public class Properties {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Municipality { get; set; }
        public int MunicipalityId { get; set; }
        public string Type { get; set; }
        public string CoordName { get; set; }
        public Attributes Attributes { get; set; }
    }

    [GeneratedCodeAttribute("JSON to C#", "1.0")]
    public class LocationStop {
        public string Type { get; set; }
        public Geometry Geometry { get; set; }
        public Properties Properties { get; set; }
    }

    [GeneratedCodeAttribute("JSON to C#", "1.0")]
    public class DepartureTime {
        public string TimePlanned { get; set; }
        public string TimeReal { get; set; }
        public int Countdown { get; set; }

        public DepartureTime() {
            TimePlanned = "";
            TimeReal = "";
        }
    }

    [GeneratedCodeAttribute("JSON to C#", "1.0")]
    public class Vehicle {
        public string Name { get; set; }
        public string Towards { get; set; }
        public string Direction { get; set; }
        public string RichtungsId { get; set; }
        public bool BarrierFree { get; set; }
        public bool RealtimeSupported { get; set; }
        public bool Trafficjam { get; set; }
        public string Type { get; set; }
        public int LinienId { get; set; }
    }

    [GeneratedCodeAttribute("JSON to C#", "1.0")]
    public class Departure {
        public DepartureTime DepartureTime { get; set; }
        public Vehicle Vehicle { get; set; }

        public Departure(DepartureTime depTime) {
            DepartureTime = depTime;
        }
        public Departure() {}
    }

    [GeneratedCodeAttribute("JSON to C#", "1.0")]
    public class Departures {
        public List<Departure> Departure { get; set; }
    }

    [GeneratedCodeAttribute("JSON to C#", "1.0")]
    public class Line {
        public string Name { get; set; }
        public string Towards { get; set; }
        public string Direction { get; set; }
        public string Platform { get; set; }
        public string RichtungsId { get; set; }
        public bool BarrierFree { get; set; }
        public bool RealtimeSupported { get; set; }
        public bool Trafficjam { get; set; }
        public Departures Departures { get; set; }
        public string Type { get; set; }
        public int LineId { get; set; }
    }

    [GeneratedCodeAttribute("JSON to C#", "1.0")]
    public class Monitor {
        public LocationStop LocationStop { get; set; }
        public List<Line> Lines { get; set; }
    }

    [GeneratedCodeAttribute("JSON to C#", "1.0")]
    public class TrafficInfoCategory {
        public int Id { get; set; }
        public int RefTrafficInfoCategoryGroupId { get; set; }
        public string Name { get; set; }
        public string TrafficInfoNameList { get; set; }
        public string Title { get; set; }
    }

    [GeneratedCodeAttribute("JSON to C#", "1.0")]
    public class TrafficInfoCategoryGroup {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [GeneratedCodeAttribute("JSON to C#", "1.0")]
    public class Data {
        public List<Monitor> Monitors { get; set; }
        public List<TrafficInfoCategory> TrafficInfoCategories { get; set; }
        public List<TrafficInfoCategoryGroup> TrafficInfoCategoryGroups { get; set; }
    }

    [GeneratedCodeAttribute("JSON to C#", "1.0")]
    public class Message {
        public string Value { get; set; }
        public int MessageCode { get; set; }
        public string ServerTime { get; set; }
    }

}