﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Evaluations.ViewModels.Switch;
using Lte.Parameters.Entities.Switch;
using Lte.Parameters.Abstract.Switch;
using Lte.Parameters.Abstract.Basic;
using AutoMapper;

namespace Lte.Evaluations.DataService.Switch
{
    public class IntraFreqHoService
    {
        private readonly IUeEUtranMeasurementRepository _zteMeasurementRepository;
        private readonly ICellMeasGroupZteRepository _zteGroupRepository;
        private readonly IEUtranCellMeasurementZteRepository _zteCellGroupRepository;
        private readonly IIntraFreqHoGroupRepository _huaweiCellHoRepository;
        private readonly IIntraRatHoCommRepository _huaweiENodebHoRepository;
        private readonly ICellHuaweiMongoRepository _huaweiCellRepository;
        private readonly IENodebRepository _eNodebRepository;

        public IntraFreqHoService(IUeEUtranMeasurementRepository zteMeasurementRepository,
            ICellMeasGroupZteRepository zteGroupRepository, IEUtranCellMeasurementZteRepository zteCellGroupRepository,
            IIntraFreqHoGroupRepository huaweiCellHoRepository, IIntraRatHoCommRepository huaweiENodebHoRepository,
            ICellHuaweiMongoRepository huaweiCellRepository, IENodebRepository eNodebRepository)
        {
            _zteMeasurementRepository = zteMeasurementRepository;
            _zteGroupRepository = zteGroupRepository;
            _zteCellGroupRepository = zteCellGroupRepository;
            _huaweiCellHoRepository = huaweiCellHoRepository;
            _huaweiENodebHoRepository = huaweiENodebHoRepository;
            _huaweiCellRepository = huaweiCellRepository;
            _eNodebRepository = eNodebRepository;
        }

        private IMongoQuery<ENodebIntraFreqHoView> ConstructENodebQuery(int eNodebId)
        {
            var eNodeb = _eNodebRepository.GetByENodebId(eNodebId);
            if (eNodeb == null) return null;
            return eNodeb.Factory == "华为"
                ? (IMongoQuery<ENodebIntraFreqHoView>) new HuaweiIntraFreqENodebQuery(_huaweiENodebHoRepository, eNodebId)
                : new ZteIntraFreqENodebQuery(_zteGroupRepository, _zteMeasurementRepository, eNodebId);
        }

        public ENodebIntraFreqHoView QueryENodebHo(int eNodebId)
        {
            var query = ConstructENodebQuery(eNodebId);
            return query?.Query();
        }

        private IMongoQuery<CellIntraFreqHoView> ConstructCellQuery(int eNodebId, byte sectorId)
        {
            var eNodeb = _eNodebRepository.GetByENodebId(eNodebId);
            if (eNodeb == null) return null;
            return eNodeb.Factory == "华为"
                ? (IMongoQuery<CellIntraFreqHoView>)
                    new HuaweiIntraFreqCellQuery(_huaweiCellRepository, _huaweiCellHoRepository, eNodebId, sectorId)
                : new ZteIntraFreqCellQuery(_zteMeasurementRepository, _zteGroupRepository, _zteCellGroupRepository, eNodebId,
                    sectorId);
        }

        public CellIntraFreqHoView QueryCellHo(int eNodebId, byte sectorId)
        {
            var query = ConstructCellQuery(eNodebId, sectorId);
            return query?.Query();
        }
    }

    internal class HuaweiIntraFreqENodebQuery : HuaweiENodebQuery<IntraRatHoComm, ENodebIntraFreqHoView, IIntraRatHoCommRepository>
    {
        public HuaweiIntraFreqENodebQuery(IIntraRatHoCommRepository repository, int eNodebId) : base(repository, eNodebId)
        {
        }
    }

    internal class ZteIntraFreqENodebQuery : ZteGeneralENodebQuery<UeEUtranMeasurementZte, ENodebIntraFreqHoView>
    {
        private readonly ICellMeasGroupZteRepository _zteGroupRepository;
        private readonly IUeEUtranMeasurementRepository _zteMeasurementRepository;

        public ZteIntraFreqENodebQuery(ICellMeasGroupZteRepository zteGroupRepository,
            IUeEUtranMeasurementRepository zteMeasurementRepository, int eNodebId) 
            : base(eNodebId)
        {
            _zteGroupRepository = zteGroupRepository;
            _zteMeasurementRepository = zteMeasurementRepository;
        }

        protected override UeEUtranMeasurementZte QueryStat()
        {
            if (UeEUtranMeasurementZte.IntraFreqHoConfigId < 0)
            {
                var zteGroup = _zteGroupRepository.GetRecent(ENodebId);
                UeEUtranMeasurementZte.IntraFreqHoConfigId = zteGroup == null
                    ? 50
                    : int.Parse(zteGroup.intraFHOMeasCfg.Split(',')[0]);
            }

            return _zteMeasurementRepository.GetRecent(ENodebId, UeEUtranMeasurementZte.IntraFreqHoConfigId);
        }
    }

    internal class HuaweiIntraFreqCellQuery : IMongoQuery<CellIntraFreqHoView>
    {
        private readonly ICellHuaweiMongoRepository _huaweiCellRepository;
        private readonly IIntraFreqHoGroupRepository _huaweiCellHoRepository;
        private readonly int _eNodebId;
        private readonly byte _sectorId;

        public HuaweiIntraFreqCellQuery(ICellHuaweiMongoRepository huaweiCellRepository,
            IIntraFreqHoGroupRepository huaweiCellHoRepository, int eNodebId, byte sectorId)
        {
            _huaweiCellHoRepository = huaweiCellHoRepository;
            _huaweiCellRepository = huaweiCellRepository;
            _eNodebId = eNodebId;
            _sectorId = sectorId;
        }

        public CellIntraFreqHoView Query()
        {
            var huaweiCell = _huaweiCellRepository.GetRecent(_eNodebId, _sectorId);
            var localCellId = huaweiCell?.LocalCellId ?? _sectorId;
            var huaweiPara = _huaweiCellHoRepository.GetRecent(_eNodebId, localCellId);
            return huaweiPara == null ? null : Mapper.Map<IntraFreqHoGroup, CellIntraFreqHoView>(huaweiPara);
        }
    }

    internal class ZteIntraFreqCellQuery : IMongoQuery<CellIntraFreqHoView>
    {
        private readonly IUeEUtranMeasurementRepository _zteMeasurementRepository;
        private readonly ICellMeasGroupZteRepository _zteGroupRepository;
        private readonly IEUtranCellMeasurementZteRepository _zteCellGroupRepository;
        private readonly int _eNodebId;
        private readonly byte _sectorId;

        public ZteIntraFreqCellQuery(IUeEUtranMeasurementRepository zteMeasurementRepository,
            ICellMeasGroupZteRepository zteGroupRepository, IEUtranCellMeasurementZteRepository zteCellGroupRepository, 
            int eNodebId, byte sectorId)
        {
            _zteGroupRepository = zteGroupRepository;
            _zteMeasurementRepository = zteMeasurementRepository;
            _zteCellGroupRepository = zteCellGroupRepository;
            _eNodebId = eNodebId;
            _sectorId = sectorId;
        }

        public CellIntraFreqHoView Query()
        {
            var zteCellGroup = _zteCellGroupRepository.GetRecent(_eNodebId, _sectorId);
            int configId;
            if (zteCellGroup != null)
                configId = int.Parse(zteCellGroup.intraFHOMeasCfg.Split(',')[0]);
            else
            {
                var zteGroup = _zteGroupRepository.GetRecent(_eNodebId);
                configId = zteGroup == null ? 50 : int.Parse(zteGroup.intraFHOMeasCfg.Split(',')[0]);
            }

            var ztePara = _zteMeasurementRepository.GetRecent(_eNodebId, configId);
            return ztePara == null ? null : Mapper.Map<UeEUtranMeasurementZte, CellIntraFreqHoView>(ztePara);
        }
    }

}
