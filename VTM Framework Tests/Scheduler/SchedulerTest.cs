using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VtmFramework.Scheduler;

namespace VtmFrameworkTests.Scheduler {

    [TestClass]
    public class SchedulerTest {

        private Scheduler<object> _scheduler;

        [TestInitialize]
        public void TestInitialize() {
            _scheduler = new Scheduler<object>();
        }

        [TestMethod]
        public void TestMethod1() {

        }

    }
}
