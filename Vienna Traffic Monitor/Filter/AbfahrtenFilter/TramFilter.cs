using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Deserializer;
using ViennaTrafficMonitor.Model;

namespace ViennaTrafficMonitor.Filter.AbfahrtenFilter {
    public class TramFilter : AbstractAbfahrtenFilter {

        public TramFilter()
            : base() {
            Filter = (ICollection<VtmResponse> abfahrten) => {
                var query = from response in abfahrten
                            where response.Typ != EVerkehrsmittel.Tram
                            select response;
                query = from response in query
                        where response.Typ != EVerkehrsmittel.TramWlb
                        select response;

                return query.ToList<VtmResponse>();
            };
        }
    }
}