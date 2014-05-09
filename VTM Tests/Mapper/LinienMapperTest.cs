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

        [TestMethod]
        public void TestFindByBezeichnung() {
            IList<ILinie> list = _mapper.FindByBezeichnung("U");
            Assert.AreEqual(10, list.Count);

            list = _mapper.FindByBezeichnung("U3");
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("U3", list[0].Bezeichnung);

            list = _mapper.FindByBezeichnung("XYZ");
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod, ExpectedException(typeof(KeyNotFoundException))]
        public void TestNotFound() {
            _mapper.Find(123);
        }

    }
}
