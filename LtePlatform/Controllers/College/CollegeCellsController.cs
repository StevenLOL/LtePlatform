﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Lte.Evaluations.DataService.College;
using Lte.Evaluations.MapperSerive.Infrastructure;
using Lte.Evaluations.ViewModels;
using Lte.Evaluations.ViewModels.Basic;
using LtePlatform.Models;

namespace LtePlatform.Controllers.College
{
    [ApiControl("查询校园网LTE小区的控制器")]
    public class CollegeCellsController : ApiController
    {
        private readonly CollegeCellsService _service;
        private readonly CollegeCellViewService _viewService;

        public CollegeCellsController(CollegeCellsService service, CollegeCellViewService viewService)
        {
            _service = service;
            _viewService = viewService;
        }

        [HttpGet]
        [ApiDoc("查询校园网LTE小区")]
        [ApiParameterDoc("collegeName", "校园名称")]
        [ApiResponse("校园网LTE小区列表")]
        public IEnumerable<CellView> Get(string collegeName)
        {
            return _viewService.GetViews(collegeName);
        }

        [HttpPost]
        [ApiDoc("查询多个校园对应的LTE小区扇区列表（可用于地理化显示）")]
        [ApiParameterDoc("collegeNames", "校园名称列表")]
        [ApiResponse("LTE小区扇区列表（可用于地理化显示）")]
        public IEnumerable<SectorView> Post(CollegeNamesContainer collegeNames)
        {
            return _service.Query(collegeNames.Names);
        }
    }

    public class CollegeCellContainerController : ApiController
    {
        private CollegeCellsService _service;

        public CollegeCellContainerController(CollegeCellsService serive)
        {
            _service = serive;
        }

        [HttpPost]
        public async Task<int> Post(CollegeCellNamesContainer container)
        {
            return await _service.UpdateCells(container);
        } 
    }

    [ApiControl("查询校园网CDMA小区的控制器")]
    public class CollegeCdmaCellsController : ApiController
    {
        private readonly CollegeCdmaCellsService _service;
        private readonly CollegeCdmaCellViewService _viewService;

        public CollegeCdmaCellsController(CollegeCdmaCellsService service, CollegeCdmaCellViewService viewService)
        {
            _service = service;
            _viewService = viewService;
        }

        [HttpGet]
        [ApiDoc("查询校园网CDMA小区列表")]
        [ApiParameterDoc("collegeName", "校园名称")]
        [ApiResponse("校园网CDMA小区列表")]
        public IEnumerable<CdmaCellView> Get(string collegeName)
        {
            return _viewService.GetViews(collegeName);
        }

        [HttpPost]
        [ApiDoc("查询多个校园对应的CDMA小区扇区列表（可用于地理化显示）")]
        [ApiParameterDoc("collegeNames", "校园名称列表")]
        [ApiResponse("CDMA小区扇区列表（可用于地理化显示）")]
        public IEnumerable<SectorView> Post(CollegeNamesContainer collegeNames)
        {
            return _service.Query(collegeNames.Names);
        }
    }
}
