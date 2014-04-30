using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtmFramework.Logging {
    public static class VTMLoggerFactory {

        public static IVtmLogger getInstance (string logPath) {
            VTMLogger.LogPath = logPath;
            return getInstance();
        }

        public static IVtmLogger getInstance () {
            return VTMLogger.Instance;
        }

    }
}
