﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lte.Domain.Common.Wireless
{
    public interface ICellStastic
    {
        int Mod3Count { get; set; }

        int WeakCoverCount { get; set; }

        int Mod6Count { get; set; }

        int OverCoverCount { get; set; }

        int PreciseCount { get; set; }

        int MrCount { get; set; }
    }

    public interface IBtsIdQuery
    {
        int BtsId { get; }
    }

    public interface ILteCellQuery
    {
        int ENodebId { get; set; }

        byte SectorId { get; set; }
    }

    public interface IWorkItemCell : ILteCellQuery
    {
        string WorkItemNumber { get; set; }
    }
}
