﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViennaTrafficMonitor.Model;
using VtmFramework.Library;

namespace ViennaTrafficMonitor.Mapper {

    public class HaltestellenMapper : IHaltestellenMapper {

        private readonly ConcurrentDictionary<int, IHaltestelle> _data;

        public HaltestellenMapper(ConcurrentDictionary<int, IHaltestelle> data, ILinienMapper linienMappper) {
            var query = from haltestelle in data
                        let linien = linienMappper.FindByHaltestelle(haltestelle.Value.Id)
                        where !((linien.Count == 1) && (linien.First().Verkehrsmittel == EVerkehrsmittel.SBahn))
                        select haltestelle;

            this._data = new ConcurrentDictionary<int, IHaltestelle>(query);
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
        public ICollection<IHaltestelle> FindByName(String name) {
            if (String.IsNullOrWhiteSpace(name)) {
                return new List<IHaltestelle>();
            }
            // Damit die Suche etwas intuitiver wird, werden die Ergebnisse, 
            // bei denen der Suchtext im Wort vorne steht, bevorzugt.
            var firstMatches = from haltestelle in _data.Values
                               where haltestelle.Name.StartsWith(name.Trim(), true, CultureInfo.CurrentCulture)
                               orderby haltestelle.Name.Length
                               select haltestelle;
            var allMatches = from haltestelle in _data.Values
                             where haltestelle.Name.ToLower().Contains(name.Trim().ToLower())
                             orderby haltestelle.Name.Length
                             select haltestelle;
            return firstMatches.Union(allMatches).ToList();
        }

        public IDictionary<int, Point> AllCoordinates {
            get { return _getAllCoordinates(); }
        }
        private IDictionary<int, Point> _getAllCoordinates() {
            var dict = _data.Select(t => new { t.Key, t.Value.Location })
                .ToDictionary(t => t.Key, t => t.Location);
            return dict;
        }

        public ICollection<IHaltestelle> GetByNameLength(int length) {
            var query = from haltestelle in _data.Values
                        where haltestelle.Name.Length == length
                        select haltestelle;
            return query.ToList();
        }

        public ICollection<IHaltestelle> All {
            get { return _data.Values.ToList(); }
        }

    }

}
