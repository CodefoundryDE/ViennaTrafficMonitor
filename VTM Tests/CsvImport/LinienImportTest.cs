using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.CsvImport.Parser;
using ViennaTrafficMonitor.Model;

namespace VtmTests.CsvImport {

    [TestClass]
    public class LinienImportTest {
        private String testPath = @"..\..\CsvImport\TestFiles\DemoLinien.csv";
        
        [TestInitialize]

        [TestMethod]

        public void TestLinienImport() {
            Collection<ILinie> linien =  (Collection<ILinie>) LinienParser.ReadFile(testPath);

            Assert.AreEqual(214433717, linien[0].Id);
            Assert.AreEqual("D", linien[0].Bezeichnung);
            Assert.AreEqual(10, linien[0].Reihenfolge);
            Assert.AreEqual(true, linien[0].Echtzeit);
            Assert.AreEqual(EVerkehrsmittel.Tram, linien[0].Verkehrsmittel);

            Assert.AreEqual(214433953, linien[1].Id);
            Assert.AreEqual("N20", linien[1].Bezeichnung);
            Assert.AreEqual(320, linien[1].Reihenfolge);
            Assert.AreEqual(true, linien[1].Echtzeit);
            Assert.AreEqual(EVerkehrsmittel.NachtBus, linien[1].Verkehrsmittel);

            Assert.AreEqual(214432069, linien[2].Id);
            Assert.AreEqual("S2", linien[2].Bezeichnung);
            Assert.AreEqual(401, linien[2].Reihenfolge);
            Assert.AreEqual(false, linien[2].Echtzeit);
            Assert.AreEqual(EVerkehrsmittel.SBahn, linien[2].Verkehrsmittel);

            Assert.AreEqual(214433691, linien[3].Id);
            Assert.AreEqual("U3", linien[3].Bezeichnung);
            Assert.AreEqual(3, linien[3].Reihenfolge);
            Assert.AreEqual(true, linien[3].Echtzeit);
            Assert.AreEqual(EVerkehrsmittel.Metro, linien[3].Verkehrsmittel);

            Assert.AreEqual(214433055, linien[4].Id);
            Assert.AreEqual("WLB", linien[4].Bezeichnung);
            Assert.AreEqual(98, linien[4].Reihenfolge);
            Assert.AreEqual(true, linien[4].Echtzeit);
            Assert.AreEqual(EVerkehrsmittel.TramWlb, linien[4].Verkehrsmittel);

            Assert.AreEqual(214433817, linien[5].Id);
            Assert.AreEqual("13A", linien[5].Bezeichnung);
            Assert.AreEqual(115, linien[5].Reihenfolge);
            Assert.AreEqual(true, linien[5].Echtzeit);
            Assert.AreEqual(EVerkehrsmittel.CityBus, linien[5].Verkehrsmittel);
 
        }
    }
}
