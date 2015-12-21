﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Abstract
{
    public interface ICsvFileInfoRepository
    {
        IQueryable<CsvFilesInfo> CsvFilesInfos { get; }

        IEnumerable<FileRecord4G> GetFileRecord4Gs(string fileName);
    }
}
