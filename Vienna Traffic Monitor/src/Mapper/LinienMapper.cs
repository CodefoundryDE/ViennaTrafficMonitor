using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Model;

namespace ViennaTrafficMonitor.Mapper {

    public class LinienMapper : ILinienMapper {

        private IEnumerable<ILinie> data;

        public LinienMapper(IEnumerable<ILinie> data) {
            this.data = data;
        }

        public ILinie Find(int id) {
            IEnumerable<ILinie> query = from linie in data
                                        where linie.Id == id
                                        select linie;
            return query.First();
        }

    }

}
