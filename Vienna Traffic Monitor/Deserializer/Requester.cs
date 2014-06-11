using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Collections;

namespace ViennaTrafficMonitor.Deserializer {
    public class RblRequester : IRequester {

        private const string _rblAllocator = "rbl=";
        private const string _rblConnector = "&rbl=";

        public async Task<Response> GetResponseAsync(int rbl) {
            return await GetResponseAsync(new SortedSet<int> { rbl });
        }

        /// <summary>
        /// Erzeugt einen request string für ein IEnumerable rbl, führt den request an die Wiener 
        /// Linien aus und parst das Ergebnis in eine Instanz der Klasse Response
        /// </summary>
        public async Task<Response> GetResponseAsync(ISet<int> rblEnumerable) {
            string requestString = buildRequestString(rblEnumerable);
            string responded = await requestAndAwaitResponse(requestString);
            Response response = deserializeRespondedString(responded);
            return response;
        }

        private static string buildRequestString(ISet<int> setOfRbls) {
            if (setOfRbls.Count <= 0) {
                throw new ArgumentOutOfRangeException("setOfRbls", setOfRbls.Count, "Leeres Set von Rbls an den Requester übergeben");
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(ViennaTrafficMonitor.Properties.Settings.Default.MonitorRequestBegin);
            builder.Append(_rblAllocator);
            builder.Append(setOfRbls.First());
            IEnumerable rblEnumerable = setOfRbls.Skip(1);

            foreach (int rbl in rblEnumerable) {
                builder.Append(_rblConnector);
                builder.Append(rbl);
            }
            builder.Append(ViennaTrafficMonitor.Properties.Settings.Default.MonitorRequestEnd);
            builder.Append(ViennaTrafficMonitor.Properties.Settings.Default.SenderIdDev);
            return builder.ToString();
        }

        private async Task<string> requestAndAwaitResponse(string requestString) {
            Task<string> request = new HttpClient().GetStringAsync(requestString);
            return await request;
        }

        private static Response deserializeRespondedString(string responded) {
            JavaScriptSerializer deserializer = new JavaScriptSerializer();
            return deserializer.Deserialize<Response>(responded);
        }
    }
}
