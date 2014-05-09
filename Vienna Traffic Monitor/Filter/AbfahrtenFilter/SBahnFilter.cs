using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Deserializer;
using ViennaTrafficMonitor.Model;

namespace ViennaTrafficMonitor.Filter.AbfahrtenFilter {
    public class SBahnFilter : AbstractAbfahrtenFilter {

        public SBahnFilter(bool activated)
            : base("RemoveSBahn", activated) {
            Filter = (IList<VtmResponse> abfahrten) => {
                var query = from response in abfahrten
                            where response.Typ != EVerkehrsmittel.SBahn
                            select response;

                return query.ToList<VtmResponse>();
            };
        }
    }
}
