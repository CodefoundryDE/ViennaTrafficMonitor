﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtmFramework.Logging {
    public static class VtmLoggerFactory {

        public static IVtmLogger GetInstance (string logPath) {
            VtmLogger.LogPath = logPath;
            return GetInstance();
        }

        public static IVtmLogger GetInstance () {
            return VtmLogger.Instance;
        }

    }
}
