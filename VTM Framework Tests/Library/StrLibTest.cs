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

        [TestMethod]
        public void TestAsciiInc() {
            Assert.AreEqual('b', StrLib.AsciiInc('a', 'a', 'z')); // a -> b
            Assert.AreEqual('n', StrLib.AsciiInc('m', 'a', 'z')); // m -> n
            Assert.AreEqual('z', StrLib.AsciiInc('y', 'a', 'z')); // y -> z
            Assert.AreEqual('a', StrLib.AsciiInc('z', 'a', 'z')); // z -> a

            // Auto-Korrektur
            Assert.AreEqual('a', StrLib.AsciiInc('A', 'a', 'z')); // A -> a
            Assert.AreEqual('z', StrLib.AsciiInc('{', 'a', 'z')); // { -> z
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void TestAsciiIncArgumentException() {
            StrLib.AsciiInc('m', 'z', 'a');
        }

        [TestMethod]
        public void TestUmlautFilter() {
            Assert.AreEqual("AeOeUeaeoeuess", StrLib.UmlautFilter("ÄÖÜäöüß"));
        }

    }
}
