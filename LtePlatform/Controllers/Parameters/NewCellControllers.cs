﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Lte.Domain.Common;
using Lte.Evaluations.DataService.Basic;
using Lte.Evaluations.DataService.Dump;
using LtePlatform.Models;

namespace LtePlatform.Controllers.Parameters
{
    [ApiControl("新LTE Excel信息查询数据库")]
    public class NewCellExcelsController : ApiController
    {
        private readonly BasicImportService _service;
        private readonly CellDumpService _dumpService;

        public NewCellExcelsController(BasicImportService service, CellDumpService dumpService)
        {
            _service = service;
            _dumpService = dumpService;
        }

        [HttpGet]
        [ApiDoc("查询待导入的小区Excel信息列表")]
        [ApiResponse("待导入的小区Excel信息列表")]
        public IEnumerable<CellExcel> Get()
        {
            return _service.GetNewCellExcels();
        }

        [HttpPost]
        [ApiDoc("导入新的小区信息")]
        [ApiParameterDoc("container", "新的小区信息列表容器")]
        [ApiResponse("成功导入条数")]
        public int Post(NewCellListContainer container)
        {
            _dumpService.UpdateENodebBtsIds(container.Infos);
            return _dumpService.DumpNewCellExcels(container.Infos);
        }
    }

    public class DumpLteRruController : ApiController
    {
        private readonly CellDumpService _service;

        public DumpLteRruController(CellDumpService service)
        {
            _service = service;
        }

        [HttpPut]
        public async Task<int> Put()
        {
            return await _service.ImportRru(BasicImportService.CellExcels);
        }

        [HttpPost]
        public async Task<int> Post()
        {
            return await _service.UpdateCells(BasicImportService.CellExcels);
        }

        [HttpGet]
        public int Get()
        {
            return BasicImportService.CellExcels.Count;
        }
    }

    [ApiControl("新增CDMA小区的EXCEL记录的控制器，处理的逻辑是先上传解析EXCEL文件，然后从服务器获取解析结果，浏览器处用户对数据进行手工处理，然后批量或逐个上传")]
    public class NewCdmaCellExcelsController : ApiController
    {
        private readonly BasicImportService _service;
        private readonly CdmaCellDumpService _dumpService;

        public NewCdmaCellExcelsController(BasicImportService service, CdmaCellDumpService dumpService)
        {
            _service = service;
            _dumpService = dumpService;
        }

        [HttpGet]
        [ApiDoc("从服务器获取待新增的CDMA小区的EXCEL记录集合，假设之前已经向服务器上传了EXCEL文件并完成了解析")]
        [ApiResponse("待新增的CDMA小区的EXCEL记录集合")]
        public IEnumerable<CdmaCellExcel> Get()
        {
            return _service.GetNewCdmaCellExcels();
        }

        [HttpPost]
        [ApiDoc("批量上传CDMA小区信息到数据库")]
        [ApiParameterDoc("container", "包含待新增的CDMA小区的EXCEL记录集合的容器，由于WebApi不支持直接POST集合，需要将其包装成容器")]
        [ApiResponse("上传成功的数量")]
        public int Post(NewCdmaCellListContainer container)
        {
            return _dumpService.DumpNewCellExcels(container.Infos);
        }
    }

    public class DumpCdmaRruController : ApiController
    {
        private readonly CdmaCellDumpService _service;

        public DumpCdmaRruController(CdmaCellDumpService service)
        {
            _service = service;
        }

        [HttpPut]
        public async Task<int> Put()
        {
            return await _service.ImportRru(BasicImportService.CdmaCellExcels);
        }

        [HttpGet]
        public int Get()
        {
            return BasicImportService.CdmaCellExcels.Count;
        }
    }
}