using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Model;
using VtmFramework.Libary;

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
        public List<IHaltestelle> findByName(String name) {

            return (from date in _data
                    where date.Value.Name.Contains(name)
                    select date.Value).ToList();
        }

        /// <summary>
        /// Findet alle Haltestellen in dem übergebenen Rechteck 
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public List<IHaltestelle> findByRectangle(Rectangle rect) {
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

    }

}
