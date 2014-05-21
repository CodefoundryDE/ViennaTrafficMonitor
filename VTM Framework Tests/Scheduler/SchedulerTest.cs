using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VtmFramework.Scheduler;
using VtmFramework.ViewModel;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace VtmFrameworkTests.Scheduler {

    [TestClass, System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public class SchedulerTest {

        private class ConcreteViewModel : AbstractViewModel { }

        private Scheduler<AbstractViewModel> _scheduler;

        [TestInitialize]
        public void TestInitialize() {
            _scheduler = new Scheduler<AbstractViewModel>();
            _scheduler.Interval = 1000;
        }

        [TestMethod]
        public async Task TestSchedule() {
            var vm1 = new ConcreteViewModel();
            var vm2 = new ConcreteViewModel();
            var vm3 = new ConcreteViewModel();

            // Scheduler hat noch kein Aktuelles ViewModel
            Assert.IsNull(_scheduler.Aktuell);

            _scheduler.Start();

            // Scheduler hat immer noch kein Aktuelles ViewModel
            Assert.IsNull(_scheduler.Aktuell);
             
            _scheduler.Schedule(vm1);
            _scheduler.Schedule(vm2);
            
            // Scheduler hat Aktuell = vm1
            Assert.ReferenceEquals(_scheduler.Aktuell, vm1);
            await Task.Run(() => Thread.Sleep(1100));

            // Scheduler hat Aktuell = vm2
            Assert.ReferenceEquals(_scheduler.Aktuell, vm2);
            await Task.Run(() => Thread.Sleep(1000));

            // Scheduler hat Aktuell wieder vm2
            Assert.ReferenceEquals(_scheduler.Aktuell, vm1);

            _scheduler.ScheduleInstant(vm3);

            // Scheduler hat jetzt vm3 und behält es
            Assert.ReferenceEquals(_scheduler.Aktuell, vm3);
            await Task.Run(() => Thread.Sleep(1500));
            Assert.ReferenceEquals(_scheduler.Aktuell, vm3);
        }

        [TestMethod]
        public void TestAktuellChanged() {
            // In dieser Liste werden die Events gesammelt, die der Scheduler raisen muss
            List<string> list = new List<string>();
            _scheduler.AktuellChanged += (e, s) => {
                list.Add(s.PropertyName);
            };
            _scheduler.Start();
            _scheduler.ScheduleInstant(new ConcreteViewModel());
            _scheduler.ScheduleInstant(new ConcreteViewModel());
            _scheduler.ScheduleInstant(new ConcreteViewModel());

            Assert.AreEqual(3, list.Count);
            for (int i = 0; i < 3; i++) {
                Assert.AreEqual("Aktuell", list[i]);
            }
        }
    }
}
