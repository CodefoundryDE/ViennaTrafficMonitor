using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VtmFramework.Command;
using System.Threading;
using System.Threading.Tasks;

namespace VtmFrameworkTests.Command {

    [TestClass]
    public class AwaitableDelegateCommandTest {

        private AwaitableDelegateCommand _command;
        private int testCount;

        [TestInitialize]
        public void TestInitialize() {
            _command = new AwaitableDelegateCommand(_task);
        }

        private async Task _task() {
            await Task.Run(() => Thread.Sleep(1000));
        }

        [TestMethod]
        public async Task TestCanExecute() {
            Assert.IsTrue(_command.CanExecute(null), "CanExecute gibt nicht true zurück!");
            // Command ausführen, sollte 1000ms laufen, währenddessen muss CanExecute false zurückgeben
            _command.Execute(null);
            await Task.Run(() => Thread.Sleep(100));
            Assert.IsFalse(_command.CanExecute(null), "CanExecute sollte während der Ausführung false sein!");
            await Task.Run(() => Thread.Sleep(1000));
            // CanExecute sollte wieder true zurückgeben
            Assert.IsTrue(_command.CanExecute(null), "CanExecute gibt nicht true zurück!");
        }

        private async Task _testIncrement() {
            await Task.Run(() => testCount++);
        }

        [TestMethod]
        public async Task TestCommand() {
            testCount = 0;
            var command = new AwaitableDelegateCommand(_testIncrement);
            for (int i = 0; i < 10; i++) {
                await command.ExecuteAsync();
            }
            Assert.AreEqual(10, testCount, "testCount hat nicht den erwarteten Wert!");
        }

    }

}
