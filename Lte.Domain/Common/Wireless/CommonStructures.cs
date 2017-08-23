﻿using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Common.Geo;
using Lte.Domain.Regular;
using Lte.Domain.Regular.Attributes;

namespace Lte.Domain.Common.Wireless
{

    public interface ITownId
    {
        int TownId { get; set; }
    }

    public interface IDistrictTown
    {
        string District { get; set; }

        string Town { get; set; }
    }

    public interface ICityDistrictTown : IDistrictTown
    {
        string City { get; set; }
    }

    public interface ICityDistrict
    {
        string City { get; set; }

        string District { get; set; }
    }

    public interface IArea
    {
        string Area { get; set; }
    }

    public static class CityDistrictQueries
    {
        public static IEnumerable<TDistrictView> Merge<TDistrictView, TTownView>(this IEnumerable<TTownView> townPreciseViews,
            Func<TTownView, TDistrictView> constructView)
            where TDistrictView : ICityDistrict
            where TTownView : class, ICityDistrictTown, new()
        {
            var preciseViews = townPreciseViews as TTownView[] ?? townPreciseViews.ToArray();
            if (!preciseViews.Any()) return null;
            var districts = preciseViews.Select(x => x.District).Distinct();
            var city = preciseViews.ElementAt(0).City;
            return districts.Select(district =>
            {
                var view =
                    constructView(preciseViews.Where(x => x.District == district).ArraySum());
                view.City = city;
                view.District = district;
                return view;
            }).ToList();
        }
    }

    public interface ITown
    {
        string CityName { get; set; }

        string DistrictName { get; set; }

        string TownName { get; set; }
    }

    public interface IENodebId
    {
        int ENodebId { get; set; }
    }

    public interface IENodebName
    {
        string ENodebName { get; set; }
    }

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
        int BtsId { get; set; }
    }

    public class TopCellContainer<TTopCell>
        where TTopCell : IBtsIdQuery
    {
        public TTopCell TopCell { get; set; }

        public string CdmaName { get; set; }

        public string LteName { get; set; }
    }

    public interface ILteCellQuery : IENodebId
    {
        byte SectorId { get; set; }
    }

    public interface ILocalCellQuery : IENodebId
    {
        byte LocalCellId { get; set; }
    }

    public interface ICdmaCellQuery : IBtsIdQuery
    {
        byte SectorId { get; set; }
    }

    public interface IWorkItemCell : ILteCellQuery
    {
        string WorkItemNumber { get; set; }
    }

    public interface IHotSpot
    {
        HotspotType HotspotType { get; set; }

        string HotspotName { get; set; }

        InfrastructureType InfrastructureType { get; set; }
    }

    [TypeDoc("区域内基站小区统计")]
    public class DistrictStat
    {
        [MemberDoc("区")]
        public string District { get; set; }

        [MemberDoc("LTE基站总数")]
        public int TotalLteENodebs { get; set; }

        [MemberDoc("LTE小区总数")]
        public int TotalLteCells { get; set; }

        [MemberDoc("NB-IoT小区总数")]
        public int TotalNbIotCells { get; set; }

        [MemberDoc("CDMA基站总数")]
        public int TotalCdmaBts { get; set; }

        [MemberDoc("CDMA小区总数")]
        public int TotalCdmaCells { get; set; }
    }

    [TypeDoc("区域内LTE室内外小区统计")]
    public class DistrictIndoorStat
    {
        [MemberDoc("区")]
        public string District { get; set; }

        [MemberDoc("室内小区总数")]
        public int TotalIndoorCells { get; set; }

        [MemberDoc("室外小区总数")]
        public int TotalOutdoorCells { get; set; }
    }

    [TypeDoc("区域内LTE小区频段统计")]
    public class DistrictBandClassStat
    {
        [MemberDoc("区")]
        public string District { get; set; }

        [MemberDoc("2.1G频段小区总数")]
        public int Band1Cells { get; set; }

        [MemberDoc("1.8G频段小区总数")]
        public int Band3Cells { get; set; }

        [MemberDoc("800M频段（非NB-IoT）小区总数")]
        public int Band5Cells { get; set; }

        [MemberDoc("800M频段（NB-IoT）小区总数")]
        public int NbIotCells { get; set; }

        [MemberDoc("TDD频段小区总数")]
        public int Band41Cells { get; set; }
    }

    [TypeDoc("镇区小区数统计")]
    public class TownStat
    {
        [MemberDoc("镇")]
        public string Town { get; set; }

        [MemberDoc("LTE基站总数")]
        public int TotalLteENodebs { get; set; }

        [MemberDoc("LTE小区总数")]
        public int TotalLteCells { get; set; }

        [MemberDoc("NB-IoT小区总数")]
        public int TotalNbIotCells { get; set; }

        [MemberDoc("CDMA基站总数")]
        public int TotalCdmaBts { get; set; }

        [MemberDoc("CDMA小区总数")]
        public int TotalCdmaCells { get; set; }
    }

}
