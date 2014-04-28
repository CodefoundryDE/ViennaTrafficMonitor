using System;
namespace VtmFramework.Logging {

    public interface IVtmLogger {
        void Error(Exception ex);
        void Error(string errorMessage);
        void Info(string infoMessage);
        void Warning(string warningMessage);
    }

}
