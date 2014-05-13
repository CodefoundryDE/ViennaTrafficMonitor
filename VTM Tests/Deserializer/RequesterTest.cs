using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViennaTrafficMonitor.Deserializer;

namespace VtmTests.Deserializer {

    [TestClass]
    public class RequesterTest {

        private Response _Response;
        private Response _ResponseEnumerable;
        private int _TestRbl;
        private IEnumerable<int> _TestRblEnumerable;
        private IRequester _Requester;
        private IRequester _DummyRequester;

        [TestInitialize]
        public void TestInitialize() {

            _TestRbl = 147;
            _TestRblEnumerable = new int[] { 7, 8, 9 };
            _Requester = new RblRequester();
            _DummyRequester = new DummyRequester();

        }

        [TestMethod]
        public async Task TestRequest() {

            Task<Response> request = _Requester.GetResponseAsync(_TestRbl);
            Task<Response> requestEnumerable = _Requester.GetResponseAsync(_TestRblEnumerable);

            _Response = await request;
            _ResponseEnumerable = await requestEnumerable;

            Assert.IsNotNull(_Response);
            Assert.IsNotNull(_ResponseEnumerable);



        }

        [TestMethod]
        public async Task TestDummyRequest() {

            Task<Response> request = _DummyRequester.GetResponseAsync(_TestRbl);
            Task<Response> requestEnumerable = _DummyRequester.GetResponseAsync(_TestRblEnumerable);

            _Response = await request;
            _ResponseEnumerable = await requestEnumerable;

            Assert.IsNotNull(_Response);
            Assert.IsNotNull(_ResponseEnumerable);

        }





    }
}
