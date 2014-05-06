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
    public class VTMLogger : VtmFramework.Logging.IVTMLogger {
        private BooleanSwitch _bSwitch = null;
        private static FileStream _hlogFile = null;
        private static TraceSwitch _tSwitch = new TraceSwitch("Type", "", VtmFramework.Properties.Settings.Default.Type);
        private static volatile VTMLogger _instance;
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
            TraceListener _tListener =Trace.Listeners[_traceName];
            _bSwitch = new BooleanSwitch("Enable", "", VtmFramework.Properties.Settings.Default.Enable);

        }

        public static VTMLogger Instance {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get {
                if (_instance == null || _logPathChanged) {
                    if (_logPathChanged == true) {
                        
                        CreateTraceListener(_logPath);
                        _logPathChanged = false;
                    } else {
                        CreateTraceListener(_defaultLogPath);
                    }
                    _instance = new VTMLogger();
                }
                return _instance;
            }
        }

        private static void CreateTraceListener(string logPath) {
            Trace.Listeners.Clear();
            if (_hlogFile != null) {
                _hlogFile.Dispose();
            }
            _hlogFile = new FileStream(logPath, FileMode.OpenOrCreate, FileAccess.Write);
            TextWriterTraceListener VTMListener = new TextWriterTraceListener(_hlogFile);
            VTMListener.Name = _traceName;
            Trace.Listeners.Add(VTMListener);

        }

        public void Error(Exception ex) {
            //Logging im Falle von "nur Errors" oder "Alles"
            if (_bSwitch.Enabled && (_tSwitch.TraceError || _tSwitch.TraceVerbose)) {
                if (ex != null) {
                    Trace.WriteLine(DateTime.Now + " " + ex.Message);
                    Trace.TraceError(ex.StackTrace);
                } else {
                    Trace.WriteLine("Error-Logging aufgerufen: Übergebene Exception = NULL!");
                }
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

        public static void SetLoggingLevel(System.Diagnostics.TraceLevel level) {
            _tSwitch.Level = level;
        }

        public void Dispose() {
            //Freigabe des Filehandels und der Instanz; 
            _hlogFile.Dispose();
            _instance = null;            
        }


    }
}
