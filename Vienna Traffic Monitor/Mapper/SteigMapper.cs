using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Model;
using VtmFramework.Mapper;

namespace ViennaTrafficMonitor.Mapper {

    public class SteigMapper : ISteigMapper {

        private readonly ConcurrentDictionary<int, ISteig> _data;

        public SteigMapper(ConcurrentDictionary<int, ISteig> data) {
            this._data = data;
        }

        public ISteig Find(int id) {
            return _data[id];
        }

        public IList<ISteig> FindByHaltestelle(int haltestellenId) {
            var query = from steig in _data.Values
                        where steig.HaltestellenId.Equals(haltestellenId)
                        select steig;
            return query.ToList();
        }

        public IList<ISteig> FindByRbl(int rbl) {
            var query = from steig in _data.Values
                        where steig.Rbl.Equals(rbl)
                        select steig;
            return query.ToList();
        }

        public IList<ISteig> FindByLinie(int linienId) {
            var query = from steig in _data.Values
                        where steig.LinienId.Equals(linienId)
                        orderby steig.Reihenfolge ascending
                        select steig;
            return query.ToList();
        }

        public ConcurrentDictionary<int, ISteig> All {
            get { return _data; }
        }

    }

}
