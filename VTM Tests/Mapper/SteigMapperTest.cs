using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViennaTrafficMonitor.Mapper;
using ViennaTrafficMonitor.Model;
using System.Collections.Generic;
using System.Windows;
using System.Collections.Concurrent;

namespace VtmTests.Mapper {

    [TestClass]
    public class SteigMapperTest {

        private ConcurrentDictionary<int, ISteig> _data;
        private ISteigMapper _mapper;

        [TestInitialize]
        public void TestInitialize() {
            _data = new ConcurrentDictionary<int, ISteig>();
            for (int i = 1; i <= 10; i++) {
                _data.TryAdd(i, new Steig(i, 123, 123, ERichtung.Rueck, 12, 1234, 34, "Bahnsteig " + i.ToString(), new Point()));
            }
            
            _mapper = new SteigMapper(_data);
        }

        [TestMethod]
        public void TestFind() {
            for (int i = 1; i <= 10; i++) {
                Assert.AreEqual("Bahnsteig " + i.ToString(), _mapper.Find(i).Name, "Vorhandenes Element wurde nicht gefunden.");    
            }
        }

        [TestMethod, ExpectedException(typeof(KeyNotFoundException))]
        public void TestNotFound() {
            _mapper.Find(123);
        }

    }
}
