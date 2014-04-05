using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViennaTrafficMonitor.Mapper;
using ViennaTrafficMonitor.Model;
using System.Collections.Generic;
using System.Windows;

namespace VtmTests.Mapper {

    [TestClass]
    public class SteigMapperTest {

        private List<ISteig> data;
        private ISteigMapper mapper;

        [TestInitialize]
        public void TestInitialize() {
            data = new List<ISteig>();
            for (int i = 1; i <= 10; i++) {
                data.Add(new Steig(i, 123, 123, ERichtung.Rueck, 12, 1234, 34, "Bahnsteig " + i.ToString(), new Point()));
            }
            
            mapper = new SteigMapper(data);
        }

        [TestMethod]
        public void TestFind() {
            for (int i = 1; i <= 10; i++) {
                Assert.AreEqual("Bahnsteig " + i.ToString(), mapper.Find(i).Name, "Vorhandenes Element wurde nicht gefunden.");    
            }
            try {
                mapper.Find(123);
                Assert.Fail("Find auf leeres Element wirft keine Exception.");
            } catch (InvalidOperationException) { }
        }

    }
}
