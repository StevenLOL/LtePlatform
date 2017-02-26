﻿using Abp.EntityFramework.AutoMapper;
using Lte.Domain.Common;
using Lte.Domain.Regular.Attributes;
using Lte.MySqlFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Common.Geo;

namespace Lte.Evaluations.ViewModels.Kpi
{
    [AutoMapFrom(typeof(FlowHuawei), typeof(FlowZte))]
    [TypeDoc("小区单日流量统计视图")]
    public class FlowView
    {
        [MemberDoc("统计时间")]
        public DateTime StatTime { get; set; }

        [MemberDoc("基站编号")]
        public int ENodebId { get; set; }

        [MemberDoc("扇区编号")]
        public byte SectorId { get; set; }

        [AutoMapPropertyResolve("DownlinkPdcpFlow", typeof(FlowZte))]
        [MemberDoc("PDCP层下行流量")]
        public double PdcpDownlinkFlow { get; set; }

        [AutoMapPropertyResolve("UplindPdcpFlow", typeof(FlowZte))]
        [MemberDoc("PDCP层上行流量")]
        public double PdcpUplinkFlow { get; set; }

        [AutoMapPropertyResolve("AverageRrcUsers", typeof(FlowZte))]
        [MemberDoc("平均用户数")]
        public double AverageUsers { get; set; }

        [AutoMapPropertyResolve("MaxRrcUsers", typeof(FlowZte))]
        [MemberDoc("最大用户数")]
        public int MaxUsers { get; set; }

        [MemberDoc("平均激活用户数")]
        public double AverageActiveUsers { get; set; }

        [MemberDoc("最大激活用户数")]
        public int MaxActiveUsers { get; set; }

        public double DownlinkFeelingThroughput { get; set; }

        public double DownlinkFeelingDuration { get; set; }

        public double DownlinkFeelingRate
            => DownlinkFeelingDuration == 0 ? 0 : DownlinkFeelingThroughput/DownlinkFeelingDuration;

        public double UplinkFeelingThroughput { get; set; }

        public double UplinkFeelingDuration { get; set; }

        public double UplinkFeelingRate
            => UplinkFeelingDuration == 0 ? 0 : UplinkFeelingThroughput/UplinkFeelingDuration;

        public static FlowView Average(IEnumerable<FlowView> views)
        {
            if (views == null || !views.Any()) return null;
            var first = views.FirstOrDefault();
            return new FlowView
            {
                StatTime = first.StatTime,
                ENodebId = first.ENodebId,
                SectorId = first.SectorId,
                PdcpUplinkFlow = views.Average(x => x.PdcpUplinkFlow),
                PdcpDownlinkFlow = views.Average(x => x.PdcpDownlinkFlow),
                AverageUsers = views.Average(x => x.AverageUsers),
                MaxUsers = views.Max(x => x.MaxUsers),
                AverageActiveUsers = views.Average(x => x.AverageActiveUsers),
                MaxActiveUsers = views.Max(x => x.MaxActiveUsers),
                DownlinkFeelingDuration = views.Average(x=>x.DownlinkFeelingDuration),
                DownlinkFeelingThroughput = views.Average(x=>x.DownlinkFeelingDuration),
                UplinkFeelingDuration = views.Average(x=>x.UplinkFeelingDuration),
                UplinkFeelingThroughput = views.Average(x=>x.UplinkFeelingThroughput)
            };
        }
    }

    [AutoMapFrom(typeof(FlowHuawei), typeof(FlowZte))]
    public class ENodebFlowView : IENodebId, IGeoPoint<double>
    {
        public int ENodebId { get; set; }

        public double Longtitute { get; set; }

        public double Lattitute { get; set; }

        [AutoMapPropertyResolve("DownlinkPdcpFlow", typeof(FlowZte))]
        [MemberDoc("PDCP层下行流量")]
        public double PdcpDownlinkFlow { get; set; }

        [AutoMapPropertyResolve("UplindPdcpFlow", typeof(FlowZte))]
        [MemberDoc("PDCP层上行流量")]
        public double PdcpUplinkFlow { get; set; }

        [AutoMapPropertyResolve("AverageRrcUsers", typeof(FlowZte))]
        [MemberDoc("平均用户数")]
        public double AverageUsers { get; set; }

        [AutoMapPropertyResolve("MaxRrcUsers", typeof(FlowZte))]
        [MemberDoc("最大用户数")]
        public int MaxUsers { get; set; }

        [MemberDoc("平均激活用户数")]
        public double AverageActiveUsers { get; set; }

        [MemberDoc("最大激活用户数")]
        public int MaxActiveUsers { get; set; }

        public double DownlinkFeelingThroughput { get; set; }

        public double DownlinkFeelingDuration { get; set; }

        public double DownlinkFeelingRate
            => DownlinkFeelingDuration == 0 ? 0 : DownlinkFeelingThroughput / DownlinkFeelingDuration;

        public double UplinkFeelingThroughput { get; set; }

        public double UplinkFeelingDuration { get; set; }

        public double UplinkFeelingRate
            => UplinkFeelingDuration == 0 ? 0 : UplinkFeelingThroughput / UplinkFeelingDuration;

    }

    [AutoMapFrom(typeof(FlowView))]
    [TypeDoc("聚合流量统计视图")]
    public class AggregateFlowView
    {
        [MemberDoc("小区个数")]
        public int CellCount { get; set; }

        [MemberDoc("PDCP层下行流量")]
        public double PdcpDownlinkFlow { get; set; }

        [MemberDoc("PDCP层上行流量")]
        public double PdcpUplinkFlow { get; set; }

        [MemberDoc("平均用户数")]
        public double AverageUsers { get; set; }

        [MemberDoc("最大用户数")]
        public int MaxUsers { get; set; }

        [MemberDoc("平均激活用户数")]
        public double AverageActiveUsers { get; set; }

        [MemberDoc("最大激活用户数")]
        public int MaxActiveUsers { get; set; }

        public double DownlinkFeelingThroughput { get; set; }

        public double DownlinkFeelingDuration { get; set; }

        public double DownlinkFeelingRate
            => DownlinkFeelingDuration == 0 ? 0 : DownlinkFeelingThroughput / DownlinkFeelingDuration;

        public double UplinkFeelingThroughput { get; set; }

        public double UplinkFeelingDuration { get; set; }

        public double UplinkFeelingRate
            => UplinkFeelingDuration == 0 ? 0 : UplinkFeelingThroughput / UplinkFeelingDuration;
    }
}
