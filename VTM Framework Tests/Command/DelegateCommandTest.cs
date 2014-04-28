using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VtmFramework.Command;


namespace VtmFrameworkTests.Command {

    [TestClass]
    public class DelegateCommandTest {

        /// <summary>
        /// Leeres _command zum grundsätzlichen Test
        /// </summary>
        private DelegateCommand _command { get; set; }

        [TestInitialize]
        public void TestInitialize() {
            _command = new DelegateCommand(() => { });
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
            Assert.AreEqual(true, _command.CanExecute(null), "CanExecute gibt nicht true zurück.");
        }
    }
}
