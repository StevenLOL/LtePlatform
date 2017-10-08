﻿using Lte.Domain.Common.Wireless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Lte.Domain.Common;
using Lte.MySqlFramework.Abstract;
using Lte.MySqlFramework.Entities;
using Lte.Parameters.Abstract.Basic;
using Lte.Parameters.Entities.Basic;

namespace Lte.Evaluations.Policy
{
    public static class FilterWorkItemService
    {
        public static Expression<Func<WorkItem, bool>> GetWorkItemFilter(this string filerDescription)
        {
            switch (filerDescription)
            {
                case "未完成_全部":
                    return x => x.State != WorkItemState.Finished;
                case "全部_2/3G":
                    return x => x.Type == WorkItemType.Kpi2G || x.Type == WorkItemType.NetworkProblem;
                case "全部_4G":
                    return
                        x =>
                            x.Type == WorkItemType.Infrastructure4G || x.Type == WorkItemType.Interference4G ||
                            x.Type == WorkItemType.Kpi4G || x.Type == WorkItemType.RrcConnection;
                case "全部_考核部分":
                    return
                        x =>
                            x.Type == WorkItemType.Interference4G || x.Type == WorkItemType.Kpi4G ||
                            x.Type == WorkItemType.Kpi2G;
                case "全部_作业计划":
                    return
                        x => x.Type == WorkItemType.DailyTask;
                case "未完成_2/3G":
                    return
                        x =>
                            x.State != WorkItemState.Finished &&
                            (x.Type == WorkItemType.Kpi2G || x.Type == WorkItemType.NetworkProblem);
                case "未完成_4G":
                    return
                        x =>
                            x.State != WorkItemState.Finished &&
                            (x.Type == WorkItemType.Infrastructure4G || x.Type == WorkItemType.Interference4G ||
                             x.Type == WorkItemType.Kpi4G || x.Type == WorkItemType.RrcConnection);
                case "未完成_考核部分":
                    return
                        x =>
                            x.State != WorkItemState.Finished &&
                            (x.Type == WorkItemType.Interference4G || x.Type == WorkItemType.Kpi4G ||
                            x.Type == WorkItemType.Kpi2G);
                case "未完成_作业计划":
                    return
                        x => x.State != WorkItemState.Finished && x.Type == WorkItemType.DailyTask;
                default:
                    return null;
            }
        }

        public static List<ENodeb> QueryENodebs(this ITownRepository townRepository, IENodebRepository eNodebRepository,
            string city, string district)
        {
            var towns = townRepository.GetAllList(x => x.CityName == city && x.DistrictName == district);
            if (!towns.Any())
            {
                return new List<ENodeb>();
            }
            return (from eNodeb in eNodebRepository.GetAllList()
                    join town in towns on eNodeb.TownId equals town.Id
                    select eNodeb).ToList();
        }

        public static ENodeb ConstructENodeb(this ENodebExcel info, ITownRepository repository)
        {
            var town = repository.GetAllList()
                    .FirstOrDefault(x => x.CityName == info.CityName && x.DistrictName == info.DistrictName && x.TownName == info.TownName);
            var eNodeb = Mapper.Map<ENodebExcel, ENodeb>(info);
            eNodeb.TownId = town?.Id ?? -1;
            return eNodeb;
        }

        public static CdmaBts ConstructBts(this BtsExcel info, ITownRepository repository)
        {
            var town = repository.QueryTown(info.DistrictName, info.TownName);
            var bts = Mapper.Map<BtsExcel, CdmaBts>(info);
            bts.TownId = town?.Id ?? -1;
            return bts;
        }
    }
}
