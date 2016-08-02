﻿using System.Collections.Generic;
using System.Web.Http;
using Lte.Evaluations.DataService;
using Lte.Evaluations.DataService.Basic;
using Lte.Evaluations.MapperSerive;
using Lte.Evaluations.MapperSerive.Kpi;
using Lte.Evaluations.ViewModels;
using Lte.Evaluations.ViewModels.Basic;
using Lte.Evaluations.ViewModels.Precise;
using Lte.Parameters.Entities;
using Lte.Parameters.Entities.Basic;
using LtePlatform.Models;

namespace LtePlatform.Controllers.Parameters
{
    [ApiControl("LTE小区有关的控制器")]
    public class CellController : ApiController
    {
        private readonly CellService _service;

        public CellController(CellService service)
        {
            _service = service;
        }

        [HttpGet]
        [ApiDoc("获取经纬度范围内的小区列表")]
        [ApiParameterDoc("west", "西边经度")]
        [ApiParameterDoc("east", "东边经度")]
        [ApiParameterDoc("south", "南边纬度")]
        [ApiParameterDoc("north", "北边纬度")]
        [ApiResponse("经纬度范围内的小区列表")]
        public IEnumerable<Cell> Get(double west, double east, double south, double north)
        {
            return _service.GetCells(west, east, south, north);
        }

        [HttpGet]
        [ApiDoc("给定基站编号和扇区编号查询LTE小区")]
        [ApiParameterDoc("eNodebId", "基站编号")]
        [ApiParameterDoc("sectorId", "扇区编号")]
        [ApiResponse("LTE小区，如果找不到则会返回错误")]
        public IHttpActionResult Get(int eNodebId, byte sectorId)
        {
            var item = _service.GetCell(eNodebId, sectorId);
            return item == null ? (IHttpActionResult) BadRequest("The cell cannot be found!") : Ok(item);
        }

        [HttpGet]
        [ApiDoc("给定基站编号和扇区编号、邻区PCI查询LTE可能的邻区")]
        [ApiParameterDoc("eNodebId", "基站编号")]
        [ApiParameterDoc("sectorId", "扇区编号")]
        [ApiParameterDoc("pci", "邻区PCI")]
        [ApiResponse("LTE可能的邻区")]
        public IEnumerable<CellView> Get(int eNodebId, byte sectorId, short pci)
        {
            return _service.GetNearbyCellsWithPci(eNodebId, sectorId, pci);
        }

        [HttpGet]
        [ApiDoc("给定基站编号和扇区编号查询LTE小区")]
        [ApiParameterDoc("eNodebId", "基站编号")]
        [ApiParameterDoc("sectorId", "扇区编号（模糊匹配）")]
        [ApiResponse("LTE小区，如果找不到则会返回错误")]
        public IHttpActionResult GetLocal(int eNodebId, byte localSector)
        {
            var item = _service.GetCell(eNodebId, localSector) ?? _service.GetCell(eNodebId, (byte)(localSector + 48));
            return item == null ? (IHttpActionResult)BadRequest("The cell cannot be found!") : Ok(item);
        }

        [HttpGet]
        [ApiDoc("给定基站名对应的小区扇区编号列表")]
        [ApiParameterDoc("eNodebName", "基站名")]
        [ApiResponse("对应的小区扇区编号列表，如果找不到则会返回错误")]
        public IHttpActionResult Get(string eNodebName)
        {
            var query = _service.GetSectorIds(eNodebName);
            return query == null ? (IHttpActionResult)BadRequest("Wrong ENodeb Name!") : Ok(query);
        }

        [HttpGet]
        [ApiDoc("给定基站编号对应的小区视图列表")]
        [ApiParameterDoc("cellId", "基站编号")]
        [ApiResponse("基站编号对应的小区视图列表")]
        public IEnumerable<CellView> GetViews(int cellId)
        {
            return _service.GetCellViews(cellId);
        }

        [HttpGet]
        [ApiDoc("给定基站编号对定的扇区视图对象列表")]
        [ApiParameterDoc("eNodebId", "基站编号")]
        [ApiResponse("对定的扇区视图对象列表")]
        public IEnumerable<SectorView> Get(int eNodebId)
        {
            return _service.QuerySectors(eNodebId);
        }

        [HttpPost]
        [ApiDoc("将Top精确覆盖率4G小区视图列表转化为便于地理化显示的Top精确覆盖率4G扇区列表")]
        [ApiParameterDoc("container", "Top精确覆盖率4G小区视图列表")]
        [ApiResponse("Top精确覆盖率4G扇区列表")]
        public IEnumerable<Precise4GSector> Post(TopPreciseViewContainer container)
        {
            return _service.QuerySectors(container);
        }
    }

    [ApiControl("CDMA小区有关的控制器")]
    public class CdmaCellController : ApiController
    {
        private readonly CdmaCellService _service;

        public CdmaCellController(CdmaCellService service)
        {
            _service = service;
        }

        [HttpGet]
        [ApiDoc("给定基站编号对定的扇区视图对象列表")]
        [ApiParameterDoc("btsId", "基站编号")]
        [ApiResponse("对定的扇区视图对象列表")]
        public IEnumerable<SectorView> Get(int btsId)
        {
            return _service.QuerySectors(btsId);
        }

        [HttpGet]
        [ApiDoc("给定基站名对应的小区扇区编号列表")]
        [ApiParameterDoc("btsName", "基站名")]
        [ApiResponse("对应的小区扇区编号列表，如果找不到则会返回错误")]
        public IHttpActionResult Get(string btsName)
        {
            var query = _service.GetSectorIds(btsName);
            return query == null ? (IHttpActionResult)BadRequest("Wrong ENodeb Name!") : Ok(query);
        }

        [HttpGet]
        [ApiDoc("给定基站名对应的小区列表")]
        [ApiParameterDoc("name", "基站名")]
        [ApiResponse("对应的小区列表，如果找不到则会返回错误")]
        public IHttpActionResult GetViews(string name)
        {
            var query = _service.GetCellViews(name);
            return query == null ? (IHttpActionResult)BadRequest("Wrong ENodeb Name!") : Ok(query);
        }

        [HttpGet]
        [ApiDoc("给定基站编号和扇区编号查询CDMA复合小区（同时包括1X和DO信息）")]
        [ApiParameterDoc("btsId", "基站编号")]
        [ApiParameterDoc("sectorId", "扇区编号")]
        [ApiResponse("CDMA小区，如果找不到则会返回错误")]
        public IHttpActionResult Get(int btsId, byte sectorId)
        {
            var item = _service.QueryCell(btsId, sectorId);
            return item == null ? (IHttpActionResult)BadRequest("The cell cannot be found!") : Ok(item);
        }

        [HttpGet]
        [ApiDoc("给定基站编号和扇区编号以及小区类型查询CDMA复合小区")]
        [ApiParameterDoc("btsId", "基站编号")]
        [ApiParameterDoc("sectorId", "扇区编号")]
        [ApiParameterDoc("cellType", "小区类型")]
        [ApiResponse("CDMA小区，如果找不到则会返回错误")]
        public IHttpActionResult Get(int btsId, byte sectorId, string cellType)
        {
            var item = _service.QueryCell(btsId, sectorId, cellType);
            return item == null ? (IHttpActionResult)BadRequest("The cell cannot be found!") : Ok(item);
        }
    }
}