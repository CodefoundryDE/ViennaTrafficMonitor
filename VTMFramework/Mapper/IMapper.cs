﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtmFramework.Mapper {

    public interface IMapper<T> {

        T Find(int id);

    }

}
