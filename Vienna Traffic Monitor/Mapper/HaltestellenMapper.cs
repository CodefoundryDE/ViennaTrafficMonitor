using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Model;

namespace ViennaTrafficMonitor.Mapper {

    public class HaltestellenMapper : IHaltestellenMapper {

        private IEnumerable<IHaltestelle> data;

        public HaltestellenMapper(IEnumerable<IHaltestelle> data) {
            this.data = data;
        }

        public IHaltestelle Find(int id) {
            IEnumerable<IHaltestelle> query = from haltestelle in data
                                              where haltestelle.Id == id
                                              select haltestelle;
            return query.First();
        }

    }

}
