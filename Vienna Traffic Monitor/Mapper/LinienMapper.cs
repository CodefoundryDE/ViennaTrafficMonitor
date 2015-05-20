using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Model;

namespace ViennaTrafficMonitor.Mapper {

    public class LinienMapper : ILinienMapper {

        private readonly ConcurrentDictionary<int, ILinie> _data;

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
            return query.ToList();
        }



        public IDictionary<ILinie, List<IHaltestelle>> HaltestellenOrdered {
            get { return _getHaltestellenOrdered(); }
        }
        private IDictionary<ILinie, List<IHaltestelle>> _getHaltestellenOrdered() {
            ConcurrentDictionary<int, ISteig> steige = SteigMapperFactory.Instance.All;
            ICollection<IHaltestelle> haltestellen = HaltestellenMapperFactory.Instance.All;

            var query = (from steig in steige.Values
                         where steig.Richtung == ERichtung.Hin
                         orderby steig.Reihenfolge
                         join linie in _data.Values on steig.LinienId equals linie.Id
                         join haltestelle in haltestellen on steig.HaltestellenId equals haltestelle.Id
                         select new { linie, haltestelle })
            .GroupBy(x => x.linie)
            .ToDictionary(x => x.Key, x => x.Select(o => o.haltestelle).ToList());
            return query;
        }


        public ISet<ILinie> FindByHaltestelle(int haltestellenId) {
            ISteigMapper sm = SteigMapperFactory.Instance;
            var query = sm.FindByHaltestelle(haltestellenId);
            var idSet = (from steig in query
                    select steig.LinienId).Distinct();
            HashSet<ILinie> linienSet = new HashSet<ILinie>();
            foreach (int i in idSet) {
                linienSet.Add(Find(i));
            }
            return linienSet;
        }
    }

}
