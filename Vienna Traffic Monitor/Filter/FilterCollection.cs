using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViennaTrafficMonitor.Filter {

    [Serializable]
    public class FilterCollection<T> :  Dictionary<string, IFilter<T>>, IFilter<T> {

        public FilterCollection() {
            Active = true;
        }

        public Func<ICollection<T>, ICollection<T>> Filter {
            get {
                return (ICollection<T> collection) => {
                    foreach (KeyValuePair<string, IFilter<T>> kvp in this) {
                        collection = kvp.Value.Filter(collection);
                    }
                    return collection;
                };
            }
            set {
                throw new NotSupportedException("Die Filter-Eigenschaft der FilterCollection kann nicht überschrieben werden!");
            }
        }

        public bool Active { get; set; }
    }

}
