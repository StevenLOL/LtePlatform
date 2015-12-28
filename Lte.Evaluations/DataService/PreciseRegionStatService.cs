﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Domain.Regular;
using Lte.Evaluations.ViewModels;
using Lte.Evaluations.ViewModels.RegionKpi;
using Lte.Parameters.Abstract;

namespace Lte.Evaluations.DataService
{
    public class PreciseRegionStatService
    {
        private readonly ITownPreciseCoverage4GStatRepository _statRepository;
        private readonly ITownRepository _townRepository;

        public PreciseRegionStatService(ITownPreciseCoverage4GStatRepository statRepository,
            ITownRepository townRepository)
        {
            _statRepository = statRepository;
            _townRepository = townRepository;
        }

        public PreciseRegionDateView QueryLastDateStat(DateTime initialDate, string city)
        {
            var beginDate = initialDate.AddDays(-100);
            var endDate = initialDate.AddDays(1);
            var query =
                _statRepository.GetAllList(beginDate, endDate);
            var result =
                (from q in query
                    join t in _townRepository.GetAll(city) on q.TownId equals t.Id
                    select q).ToList();
            if (result.Count == 0) return null;
            var maxDate = result.Max(x => x.StatTime);
            var townViews =
                result.Where(x => x.StatTime == maxDate)
                    .Select(x => TownPreciseView.ConstructView(x, _townRepository))
                    .ToList();
            return new PreciseRegionDateView
            {
                StatDate = maxDate.ToShortDateString(),
                TownPreciseViews = townViews,
                DistrictPreciseViews = Merge(townViews)
            };
        }

        public IEnumerable<PreciseRegionDateView> QueryDateViews(DateTime begin, DateTime end, string city)
        {
            var query = _statRepository.GetAllList(begin, end);
            var result =
                (from q in query
                 join t in _townRepository.GetAll(city) on q.TownId equals t.Id
                 select q).ToList();
            var townViews = result.Select(x => TownPreciseView.ConstructView(x, _townRepository)).ToList();
            return from view in townViews
                   group view by view.StatTime into g
                   select new PreciseRegionDateView
                   {
                       StatDate = g.Key.ToShortDateString(),
                       TownPreciseViews = g.Select(x => x),
                       DistrictPreciseViews = Merge(g.Select(x => x))
                   };
        }

        public static IEnumerable<DistrictPreciseView> Merge(IEnumerable<TownPreciseView> townPreciseViews)
        {
            if (!townPreciseViews.Any()) return null;
            var districts = townPreciseViews.Select(x => x.District).Distinct();
            var city = townPreciseViews.ElementAt(0).City;
            return districts.Select(district =>
            {
                var view =
                    DistrictPreciseView.ConstructView(townPreciseViews.Where(x => x.District == district).ArraySum());
                view.City = city;
                view.District = district;
                return view;
            }).ToList();
        } 
    }
}
