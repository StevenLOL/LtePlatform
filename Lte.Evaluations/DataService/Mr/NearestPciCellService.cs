﻿using System;
using Lte.Evaluations.ViewModels.Mr;
using Lte.Parameters.Abstract.Basic;
using Lte.Parameters.Entities.Neighbor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Abp.EntityFramework.AutoMapper;
using Abp.EntityFramework.Dependency;
using Abp.EntityFramework.Repositories;
using Lte.Domain.Common;
using Lte.Domain.Common.Geo;
using Lte.Domain.Common.Wireless;
using Lte.Domain.LinqToCsv.Context;
using Lte.Domain.Regular;
using Lte.MySqlFramework.Entities;
using Lte.Parameters.Abstract;
using Lte.MySqlFramework.Abstract;
using Lte.Parameters.Abstract.Infrastructure;
using Lte.Parameters.Abstract.Kpi;
using Lte.Parameters.Entities.Channel;

namespace Lte.Evaluations.DataService.Mr
{
    internal class TownSupportService
    {
        private readonly ITownRepository _townRepository;
        private readonly ITownBoundaryRepository _boundaryRepository;

        public TownSupportService(ITownRepository townRepository, ITownBoundaryRepository boundaryRepository)
        {
            _townRepository = townRepository;
            _boundaryRepository = boundaryRepository;
        }

        public string QueryFileNameDistrict(string fileName)
        {
            var districts = _townRepository.GetAllList().Select(x => x.DistrictName).Distinct();
            return districts.FirstOrDefault(fileName.Contains);
        }

        public IEnumerable<List<GeoPoint>> QueryTownBoundaries(string district, string town)
        {
            var townItem = _townRepository.QueryTown(district, town);
            if (townItem == null) return null;
            return _boundaryRepository.GetAllList(x => x.TownId == townItem.Id).Select(x =>
            {
                var coors = x.Boundary.GetSplittedFields(' ');
                var coorList = new List<GeoPoint>();
                for (var i = 0; i < coors.Length / 2; i++)
                {
                    coorList.Add(new GeoPoint(coors[i * 2].ConvertToDouble(0), coors[i * 2 + 1].ConvertToDouble(0)));
                }
                return coorList;
            });
        } 
    }

    internal class AgpsService
    {
        private readonly ITelecomAgpsRepository _telecomAgpsRepository;
        private readonly IMobileAgpsRepository _mobileAgpsRepository;

        public AgpsService(ITelecomAgpsRepository telecomAgpsRepository, IMobileAgpsRepository mobileAgpsRepository)
        {
            _telecomAgpsRepository = telecomAgpsRepository;
            _mobileAgpsRepository = mobileAgpsRepository;
        }

        public IEnumerable<AgpsCoverageView> QueryTelecomCoverageViews(DateTime begin, DateTime end,
            IEnumerable<List<GeoPoint>> boundaries)
        {
            return new List<AgpsCoverageView>();
        } 
    }

    internal class MrGridService
    {
        private readonly IMrGridRepository _repository;

        public MrGridService(IMrGridRepository repository)
        {
            _repository = repository;
        }

        public void UploadMrGrids(XmlDocument xml, string district, string fileName)
        {
            var candidateDescritions = new[] { "竞对总体", "移动竞对", "联通竞对" };
            var competeDescription = candidateDescritions.FirstOrDefault(fileName.Contains);
            var list = competeDescription == null
                ? MrGridXml.ReadGridXmls(xml, district ?? "禅城")
                : MrGridXml.ReadGridXmlsWithCompete(xml, district ?? "禅城", competeDescription);
            foreach (var item in list)
            {
                _repository.Insert(item.MapTo<MrGrid>());
            }
            _repository.SaveChanges();
        }

        public IEnumerable<MrCoverageGridView> QueryCoverageGridViews(DateTime initialDate, string district)
        {
            var stats =
                _repository.QueryDate(initialDate, (repository, beginDate, endDate) => repository.GetAllList(
                    x =>
                        x.StatDate >= beginDate && x.StatDate < endDate && x.District == district &&
                        x.Compete == AlarmCategory.Self));
            return stats.MapTo<IEnumerable<MrCoverageGridView>>();
        }

        public IEnumerable<MrCoverageGridView> QueryCoverageGridViews(DateTime initialDate,
            IEnumerable<List<GeoPoint>> boundaries, string district)
        {
            var stats =
                _repository.QueryDate(initialDate, (repository, beginDate, endDate) => repository.GetAllList(
                    x =>
                        x.StatDate >= beginDate && x.StatDate < endDate && x.District == district &&
                        x.Compete == AlarmCategory.Self)).Where(x =>
                        {
                            var fields = x.Coordinates.GetSplittedFields(';')[0].GetSplittedFields(',');
                            var point = new GeoPoint(fields[0].ConvertToDouble(0), fields[1].ConvertToDouble(0));
                            return boundaries.Any(boundary => GeoMath.IsInPolygon(point, boundary));
                        });
            return stats.MapTo<IEnumerable<MrCoverageGridView>>();
        }

        public IEnumerable<MrCompeteGridView> QueryCompeteGridViews(DateTime initialDate, string district,
            AlarmCategory? compete)
        {
            var stats =
                _repository.QueryDate(initialDate, (repository, beginDate, endDate) => repository.GetAllList(
                    x =>
                        x.StatDate >= beginDate && x.StatDate < endDate && x.District == district &&
                        x.Frequency == -1 && x.Compete == compete)).ToList();
            return stats.MapTo<IEnumerable<MrCompeteGridView>>();
        }

        public IEnumerable<MrCompeteGridView> QueryCompeteGridViews(DateTime initialDate, string district,
            AlarmCategory? compete, IEnumerable<List<GeoPoint>> boundaries)
        {
            var stats =
                _repository.QueryDate(initialDate, (repository, beginDate, endDate) => repository.GetAllList(
                    x =>
                        x.StatDate >= beginDate && x.StatDate < endDate && x.District == district &&
                        x.Frequency == -1 && x.Compete == compete)).Where(x =>
                        {
                            var fields = x.Coordinates.GetSplittedFields(';')[0].GetSplittedFields(',');
                            var point = new GeoPoint(fields[0].ConvertToDouble(0), fields[1].ConvertToDouble(0));
                            return boundaries.Aggregate(false, (current, boundary) => current || GeoMath.IsInPolygon(point, boundary));
                        });
            return stats.MapTo<IEnumerable<MrCompeteGridView>>();
        }
    }

    public class NearestPciCellService
    {
        private readonly INearestPciCellRepository _repository;
        private readonly ICellRepository _cellRepository;
        private readonly IENodebRepository _eNodebRepository;
        private readonly IAgisDtPointRepository _agisRepository;
        
        private readonly IAppStreamRepository _streamRepository;
        private readonly IWebBrowsingRepository _browsingRepository;

        private readonly TownSupportService _service;
        private readonly AgpsService _agpsService;
        private readonly MrGridService _mrGridService;

        private static Stack<NearestPciCell> NearestCells { get; set; }

        public NearestPciCellService(INearestPciCellRepository repository, ICellRepository cellRepository,
            IENodebRepository eNodebRepository, IAgisDtPointRepository agisRepository,
            ITownRepository townRepository, IMrGridRepository mrGridRepository,
            IAppStreamRepository streamRepository, IWebBrowsingRepository browsingRepository,
            ITownBoundaryRepository boundaryRepository, ITelecomAgpsRepository telecomAgpsRepository,
            IMobileAgpsRepository mobileAgpsRepository)
        {
            _repository = repository;
            _cellRepository = cellRepository;
            _eNodebRepository = eNodebRepository;
            _agisRepository = agisRepository;
            
            _streamRepository = streamRepository;
            _browsingRepository = browsingRepository;

            _service = new TownSupportService(townRepository, boundaryRepository);
            _agpsService = new AgpsService(telecomAgpsRepository, mobileAgpsRepository);
            _mrGridService = new MrGridService(mrGridRepository);
            
            if (NearestCells == null)
                NearestCells = new Stack<NearestPciCell>();
        }

        public List<NearestPciCellView> QueryCells(int cellId, byte sectorId)
        {
            return
                _repository.GetAllList(cellId, sectorId)
                    .Select(
                        x =>
                            NearestPciCellView.ConstructView(x, _eNodebRepository))
                    .ToList();
        }

        public List<NearestPciCell> QueryNeighbors(int cellId, byte sectorId)
        {
            return _repository.GetAllList(cellId, sectorId);
        }

        public NearestPciCell QueryNearestPciCell(int cellId, byte sectorId, short pci)
        {
            return _repository.GetNearestPciCell(cellId, sectorId, pci);
        }

        public int UpdateNeighborPcis(int cellId, byte sectorId)
        {
            var neighborList = _repository.GetAllList(cellId, sectorId);
            foreach (var pciCell in neighborList)
            {
                var cell = _cellRepository.GetBySectorId(pciCell.NearestCellId, pciCell.NearestSectorId);
                if (cell == null || pciCell.Pci == cell.Pci) continue;
                pciCell.Pci = cell.Pci;
                _repository.Update(pciCell);
                neighborList = _repository.GetAllList(pciCell.NearestCellId, pciCell.NearestSectorId);
                foreach (var nearestPciCell in neighborList)
                {
                    cell = _cellRepository.GetBySectorId(nearestPciCell.NearestCellId, nearestPciCell.NearestSectorId);
                    if (cell==null||nearestPciCell.Pci==cell.Pci) continue;
                    nearestPciCell.Pci = cell.Pci;
                    _repository.Update(nearestPciCell);
                }
            }
            return _repository.SaveChanges();
        }

        public void UpdateNeighborCell(NearestPciCell cell)
        {
            var item = _repository.GetNearestPciCell(cell.CellId, cell.SectorId, cell.Pci);
            if (item != null)
            {
                item.NearestCellId = cell.NearestCellId;
                item.NearestSectorId = cell.NearestSectorId;
                _repository.Update(item);
            }
            else
            {
                cell.TotalTimes = 98;
                _repository.Insert(cell);
            }
            _repository.SaveChanges();
        }

        public void UploadAgisDtPoints(StreamReader reader)
        {
            string line;
            int count = 0;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                var fields = line.Split(',');
                var dtPoint = new AgisDtPoint
                {
                    Operator = fields[0],
                    Longtitute = fields[1].ConvertToDouble(0),
                    Lattitute = fields[2].ConvertToDouble(0),
                    UnicomRsrp = fields[3].ConvertToDouble(-140),
                    MobileRsrp = fields[4].ConvertToDouble(-140),
                    TelecomRsrp = fields[5].ConvertToDouble(-140),
                    StatDate = DateTime.Today.AddDays(-1)
                };
                _agisRepository.Insert(dtPoint);
                if (count++%1000 == 0)
                    _agisRepository.SaveChanges();
            }
            _agisRepository.SaveChanges();

        }

        public void UploadMrGrids(StreamReader reader, string fileName)
        {
            var xml = new XmlDocument();
            xml.Load(reader);
            
            var district = _service.QueryFileNameDistrict(fileName);
            _mrGridService.UploadMrGrids(xml, district, fileName);
        }

        public async Task<int> UploadWebBrowsings(StreamReader reader)
        {
            var csvs = CsvContext.Read<WebBrowsingCsv>(reader);
            return await _browsingRepository.UpdateMany<IWebBrowsingRepository, WebBrowsing, WebBrowsingCsv>(csvs);
        }

        public async Task<int> UploadStreamings(StreamReader reader)
        {
            var csvs = CsvContext.Read<AppStreamingCsv>(reader);
            return await _streamRepository.UpdateMany<IAppStreamRepository, AppSteam, AppStreamingCsv>(csvs);
        }

        public IEnumerable<AgisDtPoint> QueryAgisDtPoints(DateTime begin, DateTime end)
        {
            var points = _agisRepository.GetAllList(x => x.StatDate > begin && x.StatDate <= end);
            return points;
        }

        public IEnumerable<AgisDtPoint> QueryAgisDtPoints(DateTime begin, DateTime end, string topic)
        {
            var points = _agisRepository.GetAllList(x => x.StatDate > begin && x.StatDate <= end && x.Operator == topic);
            return points;
        }

        public IEnumerable<MrCoverageGridView> QueryCoverageGridViews(DateTime initialDate, string district)
        {
            return _mrGridService.QueryCoverageGridViews(initialDate, district);
        }

        public IEnumerable<MrCoverageGridView> QueryCoverageGridViews(DateTime initialDate, string district, string town)
        {
            var boundaries = _service.QueryTownBoundaries(district, town);
            if (boundaries == null) return new List<MrCoverageGridView>();
            return _mrGridService.QueryCoverageGridViews(initialDate, boundaries, district);
        }

        public IEnumerable<AgpsCoverageView> QueryTelecomCoverageViews(DateTime begin, DateTime end, string district,
            string town)
        {
            var boundaries = _service.QueryTownBoundaries(district, town);
            if (boundaries == null) return new List<AgpsCoverageView>();
            return _agpsService.QueryTelecomCoverageViews(begin, end, boundaries);
        } 

        public IEnumerable<MrCompeteGridView> QueryCompeteGridViews(DateTime initialDate, string district,
            string competeDescription)
        {
            var competeTuple =
                WirelessConstants.EnumDictionary["AlarmCategory"].FirstOrDefault(x => x.Item2 == competeDescription);
            var compete = (AlarmCategory?)competeTuple?.Item1;

            return _mrGridService.QueryCompeteGridViews(initialDate, district, compete);
        }

        public IEnumerable<MrCompeteGridView> QueryCompeteGridViews(DateTime initialDate, string district, string town,
            string competeDescription)
        {

            var boundaries = _service.QueryTownBoundaries(district, town);
            if (boundaries == null) return new List<MrCompeteGridView>();
            var competeTuple =
                WirelessConstants.EnumDictionary["AlarmCategory"].FirstOrDefault(x => x.Item2 == competeDescription);
            var compete = (AlarmCategory?)competeTuple?.Item1;

            return _mrGridService.QueryCompeteGridViews(initialDate, district, compete, boundaries);
        }
    }
}
