using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Deserializer;
using ViennaTrafficMonitor.Model;

namespace ViennaTrafficMonitor.Filter.AbfahrtenFilter {
    public class BusFilter : AbstractAbfahrtenFilter {

        public BusFilter()
            : base() {
            Filter = (ICollection<VtmResponse> abfahrten) => {
                var query = from response in abfahrten
                            where (response.Typ != EVerkehrsmittel.CityBus) || (response.Typ != EVerkehrsmittel.NachtBus)
                            select response;

                return query.ToList<VtmResponse>();
            };
        }
    }
}
