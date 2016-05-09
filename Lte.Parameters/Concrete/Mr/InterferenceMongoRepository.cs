﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.MongoDb;
using Abp.MongoDb.Repositories;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities.Mr;
using MongoDB.Bson;

namespace Lte.Parameters.Concrete.Mr
{
    public class InterferenceMongoRepository : MongoDbRepositoryBase<InterferenceMatrixMongo, ObjectId>, IInterferenceMongoRepository
    {
        public InterferenceMongoRepository(IMongoDatabaseProvider databaseProvider) : base(databaseProvider)
        {
            CollectionName = "CellInterfMatrix";
        }

        public InterferenceMongoRepository() : this(new MyMongoProvider("yaoyq"))
        {
            
        }
        
        public InterferenceMatrixMongo GetOne(int eNodebId, short pci)
        {
            var query =
                MongoDB.Driver.Builders.Query<InterferenceMatrixMongo>.Where(
                    e => e.ENodebId == eNodebId && e.Pci == pci);
            return Collection.FindOne(query);
        }
        
        public List<InterferenceMatrixMongo> GetList(int eNodebId, short pci)
        {
            var query =
                MongoDB.Driver.Builders.Query<InterferenceMatrixMongo>.Where(
                    e => e.ENodebId == eNodebId && e.Pci == pci);
            return Collection.Find(query).AsQueryable().ToList();
        }

        public async Task<List<InterferenceMatrixMongo>> GetListAsync(int eNodebId, short pci)
        {
            var query =
                MongoDB.Driver.Builders.Query<InterferenceMatrixMongo>.Where(
                    e => e.ENodebId == eNodebId && e.Pci == pci);
            return await Collection.Find(query).AsQueryable().ToListAsync();
        }
    }
}
