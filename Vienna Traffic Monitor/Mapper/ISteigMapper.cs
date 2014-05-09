using System;
using System.Collections.Generic;
using ViennaTrafficMonitor.Model;
namespace ViennaTrafficMonitor.Mapper {
    public interface ISteigMapper {
        ISteig Find(int id);
        List<ISteig> FindByHaltestelle(int haltestellenId);
        List<ISteig> FindByLinie(int linienId);
        List<ISteig> FindByRbl(int rbl);
    }
}
