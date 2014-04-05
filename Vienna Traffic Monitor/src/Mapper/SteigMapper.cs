using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Model;
using VtmFramework.Mapper;

namespace ViennaTrafficMonitor.Mapper {

    public class SteigMapper : ISteigMapper {

        private IEnumerable<ISteig> data;

        public SteigMapper(IEnumerable<ISteig> data) {
            this.data = data;
        }

        public ISteig Find(int id) {
            IEnumerable<ISteig> query = from steig in data
                                        where steig.Id == id
                                        select steig;
            return query.First();
        }
    }

}
