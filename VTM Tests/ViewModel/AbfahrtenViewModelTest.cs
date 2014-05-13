using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Deserializer;
using ViennaTrafficMonitor.Model;
using ViennaTrafficMonitor.ViewModel;

namespace VtmTests.ViewModel {
    [TestClass]
    public class AbfahrtenViewModelTest {
        //haltestellenId: 214461125 Praterstern
        private int praterstern = 214461125;

        private AbfahrtenViewModel avm;

        [TestInitialize]
        public void TestInitialize() {
            avm = AbfahrtenViewModelFactory.GetInstance(praterstern);
        }

        [TestMethod]
        public void TestFilter() {
            IHaltestelle pratersternHaltestelle = avm.Haltestelle;
            ICollection<VtmResponse> abfahrten = avm.Abfahrten;
        }

    }
}
