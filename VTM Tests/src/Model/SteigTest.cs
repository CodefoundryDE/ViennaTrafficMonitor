using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViennaTrafficMonitor.Model;
using System.Windows;

namespace VtmTests.Model {

    [TestClass]
    public class SteigTest {

        private Point point;
        private Steig steig;
        private Steig steigLeer;

        [TestInitialize]
        public void TestInitialize() {
            point = new Point(123.4, 432.1);
            steig = new Steig(1, 123, 123, ERichtung.Rueck, 23, 2356, 23, "U1-23", point);
            steigLeer = new Steig();
        }

        [TestMethod]
        public void TestSteig() {
            Assert.AreEqual(steigLeer.Id, 0);
            Assert.AreEqual(steigLeer.LinienId, 0);
            Assert.AreEqual(steigLeer.HaltestellenId, 0);
            Assert.AreEqual(steigLeer.Richtung, ERichtung.Hin);
            Assert.AreEqual(steigLeer.Reihenfolge, 0);
            Assert.AreEqual(steigLeer.Rbl, 0);
            Assert.AreEqual(steigLeer.Bereich, 0);
            Assert.AreEqual(steigLeer.Name, "");
            Assert.AreEqual(steigLeer.Location, new Point());

            Assert.AreEqual(steig.Id, 1);
            Assert.AreEqual(steig.LinienId, 123);
            Assert.AreEqual(steig.HaltestellenId, 123);
            Assert.AreEqual(steig.Richtung, ERichtung.Rueck);
            Assert.AreEqual(steig.Reihenfolge, 23);
            Assert.AreEqual(steig.Rbl, 2356);
            Assert.AreEqual(steig.Bereich, 23);
            Assert.AreEqual(steig.Name, "U1-23");
            Assert.AreEqual(steig.Location, point);
        }

    }
}
