using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViennaTrafficMonitor.Mapper;
using ViennaTrafficMonitor.Model;
using System.Collections.Generic;
using System.Windows;
using System.Collections.Concurrent;

namespace VtmTests.Mapper {

    [TestClass]
    public class HaltestellenMapperTest {

        private ConcurrentDictionary<int, IHaltestelle> _data;
        private IHaltestellenMapper _mapper;

        [TestInitialize]
        public void TestInitialize() {
            _data = new ConcurrentDictionary<int, IHaltestelle>();
            for (int i = 1; i <= 10; i++) {
                _data.TryAdd(i, new Haltestelle(i, 123, "Haltestelle " + i.ToString(), new Point()));
            }
            _mapper = new HaltestellenMapper(_data);
        }

        [TestMethod]
        public void TestFind() {
            for (int i = 1; i <= 10; i++) {
                Assert.AreEqual("Haltestelle " + i.ToString(), _mapper.Find(i).Name, "Vorhandenes Element wurde nicht gefunden.");    
            }
        }

        [TestMethod, ExpectedException(typeof(KeyNotFoundException))]
        public void TestNotFound() {
            _mapper.Find(123);
        }

    }
}
