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
                _data.TryAdd(i, new Steig(i, 123, 123, ERichtung.Rueck, i + 5, 1234, 34, "Bahnsteig " + i.ToString(), new Point()));
            }
            for (int i = 11; i <= 20; i++) {
                _data.TryAdd(i, new Steig(i, 2 + (i % 2), 987, ERichtung.Hin, i, i + i * 2, 23, "Bahnsteig " + i.ToString(), new Point()));
            }

            _mapper = new SteigMapper(_data);
        }

        [TestMethod]
        public void TestSteig_Find() {
            for (int i = 1; i <= 10; i++) {
                Assert.AreEqual("Bahnsteig " + i.ToString(), _mapper.Find(i).Name, "Vorhandenes Element wurde nicht gefunden.");
            }
        }

        [TestMethod, ExpectedException(typeof(KeyNotFoundException))]
        public void TestSteig_NotFound() {
            _mapper.Find(123);
        }

        [TestMethod]
        public void TestSteig_FindByLinie() {
            List<ISteig> result = _mapper.FindByLinie(123);
            int rhf = 6;
            for (int i = 0; i < 10; i++) {
                Assert.AreEqual(result[i].LinienId, 123, "Linien ID (123) in Result-Liste nicht korrekt");
                Assert.AreEqual(result[i].Reihenfolge, rhf++, "Reihenfolge nicht korrekt!");
            }
            result = _mapper.FindByLinie(2);
            rhf = 12;
            for (int i = 0; i < 5; i++) {
                Assert.AreEqual(result[i].LinienId, 2, "Linien ID (2) in Result-Liste nicht korrekt");
                Assert.AreEqual(result[i].Reihenfolge, rhf, "Reihenfolge nicht korrekt!");
                rhf += 2;
            }
        }

        [TestMethod]
        public void TestSteig_FindByRbl() {
            List<ISteig> result = _mapper.FindByRbl(1234);
            for (int i = 0; i < 10; i++) {
                Assert.AreEqual(result[i].Rbl, 1234, "RBL(1234) nicht korrekt!");
            }
            result = _mapper.FindByRbl(54);
            Assert.AreEqual(result.Count, 1, "Falsche Ergabnisanzahl (>1) für RBL = 54");
            Assert.AreEqual(result[0].Rbl, 54, "Falsche RBL im Suchergebnis (!= 54)");
        }

        [TestMethod]
        public void TestSteig_FindByHaltestelle() {
            List<ISteig> result = _mapper.FindByHaltestelle(987);
            for (int i = 0; i < 10; i++) {
                Assert.AreEqual(i + 11, result[i].Id,"SteigId bei FindByHaltestelle nicht korrekt!");
                Assert.AreEqual(987, result[i].HaltestellenId, "Haltestelle bei FindByHaltestelle nicht korrekt!");
            }
        }
    }
}
