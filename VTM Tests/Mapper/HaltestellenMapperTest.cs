using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViennaTrafficMonitor.Mapper;
using ViennaTrafficMonitor.Model;
using System.Collections.Generic;
using System.Windows;

namespace VtmTests.Mapper {

    [TestClass]
    public class HaltestellenMapperTest {

        private List<IHaltestelle> data;
        private IHaltestellenMapper mapper;

        [TestInitialize]
        public void TestInitialize() {
            data = new List<IHaltestelle>();
            for (int i = 1; i <= 10; i++) {
                data.Add(new Haltestelle(i, 123, "Haltestelle " + i.ToString(), new Point()));
            }
            mapper = new HaltestellenMapper(data);
        }

        [TestMethod]
        public void TestFind() {
            for (int i = 1; i <= 10; i++) {
                Assert.AreEqual("Haltestelle " + i.ToString(), mapper.Find(i).Name, "Vorhandenes Element wurde nicht gefunden.");    
            }
            try {
                mapper.Find(123);
                Assert.Fail("Find auf leeres Element wirft keine Exception.");
            } catch (InvalidOperationException) { }
        }

    }
}
