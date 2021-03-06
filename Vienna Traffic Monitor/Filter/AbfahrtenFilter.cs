﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Deserializer;
using ViennaTrafficMonitor.Model;

namespace ViennaTrafficMonitor.Filter {

    public class AbfahrtenFilter : GenericFilter<VtmResponse> {

        public AbfahrtenFilter(EVerkehrsmittel verkehrsmittel)
            : this(verkehrsmittel, true) {
        }

        public AbfahrtenFilter(EVerkehrsmittel verkehrsmittel, bool active)
            : base(active) {

            Filter = (ICollection<VtmResponse> abfahrten) => {
                if (abfahrten == null) {
                    return new List<VtmResponse>();
                }
                var query = from response in abfahrten
                            where response.Typ != verkehrsmittel
                            select response;
                return query.ToList<VtmResponse>();
            };
        }

    }

}
