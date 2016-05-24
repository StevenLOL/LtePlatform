﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.MongoDb;
using Abp.MongoDb.Repositories;
using Lte.Parameters.Abstract.Kpi;
using Lte.Parameters.Entities.Mr;
using MongoDB.Bson;

namespace Lte.Parameters.Concrete.Mr
{
    public class CellDistanceRepository : MongoDbRepositoryBase<CellDistance, ObjectId>, ICellDistanceRepository
    {
        public CellDistanceRepository(IMongoDatabaseProvider databaseProvider) : base(databaseProvider)
        {
            CollectionName = "CellDistanceDistribtute";
        }

        public CellDistanceRepository() : this(new MyMongoProvider("yaoyq"))
        {

        }

        public List<CellDistance> GetTotalList(int eNodebId, short pci, DateTime date)
        {
            var nextDate = date.AddDays(1);
            var query =
                MongoDB.Driver.Builders.Query<CellDistance>.Where(
                    e => e.ENodebId == eNodebId && e.PciInfo == pci + "总数"
                    && e.CurrentDate >= date && e.CurrentDate < nextDate);
            return Collection.Find(query).AsQueryable().ToList();
        }

        public List<CellDistance> GetRsrpList(int eNodebId, short pci, DateTime date)
        {
            var nextDate = date.AddDays(1);
            var query =
                MongoDB.Driver.Builders.Query<CellDistance>.Where(
                    e => e.ENodebId == eNodebId && e.PciInfo == pci + ";rsrp"
                    && e.CurrentDate >= date && e.CurrentDate < nextDate);
            return Collection.Find(query).AsQueryable().ToList();
        }

        public List<CellDistance> Get110List(int eNodebId, short pci, DateTime date)
        {
            var nextDate = date.AddDays(1);
            var query =
                MongoDB.Driver.Builders.Query<CellDistance>.Where(
                    e => e.ENodebId == eNodebId && e.PciInfo == pci + ";110"
                    && e.CurrentDate >= date && e.CurrentDate < nextDate);
            return Collection.Find(query).AsQueryable().ToList();
        }

        public List<CellDistance> Get105List(int eNodebId, short pci, DateTime date)
        {
            var nextDate = date.AddDays(1);
            var query =
                MongoDB.Driver.Builders.Query<CellDistance>.Where(
                    e => e.ENodebId == eNodebId && e.PciInfo == pci + ";105"
                    && e.CurrentDate >= date && e.CurrentDate < nextDate);
            return Collection.Find(query).AsQueryable().ToList();
        }
    }
}