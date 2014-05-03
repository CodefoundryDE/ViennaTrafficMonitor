using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VtmFramework.Logging {
    class VTMLogger : VtmFramework.Logging.IVtmLogger {
        private TraceListener _tListener = null;
        private BooleanSwitch _bSwitch = null;
        private TraceSwitch _tSwitch = null;
        private static volatile VTMLogger instance;
        private static String _logPath = "";
        private static bool _logPathChanged = false;
        private static string _defaultLogPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\VTM_LOG.log"; //Environment.ExpandEnvironmentVariables("%userprofile%") + @"\AppData\VTMTrafficMonitor\VTM_Log.log";
        private static string _traceName = "VTMTrace";

        public static String LogPath {
            get { return _logPath; }
            set {
                _logPath = value;
                _logPathChanged = true;
            }
        }


        private VTMLogger() {
            _tListener = Trace.Listeners[_traceName];
            _bSwitch = new BooleanSwitch("Enable", "", VtmFramework.Properties.Settings.Default.Enable);
            _tSwitch = new TraceSwitch("Type", "", VtmFramework.Properties.Settings.Default.Type);

        }

        public static VTMLogger Instance {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get {
                if (_logPathChanged == true) {
                    CreateTraceListener(_logPath);
                }
                if (instance == null) {
                    CreateTraceListener(_defaultLogPath);
                } else {
                    return instance;
                }
                instance = new VTMLogger();
                return instance;
            }
        }

        private static void CreateTraceListener(string logPath) {
            Trace.Listeners.Clear();
            FileStream hlogFile = new FileStream(logPath, FileMode.OpenOrCreate, FileAccess.Write);
            TextWriterTraceListener VTMListener = new TextWriterTraceListener(hlogFile);
            VTMListener.Name = _traceName;
            Trace.Listeners.Add(VTMListener);

        }

        public void Error(Exception ex) {
            //Logging im Falle von "nur Errors" oder "Alles"
            if (_bSwitch.Enabled && (_tSwitch.TraceError || _tSwitch.TraceVerbose)) {
                Trace.WriteLine(DateTime.Now + " " + ex.Message);
                Trace.TraceError(ex.StackTrace);
            }
        }

        public void Error(String errorMessage) {
            if (_bSwitch.Enabled && (_tSwitch.TraceError || _tSwitch.TraceVerbose)) {
                Trace.TraceError(DateTime.Now + " " + errorMessage);
            }
        }

        public void Warning(String warningMessage) {
            if (_bSwitch.Enabled && (_tSwitch.TraceWarning || _tSwitch.TraceVerbose)) {
                Trace.TraceWarning(DateTime.Now + " " + warningMessage);
            }
        }

        public void Info(String infoMessage) {
            if (_bSwitch.Enabled && (_tSwitch.TraceInfo || _tSwitch.TraceVerbose)) {
                Trace.TraceInformation(DateTime.Now + " " + infoMessage);
            }
        }
    }
}
