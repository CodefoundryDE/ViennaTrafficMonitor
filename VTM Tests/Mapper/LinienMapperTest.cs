using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViennaTrafficMonitor.Mapper;
using ViennaTrafficMonitor.Model;
using System.Collections.Generic;
using System.Windows;
using System.Collections.Concurrent;

namespace VtmTests.Mapper {

    [TestClass]
    public class LinienMapperTest {

        private ConcurrentDictionary<int, ILinie> _data;
        private ILinienMapper _mapper;

        [TestInitialize]
        public void TestInitialize() {
            _data = new ConcurrentDictionary<int, ILinie>();
            for (int i = 1; i <= 10; i++) {
                _data.TryAdd(i, new Linie(i, "U" + i.ToString(), 1, true, EVerkehrsmittel.NachtBus));    
            }
            _mapper = new LinienMapper(_data);
        }

        [TestMethod]
        public void TestFind() {
            for (int i = 1; i <= 10; i++) {
                Assert.AreEqual("U" + i.ToString(), _mapper.Find(i).Bezeichnung, "Vorhandenes Element wurde nicht gefunden.");    
            }
        }

        [TestMethod, ExpectedException(typeof(KeyNotFoundException))]
        public void TestNotFound() {
            _mapper.Find(123);
        }

    }
}
