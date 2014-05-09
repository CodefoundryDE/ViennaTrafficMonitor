using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Model;

namespace ViennaTrafficMonitor.Mapper {

    public class LinienMapper : ILinienMapper {

        private ConcurrentDictionary<int, ILinie> _data;

        public LinienMapper(ConcurrentDictionary<int, ILinie> data) {
            this._data = data;
        }

        public ILinie Find(int id) {
            return _data[id];
        }

        public IList<ILinie> FindByBezeichnung(string bezeichnung) {
            var query = from linie in _data.Values
                        where linie.Bezeichnung.Contains(bezeichnung)
                        select linie;
            return new List<ILinie>(query);
        }

        public Dictionary<ILinie, List<IHaltestelle>> GetHaltestellenOrdered() {
            ConcurrentDictionary<int, ISteig> steige = SteigMapperFactory.Instance.GetAll();
            ConcurrentDictionary<int, IHaltestelle> haltestellen = HaltestellenMapperFactory.Instance.GetAll();

            var query = (from steig in steige.Values 
                         where steig.Richtung == ERichtung.Hin
                         orderby steig.Reihenfolge
                         join linie in _data.Values on steig.LinienId equals linie.Id
                         join haltestelle in haltestellen.Values on steig.HaltestellenId equals haltestelle.Id
                         select new { linie, haltestelle })
            .GroupBy(x => x.linie)
            .ToDictionary(x => x.Key, x => x.Select(o => o.haltestelle).ToList());
            return query;
        }
    }

}
