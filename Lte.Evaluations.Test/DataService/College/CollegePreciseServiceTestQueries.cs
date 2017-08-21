﻿using Lte.Parameters.Entities.Basic;
using NUnit.Framework;

namespace Lte.Evaluations.DataService.College
{
    public static class CollegePreciseServiceTestQueries
    {
        public static void AssertBasicParameters(this CellPreciseKpiView view, int eNodebId, byte sectorId,
            double rsPower, double height, string indoor, double downTilt)
        {
            Assert.AreEqual(view.RsPower, rsPower);
            Assert.AreEqual(view.ENodebId, eNodebId);
            Assert.AreEqual(view.SectorId, sectorId);
            Assert.AreEqual(view.Height, height);
            Assert.AreEqual(view.Indoor, indoor);
            Assert.AreEqual(view.DownTilt, downTilt, 1E-6);
        }
    }
}
