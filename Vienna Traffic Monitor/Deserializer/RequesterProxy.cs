using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViennaTrafficMonitor.Deserializer {
    public static class RblRequesterProxy {

        public static async Task<IList<VtmResponse>> GetProxyResponseAsync(int rbl) {
            IRequester requester = RequesterFactory.GetInstance();
            Response response = await requester.GetResponseAsync(rbl);
            return _ModifyResponse(response);
        }

        public static async Task<IList<VtmResponse>> GetProxyResponseAsync(ISet<int> rblSet) {
            IRequester requester = RequesterFactory.GetInstance();
            Response response = await requester.GetResponseAsync(rblSet);
            return _ModifyResponse(response);
            
        }

        private static IList<VtmResponse> _ModifyResponse(Response response) {
            IList<VtmResponse> responses = new List<VtmResponse>();
            foreach (Monitor monitor in response.Data.Monitors) {
                foreach (Line line in monitor.Lines) {
                    foreach (Departure departure in line.Departures.Departure) {
                        responses.Add(new VtmResponse(line, departure, monitor.LocationStop, response.Data.TrafficInfoCategories, response.Data.TrafficInfoCategoryGroups, line.Type));
                    }
                }
            }
            return responses;
        }
    }
}
