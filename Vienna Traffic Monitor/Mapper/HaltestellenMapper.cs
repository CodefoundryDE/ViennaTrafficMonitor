using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViennaTrafficMonitor.Model;
using VtmFramework.Library;

namespace ViennaTrafficMonitor.Mapper {

    public class HaltestellenMapper : IHaltestellenMapper {

        private ConcurrentDictionary<int, IHaltestelle> _data;

        public HaltestellenMapper(ConcurrentDictionary<int, IHaltestelle> data) {
            this._data = data;
        }

        /// <summary>
        /// Findet die Haltestelle mit der übergebenen ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IHaltestelle Find(int id) {
            return _data[id];
        }

        /// <summary>
        /// Findet alle Haltestellen, in deren Name der übergebene String vorkommt
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<IHaltestelle> FindByName(String name) {
            if (String.IsNullOrWhiteSpace(name)) {
                return new List<IHaltestelle>();
            }
            return (from date in _data
                    where date.Value.Name.ToLower().Contains(name.Trim().ToLower())
                    select date.Value).ToList();
        }

        /// <summary>
        /// Findet alle Haltestellen in dem übergebenen Rechteck 
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public List<IHaltestelle> FindByRectangle(Rectangle rect) {
            if (rect != null) {
                double minX = rect.BottomLeft.X;
                double minY = rect.BottomLeft.Y;
                double maxX = rect.TopRight.X;
                double maxY = rect.TopRight.Y;


                return (from date in _data
                        where date.Value.Location.X >= minX
                        && date.Value.Location.X <= maxX
                        && date.Value.Location.Y >= minY
                        && date.Value.Location.Y <= maxY
                        select date.Value).ToList();
            }
            return null;
        }

        public IDictionary<int, Point> AllCoordinates {
            get { return _getAllCoordinates(); }
        }
        private IDictionary<int, Point> _getAllCoordinates() {
            var dict = _data.Select(t => new { t.Key, t.Value.Location })
                .ToDictionary(t => t.Key, t => t.Location);
            return dict;
        }

        public ConcurrentDictionary<int, IHaltestelle> All {
            get { return _data; }
        }

    }

}
