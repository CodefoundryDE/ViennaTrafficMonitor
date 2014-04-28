using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtmFramework.Logging
{
    class VTMLogger : VtmFramework.Logging.IVtmLogger
    {
        private TraceListener _tListener = null;
        private BooleanSwitch _bSwitch = null;
        private TraceSwitch _tSwitch = null;

        public VTMLogger()
        {
            _tListener = Trace.Listeners["Tracer"];
            _bSwitch = new BooleanSwitch("Enable", "", VtmFramework.Properties.Settings.Default.Enable);
            _tSwitch = new TraceSwitch("Type", "", VtmFramework.Properties.Settings.Default.Type);

        }

        public void Error(Exception ex)
        {
            //Logging im Falle von "nur Errors" oder "Alles"
            if (_bSwitch.Enabled && (_tSwitch.TraceError || _tSwitch.TraceVerbose))
            {
                Trace.WriteLine(DateTime.Now + " " + ex.Message);
                Trace.TraceError(ex.StackTrace);
            }
        }

        public void Error (String errorMessage) {
            if (_bSwitch.Enabled && (_tSwitch.TraceError || _tSwitch.TraceVerbose))
            {
                Trace.TraceError(DateTime.Now + " " + errorMessage);
            }
        }

        public void Warning(String warningMessage)
        {
            if (_bSwitch.Enabled && (_tSwitch.TraceWarning|| _tSwitch.TraceVerbose))
            {
                Trace.TraceWarning(DateTime.Now + " " + warningMessage);
            }
        }

        public void Info (String infoMessage)
        {
            if (_bSwitch.Enabled && (_tSwitch.TraceInfo || _tSwitch.TraceVerbose))
            {
                Trace.TraceInformation(DateTime.Now + " " + infoMessage);
            }
        }
    }
}
