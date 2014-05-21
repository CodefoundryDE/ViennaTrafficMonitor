using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViennaTrafficMonitor.Filter {

    [Serializable]
    public class FilterCollection<T> : Dictionary<string, IFilter<T>>, IFilter<T> {

        public FilterCollection()
            : base() {
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

        #region Serialisierung
        /// <summary>
        /// Konstruktor zur Serialisierung, wird von der Codeanalyse erwartet
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected FilterCollection(SerializationInfo info, StreamingContext context)
            : base(info, context) {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            base.GetObjectData(info, context);
        }

        public override void OnDeserialization(object sender) {
            base.OnDeserialization(sender);
        }
        #endregion


        public double ButtonOpacity {
            get { throw new NotSupportedException("Die Opacity-Eigentschaft kann nur für einzelne Filter, nicht für die Collection abgefragt werden."); }
        }
    }

}
