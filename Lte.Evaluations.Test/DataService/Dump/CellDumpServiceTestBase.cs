﻿using Abp.EntityFramework.AutoMapper;
using Abp.Reflection;
using Lte.Evaluations.MapperSerive;
using Lte.Evaluations.MockItems;
using Lte.Evaluations.Policy;
using Lte.Parameters.Abstract.Basic;
using Lte.Parameters.Entities.Basic;
using Lte.Parameters.MockOperations;
using Moq;
using NUnit.Framework;

namespace Lte.Evaluations.DataService.Dump
{
    public abstract class CellDumpServiceTestBase
    {
        protected readonly Mock<IBtsRepository> BtsRepository = new Mock<IBtsRepository>();
        protected readonly Mock<ICellRepository> CellRepository = new Mock<ICellRepository>();
        protected CellDumpService Service;
        private readonly ITypeFinder _typeFinder = new TypeFinder(new MyAssemblyFinder());

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            Service = new CellDumpService(BtsRepository.Object, CellRepository.Object, null);
            BtsRepository.MockOperation();
            BtsRepository.MockGetId<IBtsRepository, CdmaBts>();
            BtsRepository.MockThreeBtss();
            CellRepository.MockRepositorySaveItems<Cell, ICellRepository>();
            var module = new AbpAutoMapperModule(_typeFinder);
            module.PostInitialize();
        }

    }
}
