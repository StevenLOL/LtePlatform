﻿using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;
using Lte.Parameters.Abstract.Kpi;
using Lte.Parameters.Entities.Kpi;
using System;
using System.Collections.Generic;
using System.Linq;
using Abp.EntityFramework.Dependency;
using MongoDB.Bson;

namespace Lte.Parameters.Concrete.Kpi
{
    public class EFPreciseCoverage4GRepository : EfRepositoryBase<EFParametersContext, PreciseCoverage4G>,
        IPreciseCoverage4GRepository
    {
        public List<PreciseCoverage4G> GetAllList(DateTime begin, DateTime end)
        {
            return GetAllList(x => x.StatTime >= begin && x.StatTime < end);
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public List<PreciseCoverage4G> GetAllList(int cellId, byte sectorId, DateTime begin, DateTime end)
        {
            return GetAllList(x =>
                x.StatTime >= begin && x.StatTime < end && x.CellId == cellId && x.SectorId == sectorId);
        }

        public EFPreciseCoverage4GRepository(IDbContextProvider<EFParametersContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }

    public class EFTownPreciseCoverage4GStatRepository : EfRepositoryBase<EFParametersContext, TownPreciseCoverage4GStat>,
        ITownPreciseCoverage4GStatRepository
    {
        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public EFTownPreciseCoverage4GStatRepository(IDbContextProvider<EFParametersContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }

    public class PreciseMongoRepository : MongoDbRepositoryBase<PreciseMongo, ObjectId>, IPreciseMongoRepository
    {
        public PreciseMongoRepository(IMongoDatabaseProvider databaseProvider) : base(databaseProvider)
        {
            CollectionName = "precise_combined";
        }

        public PreciseMongoRepository() : this(new MyMongoProvider("ouyh"))
        {

        }

        public List<PreciseMongo> GetAllList(DateTime statDate)
        {
            var nextDate = statDate.AddDays(1);
            var query =
                MongoDB.Driver.Builders.Query<InterferenceMatrixMongo>.Where(e =>
                    e.StatDate >= statDate && e.StatDate < nextDate);
            return Collection.Find(query).AsQueryable().ToList();
        }
    }
}
