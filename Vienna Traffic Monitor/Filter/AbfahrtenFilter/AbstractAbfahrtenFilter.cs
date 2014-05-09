using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Deserializer;

namespace ViennaTrafficMonitor.Filter {
    public abstract class AbstractAbfahrtenFilter : AbstractFilter<VtmResponse> {
        protected AbstractAbfahrtenFilter(string filterName, bool activated)
            : base (filterName, activated) {
        }
    }
}
