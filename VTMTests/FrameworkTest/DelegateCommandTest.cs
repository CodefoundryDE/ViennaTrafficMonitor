using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViennaTrafficMonitor.Framework;

namespace VTMTests.FrameworkTest {

    [TestClass]
    public class DelegateCommandTest {

        /// <summary>
        /// Leeres Command zum grundsätzlichen Test
        /// </summary>
        public DelegateCommand Command { get; set; }

        [TestInitialize]
        public void TestInitialize() {
            Command = new DelegateCommand(() => { });
        }

        /// <summary>
        /// Hier werden einzelne Commands geprüft
        /// </summary>
        [TestMethod]
        public void TestExecute() {

            int integer = 0;
            Action action = () => {
                integer = 10;
            };

            DelegateCommand command = new DelegateCommand(action);
            command.Execute(null);

            Assert.AreEqual(10, integer, "Die Action wurde vom DelegateCommand nicht ausgeführt.");
        }

        [TestMethod]
        public void TestCanExecute() {
            Assert.AreEqual(true, Command.CanExecute(null), "CanExecute gibt nicht true zurück.");
        }
    }
}
