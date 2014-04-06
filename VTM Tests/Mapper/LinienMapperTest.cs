using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViennaTrafficMonitor.Mapper;
using ViennaTrafficMonitor.Model;
using System.Collections.Generic;
using System.Windows;

namespace VtmTests.Mapper {

    [TestClass]
    public class LinienMapperTest {

        private List<ILinie> data;
        private ILinienMapper mapper;

        [TestInitialize]
        public void TestInitialize() {
            data = new List<ILinie>();
            for (int i = 1; i <= 10; i++) {
                data.Add(new Linie(i, "U" + i.ToString(), 1, true, EVerkehrsmittel.NachtBus));    
            }
            mapper = new LinienMapper(data);
        }

        [TestMethod]
        public void TestFind() {
            for (int i = 1; i <= 10; i++) {
                Assert.AreEqual("U" + i.ToString(), mapper.Find(i).Bezeichnung, "Vorhandenes Element wurde nicht gefunden.");    
            }
            try {
                mapper.Find(123);
                Assert.Fail("Find auf leeres Element wirft keine Exception.");
            } catch (InvalidOperationException) { }
        }

    }
}
