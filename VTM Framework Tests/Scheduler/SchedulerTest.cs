using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VtmFramework.Scheduler;
using VtmFramework.ViewModel;

namespace VtmFrameworkTests.Scheduler {

    [TestClass, System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public class SchedulerTest {

        private Scheduler<AbstractViewModel> _scheduler;

        [TestInitialize]
        public void TestInitialize() {
            _scheduler = new Scheduler<AbstractViewModel>();
        }

        [TestMethod]
        public void TestMethod1() {

        }

    }
}
