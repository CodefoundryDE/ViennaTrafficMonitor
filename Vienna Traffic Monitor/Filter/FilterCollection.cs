using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViennaTrafficMonitor.Filter {

    public class FilterCollection<T> : IFilter<T> {

        private IList<IFilter<T>> _filters;

        public FilterCollection() {
            Active = true;
            Clear();
        }

        /// <summary>
        /// Leert die FilterCollection.
        /// </summary>
        public void Clear() {
            _filters = new List<IFilter<T>>();
        }

        /// <summary>
        /// Fügt einen neuen Filter hinzu.
        /// </summary>
        /// <param name="filter"></param>
        public void Add(IFilter<T> filter) {
            _filters.Add(filter);
        }

        public Func<ICollection<T>, ICollection<T>> Filter {
            get {
                return (ICollection<T> collection) => {
                    foreach (IFilter<T> filter in _filters) {
                        collection = filter.Filter(collection);
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
