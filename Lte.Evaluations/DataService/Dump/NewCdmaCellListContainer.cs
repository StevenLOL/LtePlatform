﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Domain.Regular;
using Lte.Parameters.Entities;

namespace Lte.Evaluations.DataService.Dump
{
    [TypeDoc("CDMA小区EXCEL信息容器，用于打包向服务器POST")]
    public class NewCdmaCellListContainer
    {
        public IEnumerable<CdmaCellExcel> Infos { get; set; } 
    }
}
