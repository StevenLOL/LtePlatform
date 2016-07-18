﻿using Abp.EntityFramework.AutoMapper;
using Lte.Domain.Common.Wireless;
using Lte.Domain.Regular.Attributes;
using Lte.Parameters.Abstract.Basic;

namespace Lte.Evaluations.ViewModels.Kpi
{
    public class TopDrop2GTrend : IBtsIdQuery
    {
        public int BtsId { get; set; }

        public int CellId { get; set; }

        public byte SectorId { get; set; }

        public int TotalDrops { get; set; }

        public int TotalCallAttempst { get; set; }

        public int TopDates { get; set; }

        public int MoAssignmentSuccess { get; set; }

        public int MtAssignmentSuccess { get; set; }

    }

    [TypeDoc("TOP掉话小区指标趋势视图")]
    [AutoMapFrom(typeof(TopDrop2GTrend))]
    public class TopDrop2GTrendView
    {
        [MemberDoc("小区名称")]
        public string CellName { get; set; }

        [MemberDoc("LTE基站名称")]
        public string ENodebName { get; set; }

        public int BtsId { get; set; }

        public byte SectorId { get; set; }

        public int CellId { get; set; }

        public int MoAssignmentSuccess { get; set; }

        public int MtAssignmentSuccess { get; set; }

        [MemberDoc("掉话总数")]
        public int TotalDrops { get; set; }

        [MemberDoc("呼叫尝试总数")]
        public int TotalCallAttempst { get; set; }

        [MemberDoc("掉话率")]
        public double DropRate => TotalCallAttempst == 0 ? 0 : (double)TotalDrops / TotalCallAttempst;

        [MemberDoc("进入TOP的日期数")]
        public int TopDates { get; set; }
    }
}
