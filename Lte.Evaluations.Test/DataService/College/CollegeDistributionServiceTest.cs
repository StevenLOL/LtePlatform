﻿using System.Linq;
using Lte.Evaluations.MockItems;
using Lte.Evaluations.TestService;
using Lte.Parameters.Abstract;
using Lte.Parameters.Abstract.Infrastructure;
using Lte.Parameters.Entities;
using Moq;
using NUnit.Framework;

namespace Lte.Evaluations.DataService.College
{
    public class CollegeDistributionServiceTest
    {
        private readonly Mock<IInfrastructureRepository> _repository = new Mock<IInfrastructureRepository>();

        private readonly Mock<IIndoorDistributionRepository> _indoorRepository =
            new Mock<IIndoorDistributionRepository>();

        private CollegeDistributionService _service;
        private CollegeDistributionTestService _testService;

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            _repository.MockOperations();
            _indoorRepository.MockGetId<IIndoorDistributionRepository, IndoorDistribution>();
            _indoorRepository.MockThreeDistributions();
            _service = new CollegeDistributionService(_repository.Object, _indoorRepository.Object);
            _testService = new CollegeDistributionTestService(_repository, _indoorRepository);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void Test_QueryLteDistributions_SingleInfrastructure(int id)
        {
            _testService.MockOneLteDistribution(id);
            var views = _service.QueryLteDistributions("College-" + id);
            Assert.IsNotNull(views);

            if (id > 0 && id <= 3)
            {
                Assert.AreEqual(views.Count(), 1);
            }
            else
            {
                Assert.AreEqual(views.Count(), 0);
            }
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void Test_QueryCdmaDistributions_SingleInfrastructure(int id)
        {
            _testService.MockOneCdmaDistribution(id);
            var views = _service.QueryCdmaDistributions("College-" + id);
            Assert.IsNotNull(views);

            if (id > 0 && id <= 3)
            {
                Assert.AreEqual(views.Count(), 1);
            }
            else
            {
                Assert.AreEqual(views.Count(), 0);
            }
        }
    }
}
