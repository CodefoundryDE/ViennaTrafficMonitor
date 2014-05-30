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

        List<IHaltestelle> FindByRectangle(VtmRectangle rect);

        /// <summary>
        /// Gibt alle Koordinaten der Haltestellen als Dictionary<int, Point> aus, 
        /// wobei der Schlüssel die Haltestellen-Id repräsentiert.
        /// </summary>
        /// <returns></returns>
        IDictionary<int, Point> AllCoordinates { get; }

        /// <summary>
        /// Gibt alle Haltestellen zurück, deren Name eine gewisse Länge hat.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        ICollection<IHaltestelle> GetByNameLength(int length);

        /// <summary>
        /// Gibt alle Haltestellen zurück.
        /// </summary>
        /// <returns></returns>
        ICollection<IHaltestelle> All { get; }
    }

}
