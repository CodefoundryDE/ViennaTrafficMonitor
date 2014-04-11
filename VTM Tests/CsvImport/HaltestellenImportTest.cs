using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViennaTrafficMonitor.CsvImport.Parser;
using ViennaTrafficMonitor.Model;

namespace VtmTests.CsvImport
{

    [TestClass]
    public class HaltestellenImportTest
    {
        private String testPathHaltestelle = @"..\..\CsvImport\TestFiles\DemoHaltestellen.csv";

        [TestInitialize]

        [TestMethod]

        public void TestHaltestellenImport()
        {
            ConcurrentDictionary<int, IHaltestelle> haltestellen = HaltestellenParser.ReadFile(testPathHaltestelle);

            IHaltestelle testHaltestelle;
            Point testPunkt;

            if (haltestellen.TryGetValue(214460106, out testHaltestelle))
            {
                testPunkt = new Point(48.1738001480236, 16.3898043925363);

                Assert.AreEqual(214460106, testHaltestelle.Id);
                Assert.AreEqual(60200001, testHaltestelle.Diva);
                Assert.AreEqual("Absberggasse", testHaltestelle.Name);
                Assert.AreEqual(testPunkt, testHaltestelle.Location);
            }
            else
            {
                Assert.Fail();
            }

            if (haltestellen.TryGetValue(214460122, out testHaltestelle))
            {
                testPunkt = new Point(48.1720950446652, 16.4724758507966);

                Assert.AreEqual(214460122, testHaltestelle.Id);
                Assert.AreEqual(60200017, testHaltestelle.Diva);
                Assert.AreEqual("Alberner Hafenzufahrtsstraße", testHaltestelle.Name);
                Assert.AreEqual(testPunkt, testHaltestelle.Location);
            }
            else
            {
                Assert.Fail();
            }

            if (haltestellen.TryGetValue(214460135, out testHaltestelle))
            {
                testPunkt = new Point(48.1687307099108, 16.349399557923);

                Assert.AreEqual(214460135, testHaltestelle.Id);
                Assert.AreEqual(60200030, testHaltestelle.Diva);
                Assert.AreEqual("Triester Str./Altdorferstraße", testHaltestelle.Name);
                Assert.AreEqual(testPunkt, testHaltestelle.Location);
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}