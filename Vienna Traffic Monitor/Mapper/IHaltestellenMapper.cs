using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Model;
using VtmFramework.Mapper;
using VtmFramework.Libary;

namespace ViennaTrafficMonitor.Mapper {

    public interface IHaltestellenMapper : IMapper<IHaltestelle> {

        public List<IHaltestelle> findByName(String name);

        public List<IHaltestelle> findByRectangle(Rectangle rect);

        public IHaltestelle Find(int id);
    }

}
