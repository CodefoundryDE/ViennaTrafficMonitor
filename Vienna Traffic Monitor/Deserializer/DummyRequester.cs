using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViennaTrafficMonitor.Deserializer
{
    class DummyRequester : IRequester
    {
        public Task<Response> GetResponseAsync(int rbl)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetResponseAsync(IEnumerable<int> rblEnumerable)
        {
            throw new NotImplementedException();
        }

    }
}
