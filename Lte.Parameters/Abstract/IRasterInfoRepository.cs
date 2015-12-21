﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Abstract
{
    public interface IRasterInfoRepository
    {
        IQueryable<RasterInfo> RasterInfos { get; }
    }
}
