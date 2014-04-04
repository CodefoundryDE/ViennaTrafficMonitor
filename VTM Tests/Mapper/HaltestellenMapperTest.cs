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
            data.Add(new Haltestelle(1, 123, "Test 1", new Point()));
            data.Add(new Haltestelle(2, 456, "Test 2", new Point()));
            data.Add(new Haltestelle(3, 789, "Test 3", new Point()));
            data.Add(new Haltestelle(4, 101, "Test 4", new Point()));

            mapper = new HaltestellenMapper(data);
        }

        [TestMethod]
        public void TestFind() {
            Assert.AreEqual("Test 1", mapper.Find(1).Name, "Vorhandenes Element wurde nicht gefunden.");
            Assert.AreEqual("Test 2", mapper.Find(2).Name, "Vorhandenes Element wurde nicht gefunden.");
            Assert.AreEqual("Test 3", mapper.Find(3).Name, "Vorhandenes Element wurde nicht gefunden.");
            Assert.AreEqual("Test 4", mapper.Find(4).Name, "Vorhandenes Element wurde nicht gefunden.");

            try {
                mapper.Find(123);
                Assert.Fail("Find auf leeres Element wirft keine Exception.");
            } catch (InvalidOperationException) { }
        }

    }
}
