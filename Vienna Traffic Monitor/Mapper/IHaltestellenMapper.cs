using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaTrafficMonitor.Model;
using VtmFramework.Mapper;
using VtmFramework.Library;
using System.Windows;

namespace ViennaTrafficMonitor.Mapper {

    public interface IHaltestellenMapper : IMapper<IHaltestelle> {

        List<IHaltestelle> FindByName(String name);

        List<IHaltestelle> FindByRectangle(Rectangle rect);

        IDictionary<int, Point> GetAllCoordinates();
    }

}
