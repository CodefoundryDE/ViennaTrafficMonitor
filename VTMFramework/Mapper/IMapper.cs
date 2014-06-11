using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtmFramework.Mapper {

    public interface IMapper<T> {

        /// <summary>
        /// Gibt einen Datensatz basierend auf der Id aus.
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>T</returns>
        T Find(int id);
    }

}
