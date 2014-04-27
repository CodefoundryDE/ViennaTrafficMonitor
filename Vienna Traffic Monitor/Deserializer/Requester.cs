using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Script.Serialization;

namespace ViennaTrafficMonitor.Deserializer {
    public class RblRequester {

        private const string _rblAllocator = "rbl=";
        private const string _rblConnector = "&rbl=";

        /// <summary>
        /// Erzeugt einen request string für EINE rbl, führt den request an die Wiener Linien aus
        /// und parst das Ergebnis in eine Instanz der Klasse Response
        /// </summary>
        /// <param name="rbl"></param>
        /// <returns></returns>
        public static async Task<Response> getResponseAsync(int rbl) {

            string requestString =
                ViennaTrafficMonitor.Properties.Settings.Default.MonitorRequestBegin
                + _rblAllocator
                + rbl.ToString()
                + ViennaTrafficMonitor.Properties.Settings.Default.MonitorRequestEnd
                + ViennaTrafficMonitor.Properties.Settings.Default.SenderIdDev;

            Task<string> request = new HttpClient().GetStringAsync(requestString);

            JavaScriptSerializer deserializer = new JavaScriptSerializer();

            Response response = deserializer.Deserialize<Response>(await request);

            return response;


        }


        /// <summary>
        /// Erzeugt einen request string für ein IEnumerable rbl, führt den request an die Wiener 
        /// Linien aus und parst das Ergebnis in eine Instanz der Klasse Response
        /// </summary>
        /// <param name="rblEnumerable"></param>
        /// <returns></returns>
        public static async Task<Response> getResponseAsync(IEnumerable<int> rblEnumerable) {


            StringBuilder builder = new StringBuilder();

            builder.Append(ViennaTrafficMonitor.Properties.Settings.Default.MonitorRequestBegin);

            builder.Append(_rblAllocator);
            builder.Append(rblEnumerable.First());
            rblEnumerable = rblEnumerable.Skip(1);
            
            foreach (int rbl in rblEnumerable) {
                builder.Append(_rblConnector);
                builder.Append(rbl);
            }

            builder.Append(ViennaTrafficMonitor.Properties.Settings.Default.MonitorRequestEnd);
            builder.Append(ViennaTrafficMonitor.Properties.Settings.Default.SenderIdDev);

            string requestString = builder.ToString();
                 


            Task<string> request = new HttpClient().GetStringAsync(requestString);

            JavaScriptSerializer deserializer = new JavaScriptSerializer();

            String responseString = await request;

            Response response = deserializer.Deserialize<Response>(responseString);

            return response;
        }


    }
}
