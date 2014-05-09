using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViennaTrafficMonitor.Filter {
    public abstract class AbstractFilter<T> {
        private string _FilterName;

        public string FilterName {
            get { return _FilterName; }
            set { _FilterName = value; }
        }

        private Func<IList<T>, IList<T>> _Filter;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public Func<IList<T>, IList<T>> Filter {
            get { return _Filter; }
            set { _Filter = value; }
        }

        private bool _Active;
        public bool Active {
            get { return _Active; }
            set { _Active = value; }
        }

        protected AbstractFilter(string filterName, bool activated) {
            FilterName = filterName;
            Active = activated;
        }
    }
}
