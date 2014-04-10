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
    public class SteigeImportTest
    {
        private String testPathSteig = @"..\..\CsvImport\TestFiles\DemoSteige.csv";

        [TestInitialize]

        [TestMethod]

        public void TestSteigeImport()
        {
            ConcurrentDictionary<int, ISteig> steige = SteigeParser.ReadFile(testPathSteig);

            ISteig testSteig;
            Point testPunkt;

            if (steige.TryGetValue(214689583, out testSteig)) {
                testPunkt = new Point(48.1748752577222, 16.3779867315022);

                Assert.AreEqual(214689583, testSteig.Id);
                Assert.AreEqual(214433687, testSteig.LinienId);
                Assert.AreEqual(214461177, testSteig.HaltestellenId);
                Assert.AreEqual(ERichtung.Hin, testSteig.Richtung);
                Assert.AreEqual(1, testSteig.Reihenfolge);
                Assert.AreEqual(4101, testSteig.Rbl);
                Assert.AreEqual(1, testSteig.Bereich);
                Assert.AreEqual("U1-H", testSteig.Name);
                Assert.AreEqual(testPunkt, testSteig.Location);
            }
            else
            {
                Assert.Fail();
            }

            if (steige.TryGetValue(214689631, out testSteig))
            {
                testPunkt = new Point(48.2431816197346, 16.4329066285714);

                Assert.AreEqual(214689631, testSteig.Id);
                Assert.AreEqual(214433687, testSteig.LinienId);
                Assert.AreEqual(214460732, testSteig.HaltestellenId);
                Assert.AreEqual(ERichtung.Rueck, testSteig.Richtung);
                Assert.AreEqual(6, testSteig.Reihenfolge);
                Assert.AreEqual(4102, testSteig.Rbl);
                Assert.AreEqual(1, testSteig.Bereich);
                Assert.AreEqual("U1-R", testSteig.Name);
                Assert.AreEqual(testPunkt, testSteig.Location);
            }
            else
            {
                Assert.Fail();
            }

            if (steige.TryGetValue(218865019, out testSteig))
            {
                testPunkt = new Point(48.240444516333, 16.499354298603);

                Assert.AreEqual(218865019, testSteig.Id);
                Assert.AreEqual(215096434, testSteig.LinienId);
                Assert.AreEqual(214460825, testSteig.HaltestellenId);
                Assert.AreEqual(ERichtung.Rueck, testSteig.Richtung);
                Assert.AreEqual(4, testSteig.Reihenfolge);
                Assert.AreEqual(8061, testSteig.Rbl);
                Assert.AreEqual(0, testSteig.Bereich);
                Assert.AreEqual("97A-R", testSteig.Name);
                Assert.AreEqual(testPunkt, testSteig.Location);
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}