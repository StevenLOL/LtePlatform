﻿using Lte.MySqlFramework.Abstract;
using Lte.MySqlFramework.Entities;
using Lte.Parameters.MockOperations;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lte.Evaluations.DataService.College
{
    public static class CollegeKpiQueries
    {
        public static void AssertKpis(this CollegeKpiView view, int collegeId, double kpi)
        {
            Assert.AreEqual(view.CollegeName, "college-" + collegeId);
            Assert.AreEqual(view.Connection3G, kpi);
        }

        public static void MockOneItem(this Mock<ICollegeKpiRepository> repository, int id, DateTime time)
        {
            repository.MockAuditedItems(new List<CollegeKpi>
            {
                new CollegeKpi
                {
                    CollegeId = id,
                    TestTime = time
                }
            }.AsQueryable());
        }

        public static void MockOneItem(this Mock<ICollegeKpiRepository> repository, int id, DateTime time, double kpi)
        {
            repository.MockAuditedItems(new List<CollegeKpi>
            {
                new CollegeKpi
                {
                    CollegeId = id,
                    TestTime = time,
                    Connection3G = kpi
                }
            }.AsQueryable());
        }

        public static void MockItems(this Mock<ICollegeKpiRepository> repository, int[] collegeIds, DateTime time,
            double[] kpis)
        {
            var resultList = collegeIds.Select((t, i) => new CollegeKpi
            {
                CollegeId = t,
                TestTime = time,
                Connection3G = kpis[i]
            }).ToList();
            repository.MockAuditedItems(resultList.AsQueryable());
        }

        public static void MockItems(this Mock<ICollegeKpiRepository> repository, int[] collegeIds, DateTime[] times,
            double[] kpis)
        {
            var resultList = collegeIds.Select((t, i) => new CollegeKpi
            {
                CollegeId = t,
                TestTime = times[i],
                Connection3G = kpis[i]
            }).ToList();
            repository.MockAuditedItems(resultList.AsQueryable());
        }
    }
}
