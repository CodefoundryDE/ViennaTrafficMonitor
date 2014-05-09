using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Filter;
using ViennaTrafficMonitor.Model;

namespace ViennaTrafficMonitor.Deserializer {
    public class VtmResponse {

        public readonly Line Line { get; set; }

        public readonly Departure Departure { get; set; }

        public readonly EVerkehrsmittel Type { get; set; }

        public readonly List<TrafficInfoCategory> TrafficInfoCategories {get; set;}

        public readonly List<TrafficInfoCategoryGroup> TrafficInfoCategoryGroups { get; set; }

        public readonly LocationStop LocationStop { get; set; }

        public VtmResponse(Line line, Departure departure, LocationStop locstop, List<TrafficInfoCategory> tic, List<TrafficInfoCategoryGroup> ticg, String type) {
            Line = line;
            Departure = departure;
            LocationStop = locstop;
            TrafficInfoCategories = tic;
            TrafficInfoCategoryGroups = ticg;
            Type = Linie.Verkehrsmittel_Converter(type);
        }
    }
}
