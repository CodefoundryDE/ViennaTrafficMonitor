using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViennaTrafficMonitor.Deserializer;

namespace VtmTests.Deserializer {

    [TestClass]
    public class RequesterTest {

        private Response _Response;
        private Response _ResponseEnumerable;
        private int _TestRbl;
        private IEnumerable<int> _TestRblEnumerable;

        [TestInitialize]
        public void TestInitialize() {

            _TestRbl = 147;
            _TestRblEnumerable = new int[] { 7, 8, 9 };



        }

        [TestMethod]
        public async Task TestRequest() {

            Task<Response> request = RblRequester.GetResponseAsync(_TestRbl);
            Task<Response> requestEnumerable = RblRequester.GetResponseAsync(_TestRblEnumerable);

            _Response = await request;
            _ResponseEnumerable = await requestEnumerable;


            List<Monitor> monitors = _Response.Data.Monitors;
            Monitor firstMonitor = monitors.First();
            List<Line> lines = firstMonitor.Lines;
            Line firstLine = lines.First();

            Departures departures = firstLine.Departures;
            List<Departure> departure = departures.Departure;
            Departure firstDeparture = departure.First();
            Departure secondDeparture = departure.ElementAt(1);
            DepartureTime departureTime1 = firstDeparture.DepartureTime;
            DepartureTime departureTime2 = secondDeparture.DepartureTime;

            Console.WriteLine(firstLine);
            Console.WriteLine(firstDeparture);
            Console.WriteLine(departureTime1);
            Console.WriteLine(secondDeparture);
            Console.WriteLine(departureTime2);


        }




    }
}
