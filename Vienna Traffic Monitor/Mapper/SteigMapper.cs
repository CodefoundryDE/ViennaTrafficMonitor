﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Model;
using VtmFramework.Mapper;

namespace ViennaTrafficMonitor.Mapper {

    public class SteigMapper : ISteigMapper {

        private ConcurrentDictionary<int, ISteig> _data;

        public SteigMapper(ConcurrentDictionary<int, ISteig> data) {
            this._data = data;
        }

        public ISteig Find(int id) {
            return _data[id];
        }

        public List<ISteig> FindByHaltestelle(int HaltestellenId) {
            var query = from steig in _data.Values
                        where steig.HaltestellenId.Equals(HaltestellenId)
                        select steig;
            return new List<ISteig>(query);
        }

        public List<ISteig> FindByRbl(int Rbl) {
            var query = from steig in _data.Values
                        where steig.Rbl.Equals(Rbl)
                        select steig;
            return new List<ISteig>(query);
        }

        public List<ISteig> FindByLinie(int LinienId) {
            var query = from steig in _data.Values
                        where steig.LinienId.Equals(LinienId)
                        orderby steig.Reihenfolge ascending
                        select steig;       
            //SortedList<int, ISteig> linieRhf = new SortedList<int, ISteig>();
            //foreach (ISteig steig in query) {
            //    linieRhf.Add(steig.Reihenfolge, steig);
            //}
            //return linieRhf;
            return new List<ISteig>(query);
            
        }

        /// <summary>
        /// Gibt alle Steige zurück
        /// </summary>
        /// <returns></returns>
        public ConcurrentDictionary<int, ISteig> GetAll() {
            return _data;
        }
    }

}
