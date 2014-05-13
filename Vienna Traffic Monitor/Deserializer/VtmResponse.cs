﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Filter;
using ViennaTrafficMonitor.Model;

namespace ViennaTrafficMonitor.Deserializer {
    public class VtmResponse {

        public Line Line { get; private set; }

        public  Departure Departure { get; private set; }

        public EVerkehrsmittel Typ { get; private  set; }

        public List<TrafficInfoCategory> TrafficInfoCategories {get; private set;}

        public List<TrafficInfoCategoryGroup> TrafficInfoCategoryGroups { get; private set; }

        public LocationStop LocationStop { get; private set; }

        public VtmResponse(Line line, Departure departure, LocationStop locstop, List<TrafficInfoCategory> tic, List<TrafficInfoCategoryGroup> ticg, String type) {
            Line = line;
            Departure = departure;
            LocationStop = locstop;
            TrafficInfoCategories = tic;
            TrafficInfoCategoryGroups = ticg;
            Typ = Linie.VerkehrsmittelConverter(type);
        }
    }
}