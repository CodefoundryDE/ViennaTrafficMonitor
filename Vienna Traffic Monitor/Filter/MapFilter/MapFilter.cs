using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Model;

namespace ViennaTrafficMonitor.Filter.MapFilter {

    public class MapFilter : GenericFilter<KeyValuePair<ILinie, List<IHaltestelle>>> {

        public MapFilter(EVerkehrsmittel verkehrsmittel)
            : this(verkehrsmittel, true) {

        }

        public MapFilter(EVerkehrsmittel verkehrsmittel, bool active)
            : base(active) {
            this.Filter = (ICollection<KeyValuePair<ILinie, List<IHaltestelle>>> collection) => {
                var query = from linie in collection
                            where linie.Key.Verkehrsmittel != verkehrsmittel
                            select linie;
                return query.ToDictionary(x => x.Key, x => x.Value);
            };
        }

    }

}
