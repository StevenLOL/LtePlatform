﻿using System.Collections.Generic;
using System.Linq;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;
using Lte.Parameters.Abstract.Infrastructure;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Concrete.Infrastructure
{
    public class EFInfrastructureRepository : EfRepositoryBase<EFParametersContext, InfrastructureInfo>, IInfrastructureRepository
    {
        public IEnumerable<int> GetCollegeInfrastructureIds(string collegeName, InfrastructureType type)
        {
            return GetAll().Where(x =>
                x.HotspotName == collegeName && x.InfrastructureType == type && x.HotspotType == HotspotType.College
                ).Select(x => x.InfrastructureId).ToList();
        }
        
        public InfrastructureInfo GetTopPreciseMonitor(int id)
        {
            return FirstOrDefault(x => x.InfrastructureId == id && x.HotspotType == HotspotType.TopPrecise);
        }

        public List<InfrastructureInfo> GetAllPreciseMonitor()
        {
            return GetAll().Where(x => x.HotspotType == HotspotType.TopPrecise).ToList();
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public EFInfrastructureRepository(IDbContextProvider<EFParametersContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }

    public class EFIndoorDistributionRepository
        : EfRepositoryBase<EFParametersContext, IndoorDistribution>, IIndoorDistributionRepository
    {
        public EFIndoorDistributionRepository(IDbContextProvider<EFParametersContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }
    }
}
