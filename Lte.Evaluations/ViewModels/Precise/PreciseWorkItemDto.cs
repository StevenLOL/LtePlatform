﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lte.Evaluations.ViewModels.Precise
{
    public class PreciseInterferenceNeighborDto
    {
        public int ENodebId { get; set; }

        public byte SectorId { get; set; }

        public double Db6Share { get; set; }

        public double Db10Share { get; set; }

        public double Mod3Share { get; set; }

        public double Mod6Share { get; set; }
    }

    public class PreciseInterferenceNeighborsContainer
    {
        public List<PreciseInterferenceNeighborDto> PreciseInterferenceNeighbors { get; set; }

        public string WorkItemNumber { get; set; }
    }

    public class PreciseInterferenceVictimDto
    {
        public int ENodebId { get; set; }

        public byte SectorId { get; set; }

        public double BackwardDb6Share { get; set; }

        public double BackwardDb10Share { get; set; }

        public double BackwardMod3Share { get; set; }

        public double BackwardMod6Share { get; set; }
    }

    public class PreciseInterferenceVictimsContainer
    {
        public List<PreciseInterferenceVictimDto> PreciseInterferenceVictims { get; set; }

        public string WorkItemNumber { get; set; }
    }

    public class PreciseCoverageDto
    {
        public int ENodebId { get; set; }

        public byte SectorId { get; set; }

        public double WeakCoverageRate { get; set; }

        public double OverCoverageRate { get; set; }
    }
}