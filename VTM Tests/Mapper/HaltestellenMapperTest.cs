using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViennaTrafficMonitor.Mapper;
using ViennaTrafficMonitor.Model;
using System.Collections.Generic;
using System.Windows;
using System.Collections.Concurrent;
using VtmFramework.Library;

namespace VtmTests.Mapper {

    [TestClass]
    public class HaltestellenMapperTest {

        private ConcurrentDictionary<int, IHaltestelle> _data;
        private IHaltestellenMapper _mapperFictional;
        private IHaltestellenMapper _mapperReal;
        private IHaltestelle hauptbahnhof;
        private IHaltestelle hauptbahnhofOst;
        private IHaltestelle hauptbahnhofStPoelten;
        private IHaltestelle hauptbahnhofWrNeustadt;
        private IHaltestelle praterstern;
        private Point pointHauptbhfOst;
        private Point pointPraterstern;
        private Rectangle rectHbfPrater;
        private Rectangle rectHbfHbf;

        [TestInitialize]
        public void TestInitialize() {
            _data = new ConcurrentDictionary<int, IHaltestelle>();
            for (int i = 1; i <= 10; i++) {
                _data.TryAdd(i, new Haltestelle(i, 123, "Haltestelle " + i.ToString(), new Point()));
            }
            _mapperFictional = new HaltestellenMapper(_data, LinienMapperFactory.Instance);

            _mapperReal = HaltestellenMapperFactory.Instance;

            pointHauptbhfOst = new Point(48.1844162175681, 16.3803748315515);
            pointPraterstern = new Point(48.2185133276323, 16.3923272266447);

            rectHbfPrater = new Rectangle(pointPraterstern, pointHauptbhfOst);
            rectHbfHbf = new Rectangle(pointHauptbhfOst, pointHauptbhfOst);

            hauptbahnhof = new Haltestelle(214461409, 60201349, "Hauptbahnhof", new Point(48.1844162175681, 16.3803748315515));
            hauptbahnhofOst = new Haltestelle(214461006, 60200905, "Hauptbahnhof Ost", pointHauptbhfOst);
            hauptbahnhofStPoelten = new Haltestelle(214463796, 60204848, "St Pölten Hauptbahnhof", new Point(48.2081918762008, 15.6240387768482));
            hauptbahnhofWrNeustadt = new Haltestelle(214464157, 60205210, "Wr Neustadt Hauptbahnhof", new Point(47.8116057625811, 16.2341635785245));
            praterstern = new Haltestelle(214461125, 60201040, "Praterstern", pointPraterstern);
        }

        [TestMethod]
        public void TestFind() {
            for (int i = 1; i <= 10; i++) {
                Assert.AreEqual("Haltestelle " + i.ToString(), _mapperFictional.Find(i).Name, "Vorhandenes Element wurde nicht gefunden.");
            }

            Assert.AreEqual(hauptbahnhofOst, _mapperReal.Find(214461006));
            Assert.AreEqual(hauptbahnhof, _mapperReal.Find(214461409));
            Assert.AreEqual(praterstern, _mapperReal.Find(214461125));
        }

        [TestMethod, ExpectedException(typeof(KeyNotFoundException))]
        public void TestNotFound() {
            _mapperReal.Find(123);
        }

        [TestMethod]
        public void TestFindByName() {
            Assert.AreEqual(0, _mapperReal.FindByName("").Count);
            Assert.AreEqual(0, _mapperReal.FindByName(" ").Count);
            Assert.AreEqual(0, _mapperReal.FindByName("  ").Count);
            Assert.AreEqual(3, _mapperReal.FindByName("hauptbahnhof").Count);
            Assert.AreEqual(2, _mapperReal.FindByName("neustadt").Count);
            Assert.AreEqual(1, _mapperReal.FindByName("asd").Count);

            Assert.IsTrue(_mapperReal.FindByName("hauptbahnhof").Contains(hauptbahnhof));
            Assert.IsTrue(_mapperReal.FindByName("hauptbahnhof").Contains(hauptbahnhofOst));
        }

        [TestMethod]
        public void TestFindByRectangle() {
            List<IHaltestelle> result = _mapperReal.FindByRectangle(rectHbfPrater);
            Assert.IsTrue(result.Contains(praterstern));
            Assert.IsTrue(result.Contains(hauptbahnhofOst));

            result = _mapperReal.FindByRectangle(rectHbfHbf);
            Assert.IsTrue(result.Contains(hauptbahnhofOst));
            Assert.AreEqual(1, result.Count);

        }
    }
}
