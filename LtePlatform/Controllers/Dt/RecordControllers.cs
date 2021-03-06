﻿using Lte.Evaluations.DataService;
using Lte.Parameters.Entities.Dt;
using LtePlatform.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Lte.Domain.Common.Wireless;
using Lte.Evaluations.DataService.College;
using Lte.Evaluations.DataService.Mr;
using Lte.MySqlFramework.Entities;

namespace LtePlatform.Controllers.Dt
{
    [ApiControl("2G测试数据查询控制器")]
    public class Record2GController : ApiController
    {
        private readonly CsvFileInfoService _service;

        public Record2GController(CsvFileInfoService service)
        {
            _service = service;
        }

        [ApiDoc("查询指定数据文件的测试数据")]
        [ApiParameterDoc("fileName", "数据文件名")]
        [ApiResponse("指定数据文件的测试数据")]
        public IEnumerable<FileRecord2G> Get(string fileName)
        {
            return _service.GetFileRecord2Gs(fileName);
        }

        [ApiDoc("查询指定数据文件和网格编号的测试数据")]
        [ApiParameterDoc("fileName", "数据文件名")]
        [ApiParameterDoc("rasterNum", "网格编号")]
        [ApiResponse("指定数据文件的测试数据")]
        public IEnumerable<FileRecord2G> Get(string fileName, int rasterNum)
        {
            return _service.GetFileRecord2Gs(fileName, rasterNum);
        }

        [HttpPost]
        [ApiDoc("给定数据文件名称和网格编号列表，查询覆盖指标列表")]
        [ApiParameterDoc("infoView", "包含数据文件名称和网格编号列表的视图")]
        [ApiResponse("覆盖指标列表")]
        public IEnumerable<FileRecordCoverage2G> Post(FileRasterInfoView infoView)
        {
            return _service.GetCoverage2Gs(infoView);
        }
    }

    [ApiControl("2G测试详细数据查询控制器")]
    public class Record2GDetailsController : ApiController
    {
        private readonly CsvFileInfoService _service;

        public Record2GDetailsController(CsvFileInfoService service)
        {
            _service = service;
        }

        [HttpPost]
        [ApiDoc("给定数据文件名称和网格编号列表，查询覆盖指标列表")]
        [ApiParameterDoc("infoView", "包含数据文件名称和网格编号列表的视图")]
        [ApiResponse("覆盖指标列表")]
        public IEnumerable<FileRecord2G> Post(FileRasterInfoView infoView)
        {
            return _service.GetFileRecord2Gs(infoView);
        }
    }

    [ApiControl("3G测试数据查询控制器")]
    public class Record3GController : ApiController
    {
        private readonly CsvFileInfoService _service;

        public Record3GController(CsvFileInfoService service)
        {
            _service = service;
        }

        [ApiDoc("查询指定数据文件的测试数据")]
        [ApiParameterDoc("fileName", "数据文件名")]
        [ApiResponse("指定数据文件的测试数据")]
        public IEnumerable<FileRecord3G> Get(string fileName)
        {
            return _service.GetFileRecord3Gs(fileName);
        }

        [ApiDoc("查询指定数据文件和网格编号的测试数据")]
        [ApiParameterDoc("fileName", "数据文件名")]
        [ApiParameterDoc("rasterNum", "网格编号")]
        [ApiResponse("指定数据文件的测试数据")]
        public IEnumerable<FileRecord3G> Get(string fileName, int rasterNum)
        {
            return _service.GetFileRecord3Gs(fileName, rasterNum);
        }

        [HttpPost]
        [ApiDoc("给定数据文件名称和网格编号列表，查询覆盖指标列表")]
        [ApiParameterDoc("infoView", "包含数据文件名称和网格编号列表的视图")]
        [ApiResponse("覆盖指标列表")]
        public IEnumerable<FileRecordCoverage3G> Post(FileRasterInfoView infoView)
        {
            return _service.GetCoverage3Gs(infoView);
        }
    }

    [ApiControl("3G测试详细数据查询控制器")]
    public class Record3GDetailsController : ApiController
    {
        private readonly CsvFileInfoService _service;

        public Record3GDetailsController(CsvFileInfoService service)
        {
            _service = service;
        }

        [HttpPost]
        [ApiDoc("给定数据文件名称和网格编号列表，查询覆盖指标列表")]
        [ApiParameterDoc("infoView", "包含数据文件名称和网格编号列表的视图")]
        [ApiResponse("覆盖指标列表")]
        public IEnumerable<FileRecord3G> Post(FileRasterInfoView infoView)
        {
            return _service.GetFileRecord3Gs(infoView);
        }
    }

    [ApiControl("4G测试数据查询控制器")]
    public class Record4GController : ApiController
    {
        private readonly CsvFileInfoService _service;

        public Record4GController(CsvFileInfoService service)
        {
            _service = service;
        }

        [ApiDoc("查询指定数据文件的测试数据")]
        [ApiParameterDoc("fileName", "数据文件名")]
        [ApiResponse("指定数据文件的测试数据")]
        public IEnumerable<FileRecord4G> Get(string fileName)
        {
            return _service.GetFileRecord4Gs(fileName);
        }

        [ApiDoc("查询指定数据文件和网格编号的测试数据")]
        [ApiParameterDoc("fileName", "数据文件名")]
        [ApiParameterDoc("rasterNum", "网格编号")]
        [ApiResponse("指定数据文件的测试数据")]
        [HttpGet]
        public IEnumerable<FileRecord4G> Get(string fileName, int rasterNum)
        {
            return _service.GetFileRecord4Gs(fileName, rasterNum);
        }

        [HttpPost]
        [ApiDoc("给定数据文件名称和网格编号列表，查询覆盖指标列表")]
        [ApiParameterDoc("infoView", "包含数据文件名称和网格编号列表的视图")]
        [ApiResponse("覆盖指标列表")]
        public IEnumerable<FileRecordCoverage4G> Post(FileRasterInfoView infoView)
        {
            return _service.GetCoverage4Gs(infoView);
        }
    }

    [ApiControl("4G测试详细数据查询控制器")]
    public class Record4GDetailsController : ApiController
    {
        private readonly CsvFileInfoService _service;

        public Record4GDetailsController(CsvFileInfoService service)
        {
            _service = service;
        }

        [HttpPost]
        [ApiDoc("给定数据文件名称和网格编号列表，查询覆盖指标列表")]
        [ApiParameterDoc("infoView", "包含数据文件名称和网格编号列表的视图")]
        [ApiResponse("覆盖指标列表")]
        public IEnumerable<FileRecord4G> Post(FileRasterInfoView infoView)
        {
            return _service.GetFileRecord4Gs(infoView);
        }
    }

    [ApiControl("网格测试文件信息查询控制器")]
    public class RasterFileController : ApiController
    {
        private readonly RasterInfoService _service;

        public RasterFileController(RasterInfoService service)
        {
            _service = service;
        }

        [HttpGet]
        [ApiDoc("查询包含指定数据类型和坐标范围的所有网格的测试文件信息")]
        [ApiParameterDoc("dataType", "指定数据类型（2G、3G、4G）")]
        [ApiParameterDoc("west", "坐标西界")]
        [ApiParameterDoc("east", "坐标东界")]
        [ApiParameterDoc("south", "坐标南界")]
        [ApiParameterDoc("north", "坐标北界")]
        [ApiResponse("包含指定数据类型的所有网格的测试文件信息视图，包括测试文件编号和包含的网格编号列表")]
        public IEnumerable<FileRasterInfoView> Get(string dataType, double west, double east, double south,
            double north)
        {
            return _service.QueryFileNames(dataType, west, east, south, north);
        }

        [HttpGet]
        [ApiDoc("查询包含指定数据类型和坐标范围的所有网格的测试文件信息")]
        [ApiParameterDoc("dataType", "指定数据类型（2G、3G、4G）")]
        [ApiParameterDoc("west", "坐标西界")]
        [ApiParameterDoc("east", "坐标东界")]
        [ApiParameterDoc("south", "坐标南界")]
        [ApiParameterDoc("north", "坐标北界")]
        [ApiParameterDoc("begin", "开始日期")]
        [ApiParameterDoc("end", "结束日期")]
        [ApiResponse("包含指定数据类型的所有网格的测试文件信息视图，包括测试文件编号和包含的网格编号列表")]
        public IEnumerable<FileRasterInfoView> Get(string dataType, double west, double east, double south,
            double north, DateTime begin, DateTime end)
        {
            return _service.QueryFileNames(dataType, west, east, south, north, begin, end);
        }

        [HttpGet]
        [ApiDoc("查询包含指定数据类型和镇区的日期范围内的测试文件信息")]
        [ApiParameterDoc("dataType", "指定数据类型（2G、3G、4G）")]
        [ApiParameterDoc("townName", "镇区名称")]
        [ApiParameterDoc("begin", "开始日期")]
        [ApiParameterDoc("end", "结束日期")]
        [ApiResponse("包含指定数据类型的日期范围内的测试文件信息视图，包括测试文件编号和包含的网格编号列表")]
        public IEnumerable<FileRasterInfoView> Get(string dataType, string townName, DateTime begin, DateTime end)
        {
            return _service.QueryFileNames(dataType, townName, begin, end);
        }

        [HttpGet]
        [ApiDoc("查询包含指定数据类型和镇区的所有网格的测试文件信息")]
        [ApiParameterDoc("dataType", "指定数据类型（2G、3G、4G）")]
        [ApiParameterDoc("townName", "镇区名称")]
        [ApiResponse("包含指定数据类型的所有网格的测试文件信息视图，包括测试文件编号和包含的网格编号列表")]
        public IEnumerable<FileRasterInfoView> Get(string dataType, string townName)
        {
            return _service.QueryFileNames(dataType, townName);
        }

        [HttpGet]
        [ApiDoc("查询包含指定日期范围内的测试文件信息")]
        [ApiParameterDoc("begin", "开始日期")]
        [ApiParameterDoc("end", "结束日期")]
        [ApiResponse("包含指定日期范围内的测试文件信息")]
        public IEnumerable<CsvFilesInfo> Get(DateTime begin, DateTime end)
        {
            return _service.QureyFileNames(begin, end);
        }

        [HttpGet]
        public string Get(string csvFileName)
        {
            return _service.QueryNetworkType(csvFileName);
        }

        [HttpGet]
        [ApiDoc("测试用API")]
        public IEnumerable<RasterInfo> Get()
        {
            return _service.GetAllList();
        } 
    }

    [ApiControl("DT测试数据文件查询控制器")]
    public class CsvFileInfoController : ApiController
    {
        private readonly CsvFileInfoService _service;

        public CsvFileInfoController(CsvFileInfoService service)
        {
            _service = service;
        }

        [HttpGet]
        [ApiDoc("获得指定日期范围内的DT测试数据文件信息")]
        [ApiParameterDoc("begin", "开始日期")]
        [ApiParameterDoc("end", "结束日期")]
        [ApiResponse("DT测试数据文件信息，包括测试时间、数据名称、存放目录、测试网络（2G3G4G）和测试距离等")]
        public IEnumerable<CsvFilesInfo> Get(DateTime begin, DateTime end)
        {
            return _service.QueryFilesInfos(begin, end);
        }

        [HttpGet]
        [ApiDoc("查询指定DT测试文件名的基本信息")]
        [ApiParameterDoc("fileName", "DT测试文件名")]
        [ApiResponse("DT测试文件的基本信息")]
        public CsvFilesInfo Get(string fileName)
        {
            return _service.QueryCsvFilesInfo(fileName);
        }

        [HttpPost]
        [ApiDoc("更新指定DT测试文件名的距离等信息")]
        [ApiParameterDoc("filesInfo", "DT测试文件信息")]
        [ApiResponse("更新结果")]
        public int Get(CsvFilesInfo filesInfo)
        {
            return _service.UpdateFileDistance(filesInfo);
        }
    }

    [ApiControl("镇区测试文件联合信息查询控制器")]
    public class TownTestInfoController : ApiController
    {
        private readonly TownTestInfoService _service;

        public TownTestInfoController(TownTestInfoService service)
        {
            _service = service;
        }

        [HttpGet]
        [ApiDoc("查询指定数据文件（已知类型）在各个镇区的详细信息")]
        [ApiParameterDoc("csvFileName", "数据文件名称")]
        [ApiParameterDoc("type", "数据文件类型")]
        [ApiResponse("各个镇区DT统计的详细信息")]
        public IEnumerable<AreaTestInfo> Get(string csvFileName, string type)
        {
            return _service.CalculateAreaTestInfos(csvFileName, type);
        }

        [HttpGet]
        public IEnumerable<AreaTestInfo> Get(int fileId)
        {
            return _service.QueryAreaTestInfos(fileId);
        }

        [HttpPut]
        public int Put(AreaTestInfo info)
        {
            return _service.UpdateAreaTestInfo(info);
        }
    }

    [ApiControl("道路测试文件联合信息查询控制器")]
    public class RoadTestInfoController : ApiController
    {
        private readonly TownTestInfoService _service;
        private readonly HotSpotService _hotSpotService;

        public RoadTestInfoController(TownTestInfoService service, HotSpotService hotSpotService)
        {
            _service = service;
            _hotSpotService = hotSpotService;
        }

        [HttpGet]
        [ApiDoc("查询指定数据文件（已知类型）在各个道路的详细信息")]
        [ApiParameterDoc("csvFileName", "数据文件名称")]
        [ApiParameterDoc("type", "数据文件类型")]
        [ApiResponse("各个道路DT统计的详细信息")]
        public IEnumerable<AreaTestInfo> Get(string csvFileName, string type)
        {
            return _service.CalculateRoadTestInfos(csvFileName, type);
        }

        [HttpGet]
        public IEnumerable<AreaTestInfo> Get(int fileId)
        {
            return _service.QueryRoadTestInfos(fileId);
        }

        [HttpGet]
        public IEnumerable<AreaTestFileView> Get(string roadName, DateTime begin, DateTime end)
        {
            var road = _hotSpotService.QueryHotSpot(roadName, HotspotType.Highway);
            if (road == null) return new List<AreaTestFileView>();
            return _service.QueryRoadTestInfos(begin, end, road);
        }
    }

    [ApiControl("各区域测试日期信息的控制器")]
    public class AreaTestDateController : ApiController
    {
        private readonly AreaTestDateService _service;

        public AreaTestDateController(AreaTestDateService service)
        {
            _service = service;
        }

        [HttpGet]
        [ApiDoc("获得各区域测试日期信息，包括2G、3G、4G的最近一次测试日期")]
        [ApiResponse("各区域测试日期信息，包括2G、3G、4G的最近一次测试日期")]
        public IEnumerable<AreaTestDateView> Get()
        {
            return _service.QueryAllList();
        }

        [HttpGet]
        public int Get(DateTime testDate, string networkType, int townId)
        {
            return _service.UpdateLastDate(testDate, networkType, townId);
        }
    }

    [ApiControl("带有AGPS详细信息数据点查询控制器")]
    public class AgisDtPointsController : ApiController
    {
        private readonly NearestPciCellService _service;

        public AgisDtPointsController(NearestPciCellService service)
        {
            _service = service;
        }

        [HttpGet]
        [ApiDoc("查询日期范围内带有AGPS详细信息数据点")]
        [ApiParameterDoc("begin", "开始日期")]
        [ApiParameterDoc("end", "结束日期")]
        [ApiResponse("日期范围内带有AGPS详细信息数据点")]
        public IEnumerable<AgisDtPoint> Get(DateTime begin, DateTime end)
        {
            return _service.QueryAgisDtPoints(begin, end);
        }

        [HttpGet]
        [ApiDoc("查询日期范围内和指定主题带有AGPS详细信息数据点")]
        [ApiParameterDoc("begin", "开始日期")]
        [ApiParameterDoc("end", "结束日期")]
        [ApiParameterDoc("topic", "指定主题")]
        [ApiResponse("日期范围内和指定主题带有AGPS详细信息数据点")]
        public IEnumerable<AgisDtPoint> Get(DateTime begin, DateTime end, string topic)
        {
            return _service.QueryAgisDtPoints(begin, end, topic);
        }
    }
}
