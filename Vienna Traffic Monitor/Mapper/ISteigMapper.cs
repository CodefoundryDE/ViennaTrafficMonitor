using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ViennaTrafficMonitor.Model;
using VtmFramework.Mapper;
namespace ViennaTrafficMonitor.Mapper {
    public interface ISteigMapper : IMapper<ISteig> {

        IList<ISteig> FindByHaltestelle(int haltestellenId);
        IList<ISteig> FindByLinie(int linienId);
        IList<ISteig> FindByRbl(int rbl);

        /// <summary>
        /// Gibt alle Steige zurück
        /// </summary>
        /// <returns></returns>
        ConcurrentDictionary<int, ISteig> All { get; }
    }
}
