using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Model;
using VtmFramework.Mapper;

namespace ViennaTrafficMonitor.Mapper {

    public class SteigMapper : ISteigMapper {

        private ConcurrentDictionary<int, ISteig> _data;

        public SteigMapper(ConcurrentDictionary<int, ISteig> data) {
            this._data = data;
        }

        public ISteig Find(int id) {
            return _data[id];
        }
    }

}
