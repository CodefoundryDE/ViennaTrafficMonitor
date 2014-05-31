using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViennaTrafficMonitor.Filter {

    public class GenericFilter<T> : IFilter<T> {

        public GenericFilter() {
            Active = true;
        }

        public GenericFilter(bool active) {
            Active = active;
        }

        public GenericFilter(Func<ICollection<T>, ICollection<T>> filter)
            : this() {
            Filter = filter;
        }

        private Func<ICollection<T>, ICollection<T>> _filter;
        public Func<ICollection<T>, ICollection<T>> Filter {
            get {
                if (Active)
                    return _filter;
                else
                    return (ICollection<T> collection) => { return collection; };
            }
            set { _filter = value; }
        }

        public bool Active { get; set; }
    }
}
