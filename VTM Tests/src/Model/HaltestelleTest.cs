using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViennaTrafficMonitor.Model;

namespace VtmTests.Model {

    [TestClass]
    public class HaltestelleTest {

        private Point point;
        private Haltestelle halt, haltLeer;

        [TestInitialize]
        public void TestInitialize() {
            point = new Point(123.4, 432.1);
            halt = new Haltestelle(1, 1234, "Test-Haltestelle", point);
            haltLeer = new Haltestelle();
        }

        [TestMethod]
        public void TestHaltestelle() {

            Assert.AreEqual(haltLeer.Id, 0);
            Assert.AreEqual(haltLeer.Diva, 0);
            Assert.AreEqual(haltLeer.Name, "");
            Assert.AreEqual(haltLeer.Location, new Point());

            Assert.AreEqual(halt.Id, 1);
            Assert.AreEqual(halt.Diva, 1234);
            Assert.AreEqual(halt.Name, "Test-Haltestelle");
            Assert.AreEqual(halt.Location, point);
        }

    }
}
