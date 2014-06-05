using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Model;
using VtmFramework.Mapper;

namespace ViennaTrafficMonitor.Mapper {

    public interface ILinienMapper : IMapper<ILinie> {

        /// <summary>
        /// Sucht Linien welche eine bestimmte Bezeichnung enthalten.
        /// </summary>
        /// <param name="bezeichnung"></param>
        /// <returns>Liste von Linien</returns>
        IList<ILinie> FindByBezeichnung(string bezeichnung);

        /// <summary>
        /// Gibt alle Steige nach Linien geordnet aus.
        /// </summary>
        /// <returns></returns>
        IDictionary<ILinie, List<IHaltestelle>> HaltestellenOrdered { get; }

        /// <summary>
        /// Gibt alle Linien welche eine bestimmte Haltestelle bedienen zurück
        /// </summary>
        ISet<ILinie> FindByHaltestelle(int haltestellenId);
    }

}
