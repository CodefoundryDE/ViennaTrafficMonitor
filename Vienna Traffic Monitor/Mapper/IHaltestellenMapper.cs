using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Model;
using VtmFramework.Mapper;
using VtmFramework.Library;
using System.Windows;
using System.Collections.Concurrent;

namespace ViennaTrafficMonitor.Mapper {

    public interface IHaltestellenMapper : IMapper<IHaltestelle> {

        List<IHaltestelle> FindByName(String name);

        List<IHaltestelle> FindByRectangle(Rectangle rect);

        /// <summary>
        /// Gibt alle Koordinaten der Haltestellen als Dictionary<int, Point> aus, 
        /// wobei der Schlüssel die Haltestellen-Id repräsentiert.
        /// </summary>
        /// <returns></returns>
        IDictionary<int, Point> AllCoordinates { get; }

        /// <summary>
        /// Gibt alle Haltestellen zurück.
        /// </summary>
        /// <returns></returns>
        ConcurrentDictionary<int, IHaltestelle> All { get; }
    }

}
