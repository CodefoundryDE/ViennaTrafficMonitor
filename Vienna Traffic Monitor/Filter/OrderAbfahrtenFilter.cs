using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Deserializer;
using ViennaTrafficMonitor.Model;

namespace ViennaTrafficMonitor.Filter {
    public enum EAbfahrtenOrder { TimeRealAsc, TimeRealDesc, TimePlannedAsc, TimePlannedDesc };
    public class OrderAbfahrtenFilter : GenericFilter<VtmResponse> {

        private int _count;
        public OrderAbfahrtenFilter()
            : this(EAbfahrtenOrder.TimeRealAsc, 10, true) {
        }
        public OrderAbfahrtenFilter(EAbfahrtenOrder order, int resultCount, bool active)
            : base(active) {
            _count = resultCount;
            #region FilterInit
            switch (order) {
                case EAbfahrtenOrder.TimeRealAsc: {
                        Filter = (ICollection<VtmResponse> abfahrten) => {
                            if (abfahrten == null) {
                                return new List<VtmResponse>();
                            }
                            var query = from response in abfahrten
                                        orderby response.Departure.DepartureTime.TimeReal
                                        select response;
                            var result = query.ToList<VtmResponse>().Take(_count);
                            return result.ToList();
                        };
                        break;
                    }
                case EAbfahrtenOrder.TimeRealDesc: {
                    Filter = (ICollection<VtmResponse> abfahrten) => {
                        if (abfahrten == null) {
                            return new List<VtmResponse>();
                        }
                        var query = from response in abfahrten
                                    orderby response.Departure.DepartureTime.TimeReal descending
                                    select response;
                        var result = query.ToList<VtmResponse>().Take(_count);
                        return result.ToList();
                    };
                    break;
                    }
                case EAbfahrtenOrder.TimePlannedAsc: {
                        Filter = (ICollection<VtmResponse> abfahrten) => {
                            if (abfahrten == null) {
                                return new List<VtmResponse>();
                            }
                            var query = from response in abfahrten
                                        orderby response.Departure.DepartureTime.TimePlanned
                                        select response;
                            var result = query.ToList<VtmResponse>().Take(_count);
                            return result.ToList();
                        };
                        break;
                    }
                case EAbfahrtenOrder.TimePlannedDesc: {
                        Filter = (ICollection<VtmResponse> abfahrten) => {
                            if (abfahrten == null) {
                                return new List<VtmResponse>();
                            }
                            var query = from response in abfahrten
                                        orderby response.Departure.DepartureTime.TimePlanned descending
                                        select response;
                            var result = query.ToList<VtmResponse>().Take(_count);
                            return result.ToList();
                        };
                        break;
                    }
            };
            #endregion
        }
    }
}
