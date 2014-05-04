using System;
namespace VtmFramework.Logging {

    public interface IVTMLogger {
        void Error(Exception ex);
        void Error(string errorMessage);
        void Info(string infoMessage);
        void Warning(string warningMessage);
        void Dispose();
    }

}
