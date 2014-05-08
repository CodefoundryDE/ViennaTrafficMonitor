using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ViennaTrafficMonitor.Model;
namespace ViennaTrafficMonitor.Mapper {
    public interface ISteigMapper {
        ISteig Find(int id);
        List<ISteig> FindByHaltestelle(int HaltestellenId);
        List<ISteig> FindByLinie(int LinienId);
        List<ISteig> FindByRbl(int Rbl);
        ConcurrentDictionary<int, ISteig> GetAll();
    }
}
