﻿using System.Collections.Generic;
using System.Linq;
using Abp.EntityFramework.AutoMapper;
using Abp.Reflection;
using AutoMapper;
using Lte.Domain.Common.Wireless;
using Lte.Evaluations.Policy;
using Lte.MySqlFramework.Abstract;
using Lte.MySqlFramework.Entities;

namespace Lte.Evaluations.MapperSerive
{
    public class EvalutaionMapperProfile : Profile
    {
        private readonly ITypeFinder _typeFinder = new TypeFinder(new MyAssemblyFinder());

        protected override void Configure()
        {
            var module = new AbpAutoMapperModule(_typeFinder);
            module.PostInitialize();
        }
    }

    public static class TopCellQueriesService
    {
        public static IEnumerable<TopCellContainer<TTopCell>> QueryContainers<TTopCell>(this List<TTopCell> topCells,
            IBtsRepository btsRepository, IENodebRepository eNodebRepository)
            where TTopCell : IBtsIdQuery
        {
            return from stat in
                topCells
                   join bts in btsRepository.GetAllList()
                       on stat.BtsId equals bts.BtsId into btsQuery
                   from bq in btsQuery.DefaultIfEmpty()
                   join eNodeb in eNodebRepository.GetAllList()
                       on bq?.ENodebId ?? -1 equals eNodeb.ENodebId into query
                   from q in query.DefaultIfEmpty()
                   select new TopCellContainer<TTopCell>
                   {
                       TopCell = stat,
                       LteName = q == null ? "无匹配LTE基站" : q.Name,
                       CdmaName = bq == null ? "无匹配CDMA基站" : bq.Name
                   };
        }

        public static IEnumerable<TopDrop2GTrend> QueryTrends(this IEnumerable<TopDrop2GCell> stats)
        {
            return from stat in stats
                   group stat by new { stat.BtsId, stat.SectorId, stat.CellId }
                into g
                   select new TopDrop2GTrend
                   {
                       BtsId = g.Key.BtsId,
                       SectorId = g.Key.SectorId,
                       CellId = g.Key.CellId,
                       TopDates = g.Count(),
                       TotalDrops = g.Sum(x => x.Drops),
                       TotalCallAttempst = g.Sum(x => x.TrafficAssignmentSuccess),
                       MoAssignmentSuccess = g.Sum(x => x.MoAssignmentSuccess),
                       MtAssignmentSuccess = g.Sum(x => x.MtAssignmentSuccess)
                   };
        }

        public static IEnumerable<TopConnection3GTrend> QueryTrends(this IEnumerable<TopConnection3GCell> stats)
        {
            return from stat in stats
                   group stat by new { stat.BtsId, stat.SectorId, stat.CellId }
                into g
                   select new TopConnection3GTrend
                   {
                       BtsId = g.Key.BtsId,
                       SectorId = g.Key.SectorId,
                       CellId = g.Key.CellId,
                       TopDates = g.Count(),
                       WirelessDrop = g.Sum(x => x.WirelessDrop),
                       ConnectionAttempts = g.Sum(x => x.ConnectionAttempts),
                       ConnectionFails = g.Sum(x => x.ConnectionFails),
                       LinkBusyRate = g.Average(x => x.LinkBusyRate)
                   };
        }
    }
}
