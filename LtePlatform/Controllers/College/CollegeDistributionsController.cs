﻿using System.Collections.Generic;
using System.Web.Http;
using Lte.Evaluations.DataService.College;
using Lte.Evaluations.MapperSerive.Infrastructure;
using Lte.Parameters.Entities;
using LtePlatform.Models;

namespace LtePlatform.Controllers.College
{
    [ApiControl("查询校园网CDMA室内分布的控制器")]
    public class CollegeCdmaDistributionsController : ApiController
    {
        private readonly CollegeLteDistributionService _service;
        private readonly CollegeCdmaDistributionService _cdmaService;

        public CollegeCdmaDistributionsController(CollegeLteDistributionService service,
            CollegeCdmaDistributionService cdmaService)
        {
            _service = service;
            _cdmaService = cdmaService;
        }

        [HttpGet]
        [ApiDoc("查询校园网CDMA室内分布列表")]
        [ApiParameterDoc("collegeName", "校园名称")]
        [ApiResponse("校园网CDMA室内分布列表")]
        public IEnumerable<IndoorDistribution> Get(string collegeName)
        {
            return _cdmaService.Query(collegeName);
        }

        [HttpPost]
        [ApiDoc("查询多个校园对应的LTE室内分布列表（可用于地理化显示）")]
        [ApiParameterDoc("collegeNames", "校园名称列表")]
        [ApiResponse("LTE室内分布列表（可用于地理化显示）")]
        public IEnumerable<IndoorDistribution> Post(CollegeNamesContainer collegeNames)
        {
            return _service.Query(collegeNames.Names);
        }
    }

    [ApiControl("查询校园网LTE室内分布的控制器")]
    public class CollegeLteDistributionsController : ApiController
    {
        private readonly CollegeLteDistributionService _service;

        public CollegeLteDistributionsController(CollegeLteDistributionService service)
        {
            _service = service;
        }

        [HttpGet]
        [ApiDoc("查询校园网LTE室内分布列表")]
        [ApiParameterDoc("collegeName", "校园名称")]
        [ApiResponse("校园网LTE室内分布列表")]
        public IEnumerable<IndoorDistribution> Get(string collegeName)
        {
            return _service.Query(collegeName);
        }

        [HttpPost]
        [ApiDoc("查询多个校园对应的LTE室内分布列表（可用于地理化显示）")]
        [ApiParameterDoc("collegeNames", "校园名称列表")]
        [ApiResponse("LTE室内分布列表（可用于地理化显示）")]
        public IEnumerable<IndoorDistribution> Post(CollegeNamesContainer collegeNames)
        {
            return _service.Query(collegeNames.Names);
        }
    }
}