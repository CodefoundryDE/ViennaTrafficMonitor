using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViennaTrafficMonitor.Filter {

    public interface IFilter<T> {
        
        /// <summary>
        /// Lambda-Ausdruck zum Filtern einer Collection
        /// </summary>
        Func<ICollection<T>, ICollection<T>> Filter { get; set; }

        /// <summary>
        /// Status des Filters
        /// </summary>
        bool Active { get; set; }

    }

}
