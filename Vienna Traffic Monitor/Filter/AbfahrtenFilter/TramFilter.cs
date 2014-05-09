using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Deserializer;
using ViennaTrafficMonitor.Model;

namespace ViennaTrafficMonitor.Filter.AbfahrtenFilter {
    public class TramFilter : AbstractAbfahrtenFilter {

        public TramFilter(bool activated)
            : base("RemoveTram", activated) {
            Filter = (IList<VtmResponse> abfahrten) => {
                var query = from response in abfahrten
                            where response.Typ != (EVerkehrsmittel.Tram | EVerkehrsmittel.TramWlb)
                            select response;

                return query.ToList<VtmResponse>();
            };
        }
    }
}