using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViennaTrafficMonitor.Deserializer
{
    public class RequesterFactory
    {
        public static IRequester GetInstance()
        {
            if (ViennaTrafficMonitor.Properties.Settings.Default.DummyRequester)
            {
                return new DummyRequester();
            }
            return new RblRequester();

        }

    }
}
