using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Deserializer;
using ViennaTrafficMonitor.Model;

namespace ViennaTrafficMonitor.Filter {
    public class OrderByTimeRealAbfahrtenFilter : GenericFilter<VtmResponse> {

        public OrderByTimeRealAbfahrtenFilter( bool active = true)
            : base(active) {

            Filter = (ICollection<VtmResponse> abfahrten) => {
                if (abfahrten == null) {
                    return new List<VtmResponse>();
                }
                var query = from response in abfahrten
                            orderby response.Departure.DepartureTime.TimePlanned                            
                            select response;
                var result =  query.ToList<VtmResponse>().Take(35);
                return result.ToList();
            };
        }

    }
}
