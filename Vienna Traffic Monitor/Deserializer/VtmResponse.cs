using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViennaTrafficMonitor.Deserializer {
    public class VtmResponse {

        public readonly Line Line { get; set; }

        public readonly Departure Departure { get; set; }

        public readonly TrafficInfoCategory TrafficInfoCategory {get; set;}

        public readonly TrafficInfoCategoryGroup TrafficInfoCategoryGroup { get; set; }

        public readonly LocationStop LocationStop { get; set; }

        public VtmResponse(Line line, Departure departure, Message message, LocationStop locstop, TrafficInfoCategory tic, TrafficInfoCategoryGroup ticg) {
            Line = line;
            Departure = departure;
            LocationStop = locstop;
            TrafficInfoCategory = tic;
            TrafficInfoCategoryGroup = ticg;
        }
    }
}
