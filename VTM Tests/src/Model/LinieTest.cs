using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViennaTrafficMonitor.Model;

namespace VtmTests.Model {

    [TestClass]
    public class LinieTest {

        private Linie linie;
        private Linie linieLeer;

        [TestInitialize]
        public void TestInitialize() {
            linie = new Linie(1, "Testlinie", 12, true, EVerkehrsmittel.SBahn);
            linieLeer = new Linie();
        }

        [TestMethod]
        public void TestLinie() {
            Assert.AreEqual(linie.Id, 1);
            Assert.AreEqual(linie.Bezeichnung, "Testlinie");
            Assert.AreEqual(linie.Reihenfolge, 12);
            Assert.AreEqual(linie.Echtzeit, true);
            Assert.AreEqual(linie.Verkehrsmittel, EVerkehrsmittel.SBahn);

            Assert.AreEqual(linieLeer.Id, 0);
            Assert.AreEqual(linieLeer.Bezeichnung, "");
            Assert.AreEqual(linieLeer.Reihenfolge, 0);
            Assert.AreEqual(linieLeer.Echtzeit, false);
            Assert.AreEqual(linieLeer.Verkehrsmittel, EVerkehrsmittel.Metro);
        }
    }
}
