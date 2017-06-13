﻿using System;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;
using Lte.Parameters.Abstract.Infrastructure;
using Lte.Parameters.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lte.Parameters.Abstract.Kpi;
using Lte.Parameters.Entities.Kpi;

namespace Lte.Parameters.Concrete.Kpi
{
    public class EFRegionRepository : EfRepositoryBase<EFParametersContext, OptimizeRegion>, IRegionRepository
    {
        public List<OptimizeRegion> GetAllList(string city)
        {
            return GetAllList(x => x.City == city);
        }

        public async Task<List<OptimizeRegion>> GetAllListAsync(string city)
        {
            return await GetAllListAsync(x => x.City == city);
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public EFRegionRepository(IDbContextProvider<EFParametersContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }

    public class EFAlarmRepository : EfRepositoryBase<EFParametersContext, AlarmStat>, IAlarmRepository
    {
        public List<AlarmStat> GetAllList(DateTime begin, DateTime end)
        {
            return GetAllList(x => x.HappenTime >= begin && x.HappenTime < end);
        }

        public List<AlarmStat> GetAllList(DateTime begin, DateTime end, int eNodebId)
        {
            return GetAllList(x => x.HappenTime >= begin && x.HappenTime < end && x.ENodebId == eNodebId);
        }

        public List<AlarmStat> GetAllList(DateTime begin, DateTime end, int eNodebId, byte sectorId)
        {
            return
                GetAllList(
                    x => x.HappenTime >= begin && x.HappenTime < end && x.ENodebId == eNodebId && x.SectorId == sectorId);
        }

        public int Count(DateTime begin, DateTime end, int eNodebId)
        {
            return Count(x => x.HappenTime >= begin && x.HappenTime < end && x.ENodebId == eNodebId);
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public EFAlarmRepository(IDbContextProvider<EFParametersContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }

}
