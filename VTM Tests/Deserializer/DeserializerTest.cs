using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViennaTrafficMonitor.Deserializer;
using System.IO;

namespace VtmTests.Deserializer {

    [TestClass]
    public class DeserializerTest {

        private String data;
        private JavaScriptSerializer deserializer;
        private Response response;
        private String filePath = @"..\..\Deserializer\TestFiles\DemoData.txt";

        Line firstLine;
        DepartureTime departureTime1;
        DepartureTime departureTime2;

        [TestInitialize]
        public void TestInitialize() {

            try {
                StreamReader testFileReader = new StreamReader(filePath, System.Text.Encoding.Default);
                data = testFileReader.ReadToEnd();
                testFileReader.Close();
            } catch (Exception) {
                //TODO
            }

            deserializer = new JavaScriptSerializer();

            response = deserializer.Deserialize<Response>(data);

            List<Monitor> monitors = response.Data.Monitors;
            Monitor firstMonitor = monitors.First();
            List<Line> lines = firstMonitor.Lines;
            firstLine = lines.First();
            Departures departures = firstLine.Departures;
            List<Departure> departure = departures.Departure;
            Departure firstDeparture = departure.First();
            Departure secondDeparture = departure.ElementAt(1);
            departureTime1 = firstDeparture.DepartureTime;
            departureTime2 = secondDeparture.DepartureTime;
        }

        [TestMethod]
        public void TestLine() {

            Assert.AreEqual(firstLine.Name, "N49");
            Assert.AreEqual(firstLine.Towards, "Hütteldorf");
            Assert.AreEqual(firstLine.Direction, "H");
            Assert.AreEqual(firstLine.RichtungsId, "1");
            Assert.AreEqual(firstLine.BarrierFree, true);
            Assert.AreEqual(firstLine.RealtimeSupported, true);
            Assert.AreEqual(firstLine.Trafficjam, false);

        }

        [TestMethod]
        public void TestDeparture() {

            Assert.AreEqual(departureTime1.TimePlanned, "2013-08-07T02:51:00.000+0200");
            Assert.AreEqual(departureTime1.TimeReal, "2013-08-07T02:51:30.000+0200");
            Assert.AreEqual(departureTime1.Countdown, 27);
            Assert.AreEqual(departureTime2.TimePlanned, "2013-08-07T03:21:00.000+0200");
            Assert.AreEqual(departureTime2.TimeReal, "2013-08-07T03:21:30.000+0200");
            Assert.AreEqual(departureTime2.Countdown, 57);
        }
    }
}
