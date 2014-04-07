using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Model;

namespace ViennaTrafficMonitor.Mapper {

    public class HaltestellenMapper : IHaltestellenMapper {

        private ConcurrentDictionary<int, IHaltestelle> _data;

        public HaltestellenMapper(ConcurrentDictionary<int, IHaltestelle> data) {
            this._data = data;
        }

        public IHaltestelle Find(int id) {
            return _data[id];
        }

    }

}
