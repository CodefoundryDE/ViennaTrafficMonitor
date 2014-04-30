using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VtmFramework.Library;

namespace VtmFrameworkTests.Library {

    [TestClass]
    public class StrLibTest {

        [TestMethod]
        public void TestStrCat() {
            Assert.AreEqual("Hallo", StrLib.StrCat("Hallo", "", " / "));
            Assert.AreEqual("Welt", StrLib.StrCat("", "Welt", " / "));
            Assert.AreEqual("HalloWelt", StrLib.StrCat("Hallo", "Welt", ""));
            Assert.AreEqual("Hallo / Welt", StrLib.StrCat("Hallo", "Welt", " / "));
            Assert.AreEqual("Hallo / Welt", StrLib.StrCat(" Hallo ", " Welt ", " / "));
            Assert.AreEqual("", StrLib.StrCat(" ", " ", " / "));
            Assert.AreEqual("", StrLib.StrCat("", "", " / "));
        }

    }
}
