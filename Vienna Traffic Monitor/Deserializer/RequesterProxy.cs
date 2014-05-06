using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViennaTrafficMonitor.Deserializer {
    public static class RblRequesterProxy {

        public static async Task<IList<VtmResponse>> GetProxyResponseAsync(int rbl) {            
            Response response = await RblRequester.GetResponseAsync(rbl);
            return _ModifyResponse(response);
        }

        public static async Task<IList<VtmResponse>> GetProxyResponsesAsync(IEnumerable<int> rblEnumerbale) {
            Response response = await RblRequester.GetResponseAsync(rblEnumerbale);
            return _ModifyResponse(response);
            
        }

        private static IList<VtmResponse> _ModifyResponse(Response response) {
            IList<VtmResponse> responses = new List<VtmResponse>();
            foreach (Monitor monitor in response.Data.Monitors) {
                foreach (Line line in monitor.Lines) {
                    foreach (Departure departure in line.Departures.Departure) {
                        responses.Add(new VtmResponse(line, departure, monitor.LocationStop, response.Data.TrafficInfoCategories, response.Data.TrafficInfoCategoryGroups));
                    }
                }
            }
            return responses;
        }
    }
}
