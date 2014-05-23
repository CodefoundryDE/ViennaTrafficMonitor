using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Data;
using VtmFramework.Converter;

namespace VtmFrameworkTests.Converter {

    [TestClass]
    public class BoolToOpacityConverterTest {

        private IValueConverter _converter;

        [TestInitialize]
        public void TestInitialize() {
            _converter = new BoolToOpacityConverter();
        }

        [TestMethod]
        public void TestConvert() {
            Assert.AreEqual(0.5, _converter.Convert(false, typeof(double), null, null));
            Assert.AreEqual(1.0, _converter.Convert(true, typeof(double), null, null));
        }

    }

}
