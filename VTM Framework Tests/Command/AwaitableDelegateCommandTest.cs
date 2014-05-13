using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VtmFramework.Command;
using System.Threading;
using System.Threading.Tasks;

namespace VtmFrameworkTests.Command {

    [TestClass]
    public class AwaitableDelegateCommandTest {

        private const int SLEEPTIME = 1000;

        private AwaitableDelegateCommand _command;
        private int testCount;

        [TestInitialize]
        public void TestInitialize() {
            _command = new AwaitableDelegateCommand(new Func<Task>(async () => { 
                await Task.Run(() => Thread.Sleep(SLEEPTIME)); 
            }));
        }

        [TestMethod]
        public async Task TestCanExecute() {
            Assert.IsTrue(_command.CanExecute(null), "CanExecute gibt nicht true zurück!");
            // Command ausführen, sollte SLEEPTIME ms laufen, währenddessen muss CanExecute false zurückgeben
            _command.Execute(null);
            await Task.Run(() => Thread.Sleep(100));
            Assert.IsFalse(_command.CanExecute(null), "CanExecute sollte während der Ausführung false sein!");
            await Task.Run(() => Thread.Sleep(SLEEPTIME));
            // CanExecute sollte wieder true zurückgeben, da sicher fertig
            Assert.IsTrue(_command.CanExecute(null), "CanExecute gibt nicht true zurück!");
        }

        [TestMethod]
        public async Task TestCanExecuteChanged() {
            int eventFired = 0;
            _command.CanExecuteChanged += (s, e) => {
                eventFired++;
            };
            _command.Execute(null);
            await Task.Run(() => Thread.Sleep(SLEEPTIME + 100));
            // Es sollte das Event jetzt 2mal gefeuert worden sein
            Assert.AreEqual(2, eventFired);
        }

        [TestMethod]
        public async Task TestCommand() {
            testCount = 0;
            var testIncrement = new Func<Task>(async () => { await Task.Run(() => { testCount++; }); });
            var command = new AwaitableDelegateCommand(testIncrement);
            for (int i = 0; i < 10; i++) {
                await command.ExecuteAsync();
            }
            Assert.AreEqual(10, testCount, "testCount hat nicht den erwarteten Wert!");
        }

    }

}
