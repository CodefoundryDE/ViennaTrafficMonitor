using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Model;
using VtmFramework.Mapper;

namespace ViennaTrafficMonitor.Mapper {

    public interface ILinienMapper : IMapper<ILinie> {

        /// <summary>
        /// Sucht Linien welche eine bestimmte Bezeichnung enthalten.
        /// </summary>
        /// <param name="bezeichnung"></param>
        /// <returns>Liste von Linien</returns>
        List<ILinie> FindByBezeichnung(string bezeichnung);
    
    }

}
