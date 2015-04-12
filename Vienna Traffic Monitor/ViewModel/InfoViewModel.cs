using System.Reflection;
using VtmFramework.ViewModel;

namespace ViennaTrafficMonitor.ViewModel
{

    public class InfoViewModel : AbstractViewModel
    {

        public string Version
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

    }

}
