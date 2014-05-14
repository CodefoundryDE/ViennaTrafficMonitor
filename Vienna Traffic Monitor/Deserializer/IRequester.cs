using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViennaTrafficMonitor.Deserializer
{
    public interface IRequester
    {
        Task<Response> GetResponseAsync(int rbl);

        Task<Response> GetResponseAsync(ISet<int> rblEnumerable);

    }
}
