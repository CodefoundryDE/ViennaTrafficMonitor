using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViennaTrafficMonitor.Deserializer {
    public static class RblRequesterProxy {

        public static async Task<IList<VtmResponse>> GetProxyResponseAsync(int rbl) {
            
            Response response = await RblRequester.GetResponseAsync(rbl);
            IList<VtmResponse> responses = new List<VtmResponse>();
            foreach (Monitor monitor in response.Data.Monitors) {
                foreach (Line line in monitor.Lines) {
                    foreach (Departure departure in line.Departures.Departure) {
                        responses.Add(new VtmResponse(line, departure, response.Message, monitor.LocationStop));
                    }
                }
            }
            return responses;
        }

    }
}
