using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtmFramework.Logging;

namespace VtmFrameworkTests.Logging {
    [TestClass]
    public class LoggingTest {

        private IVtmLogger _log;
        private string _path;

        [TestInitialize]
        public void TestInitialize() {
            _path = ".\\TestLog.log";
            VtmLogger.SetLoggingLevel(TraceLevel.Verbose);
            _log = VtmLoggerFactory.GetInstance(_path);
            //_log = VTMLoggerFactory.getInstance();
        }

        [TestMethod]
        public void TestLogging() {
            //_log.Info("Das ist eine Info");
            //_log.Warning("Das ist eine Warnung!");
            //_log.Error("TestError 001");
            //_log.Error(new NullReferenceException("NullException 002"));
            Assert.AreEqual(File.Exists(_path), true,"LogFile korrekt angelegt");
        }


        [TestCleanup]
        public void Cleanup() {
            //Löschen des TestLog-Files
            _log.Dispose();
            System.IO.File.Delete(_path);
        }
    }
}
