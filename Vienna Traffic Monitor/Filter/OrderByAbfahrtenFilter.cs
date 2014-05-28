using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Deserializer;
using ViennaTrafficMonitor.Model;

namespace ViennaTrafficMonitor.Filter {
    public class OrderByTimeRealAbfahrtenFilter : GenericFilter<VtmResponse> {

        private const int COUNT = 6;

        public OrderByTimeRealAbfahrtenFilter()
            : this(true) {
        }

        public OrderByTimeRealAbfahrtenFilter(bool active)
            : base(active) {

            Filter = (ICollection<VtmResponse> abfahrten) => {
                if (abfahrten == null) {
                    return new List<VtmResponse>();
                }
                var query = from response in abfahrten
                            orderby response.Departure.DepartureTime.TimeReal
                            select response;
                var result = query.ToList<VtmResponse>().Take(COUNT);
                return result.ToList();
            };
        }

    }
}
