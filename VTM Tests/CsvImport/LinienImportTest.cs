using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.CsvImport.Parser;
using ViennaTrafficMonitor.Model;

namespace VtmTests.CsvImport {

    [TestClass]
    public class LinienImportTest {
        private String testPathLinie = @"..\..\CsvImport\TestFiles\DemoLinien.csv";

        [TestInitialize]

        [TestMethod]

        public void TestLinienImport() {
            ConcurrentDictionary<int, ILinie> linien = LinienParser.ReadFile(testPathLinie);

            ILinie testLinie;
            if (linien.TryGetValue(214433717, out testLinie)) {
                Assert.AreEqual(214433717, testLinie.Id);
                Assert.AreEqual("D", testLinie.Bezeichnung);
                Assert.AreEqual(10, testLinie.Reihenfolge);
                Assert.AreEqual(true, testLinie.Echtzeit);
                Assert.AreEqual(EVerkehrsmittel.Tram, testLinie.Verkehrsmittel);
            } else {
                Assert.Fail();
            }

            if (linien.TryGetValue(214433953, out testLinie)) {
                Assert.AreEqual(214433953, testLinie.Id);
                Assert.AreEqual("N20", testLinie.Bezeichnung);
                Assert.AreEqual(320, testLinie.Reihenfolge);
                Assert.AreEqual(true, testLinie.Echtzeit);
                Assert.AreEqual(EVerkehrsmittel.NachtBus, testLinie.Verkehrsmittel);
            } else {
                Assert.Fail();
            }

            if (linien.TryGetValue(214432069, out testLinie)) {
                Assert.AreEqual(214432069, testLinie.Id);
                Assert.AreEqual("S2", testLinie.Bezeichnung);
                Assert.AreEqual(401, testLinie.Reihenfolge);
                Assert.AreEqual(false, testLinie.Echtzeit);
                Assert.AreEqual(EVerkehrsmittel.SBahn, testLinie.Verkehrsmittel);
            } else {
                Assert.Fail();
            }

            if (linien.TryGetValue(214433691, out testLinie)) {
                Assert.AreEqual(214433691, testLinie.Id);
                Assert.AreEqual("U3", testLinie.Bezeichnung);
                Assert.AreEqual(3, testLinie.Reihenfolge);
                Assert.AreEqual(true, testLinie.Echtzeit);
                Assert.AreEqual(EVerkehrsmittel.Metro, testLinie.Verkehrsmittel);
            } else {
                Assert.Fail();
            }

            if (linien.TryGetValue(214433055, out testLinie)) {
                Assert.AreEqual(214433055, testLinie.Id);
                Assert.AreEqual("VRT", testLinie.Bezeichnung);
                Assert.AreEqual(98, testLinie.Reihenfolge);
                Assert.AreEqual(true, testLinie.Echtzeit);
                Assert.AreEqual(EVerkehrsmittel.TramVrt, testLinie.Verkehrsmittel);
            } else {
                Assert.Fail();
            }

            if (linien.TryGetValue(214433817, out testLinie)) {
                Assert.AreEqual(214433817, testLinie.Id);
                Assert.AreEqual("13A", testLinie.Bezeichnung);
                Assert.AreEqual(115, testLinie.Reihenfolge);
                Assert.AreEqual(true, testLinie.Echtzeit);
                Assert.AreEqual(EVerkehrsmittel.CityBus, testLinie.Verkehrsmittel);
            } else {
                Assert.Fail();
            }

        }
    }
}
