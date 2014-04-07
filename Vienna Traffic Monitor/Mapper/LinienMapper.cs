﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Model;

namespace ViennaTrafficMonitor.Mapper {

    public class LinienMapper : ILinienMapper {

        private ConcurrentDictionary<int, ILinie> _data;

        public LinienMapper(ConcurrentDictionary<int, ILinie> data) {
            this._data = data;
        }

        public ILinie Find(int id) {
            return _data[id];
        }

    }

}
