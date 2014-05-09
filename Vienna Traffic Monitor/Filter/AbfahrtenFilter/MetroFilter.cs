using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Deserializer;
using ViennaTrafficMonitor.Model;

namespace ViennaTrafficMonitor.Filter.AbfahrtenFilter {
    public class MetroFilter : AbstractAbfahrtenFilter {

        public MetroFilter()  
            :base () {
                Filter = (ICollection<VtmResponse> abfahrten) => {
                    var query = from response in abfahrten
                                where response.Typ != EVerkehrsmittel.Metro
                                select response;

                    return query.ToList<VtmResponse>();
                };
        }
    }
}
