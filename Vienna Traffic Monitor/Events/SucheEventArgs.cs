using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViennaTrafficMonitor.Events {

    public class SucheEventArgs : EventArgs {

        public SucheEventArgs(int haltestelleSelected) {
            HaltestelleSelected = haltestelleSelected;
        }

        /// <summary>
        /// Die ausgewählte Haltestelle
        /// </summary>
        public int HaltestelleSelected { get; set; }

    }

}
