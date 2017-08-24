using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Abp.Domain.Entities;
using Abp.EntityFramework.AutoMapper;
using Abp.EntityFramework.Dependency;
using AutoMapper;
using Lte.Domain.Common;
using Lte.Domain.Regular.Attributes;
using Lte.Parameters.Abstract.Basic;
using MongoDB.Bson;

namespace Lte.Parameters.Entities.Neighbor
{
    public class EUtranRelationZte : IEntity<ObjectId>, IZteMongo
    {
        public int eNodeB_Id { get; set; }

        public string eNodeB_Name { get; set; }

        public string lastModifedTime { get; set; }

        public string iDate { get; set; }

        public string parentLDN { get; set; }

        public string description { get; set; }

        public int resPRBDown { get; set; }

        public int resPRBUp { get; set; }

        public int overlapCoverage { get; set; }

        public int shareCover { get; set; }

        public int numRRCCntNumCov { get; set; }

        public int lbIntraMeasureOffset { get; set; }

        public int isX2HOAllowed { get; set; }

        public string userLabel { get; set; }

        public int coperType { get; set; }

        public int isAnrCreated { get; set; }

        public int s1DataFwdFlag { get; set; }

        public int isHOAllowed { get; set; }

        public int switchonTimeWindow { get; set; }

        public int nCelPriority { get; set; }

        public int EUtranRelation { get; set; }

        public int isESCoveredBy { get; set; }

        public int stateInd { get; set; }

        public string refEUtranCellFDD { get; set; }

        public int cellIndivOffset { get; set; }

        public int isRemoveAllowed { get; set; }

        public int qofStCell { get; set; }

        public int esSwitch { get; set; }

        public int coverESCell { get; set; }

        public string refExternalEUtranCellFDD { get; set; }

        public string supercellFlag { get; set; }

        public string refExternalEUtranCellTDD { get; set; }

        public ObjectId Id { get; set; }

        public bool IsTransient()
        {
            return false;
        }
    }

    [TypeDoc("����MongoDB��LTE������ϵ��ͼ")]
    [AutoMapFrom(typeof(EUtranRelationZte), typeof(ExternalEUtranCellFDDZte), typeof(EutranIntraFreqNCell), typeof(EutranInterFreqNCell))]
    public class NeighborCellMongo
    {
        [MemberDoc("С����ţ�����LTE��˵���ǻ�վ��ţ�")]
        [AutoMapPropertyResolve("eNodeB_Id", typeof(EUtranRelationZte))]
        [AutoMapPropertyResolve("eNodeB_Id", typeof(ExternalEUtranCellFDDZte))]
        [AutoMapPropertyResolve("eNodeB_Id", typeof(EutranIntraFreqNCell))]
        [AutoMapPropertyResolve("eNodeB_Id", typeof(EutranInterFreqNCell))]
        public int CellId { get; set; }

        [MemberDoc("�������")]
        public byte SectorId { get; set; }

        [MemberDoc("����С�����")]
        [AutoMapPropertyResolve("eNBId", typeof(ExternalEUtranCellFDDZte))]
        [AutoMapPropertyResolve("eNodeBId", typeof(EutranIntraFreqNCell))]
        [AutoMapPropertyResolve("eNodeBId", typeof(EutranInterFreqNCell))]
        public int NeighborCellId { get; set; }

        [MemberDoc("�����������")]
        [AutoMapPropertyResolve("cellLocalId", typeof(ExternalEUtranCellFDDZte))]
        [AutoMapPropertyResolve("CellId", typeof(EutranIntraFreqNCell))]
        [AutoMapPropertyResolve("CellId", typeof(EutranInterFreqNCell))]
        public byte NeighborSectorId { get; set; }

        [MemberDoc("��������")]
        [AutoMapPropertyResolve("userLabel", typeof(ExternalEUtranCellFDDZte))]
        [AutoMapPropertyResolve("NeighbourCellName", typeof(EutranIntraFreqNCell))]
        [AutoMapPropertyResolve("NeighbourCellName", typeof(EutranInterFreqNCell))]
        public string NeighborCellName { get; set; }

        [MemberDoc("PCI�����ڲ�ѯ����")]
        [AutoMapPropertyResolve("pci", typeof(ExternalEUtranCellFDDZte))]
        public short NeighborPci { get; set; }

        [MemberDoc("�Ƿ�ΪANR����")]
        [AutoMapPropertyResolve("isAnrCreated", typeof(EUtranRelationZte), typeof(IntToBoolTransform))]
        [AutoMapPropertyResolve("AnrFlag", typeof(EutranIntraFreqNCell), typeof(PositiveBoolTransform))]
        [AutoMapPropertyResolve("AnrFlag", typeof(EutranInterFreqNCell), typeof(PositiveBoolTransform))]
        public bool IsAnrCreated { get; set; }

        [MemberDoc("�Ƿ������л�")]
        [AutoMapPropertyResolve("isHOAllowed", typeof(EUtranRelationZte), typeof(IntToBoolTransform))]
        [AutoMapPropertyResolve("NoHoFlag", typeof(EutranIntraFreqNCell), typeof(ZeroBoolTransform))]
        [AutoMapPropertyResolve("NoHoFlag", typeof(EutranInterFreqNCell), typeof(ZeroBoolTransform))]
        public bool HandoffAllowed { get; set; }

        [MemberDoc("�Ƿ���Ա�ANRɾ��")]
        [AutoMapPropertyResolve("isRemoveAllowed", typeof(EUtranRelationZte), typeof(IntToBoolTransform))]
        [AutoMapPropertyResolve("NoRmvFlag", typeof(EutranIntraFreqNCell), typeof(ZeroBoolTransform))]
        [AutoMapPropertyResolve("NoRmvFlag", typeof(EutranInterFreqNCell), typeof(ZeroBoolTransform))]
        public bool RemovedAllowed { get; set; }

        [MemberDoc("С���������ȼ��Ƿ�Ϊ��")]
        [AutoMapPropertyResolve("CellMeasPriority", typeof(EutranIntraFreqNCell))]
        [AutoMapPropertyResolve("CellMeasPriority", typeof(EutranInterFreqNCell))]
        [AutoMapPropertyResolve("nCelPriority", typeof(EUtranRelationZte))]
        public int CellPriority { get; set; }
    }

    public class ExternalEUtranCellFDDZte : IEntity<ObjectId>, IZteMongo
    {
        public ObjectId Id { get; set; }

        public bool IsTransient()
        {
            return false;
        }

        public int eNodeB_Id { get; set; }

        public string eNodeB_Name { get; set; }

        public string lastModifedTime { get; set; }

        public string iDate { get; set; }

        public string parentLDN { get; set; }

        public string description { get; set; }

        public int? esCellNum { get; set; }

        public double earfcnDl { get; set; }

        public int cellType { get; set; }

        public int pci { get; set; }

        public string userLabel { get; set; }

        public int antPort1 { get; set; }

        public int cellLocalId { get; set; }

        public int plmnIdList_mcc { get; set; }

        public int switchSurportTrunking { get; set; }

        public int tac { get; set; }

        public int plmnIdList_mnc { get; set; }

        public string reservedByEUtranRelation { get; set; }

        public int bandWidthUl { get; set; }

        public int mcc { get; set; }

        public int eNBId { get; set; }

        public double earfcnUl { get; set; }

        public int freqBandInd { get; set; }

        public int voLTESwch { get; set; }

        public int coMPFlagUl { get; set; }

        public int bandWidthDl { get; set; }

        public int mnc { get; set; }

        public string addiFreqBand { get; set; }

        public int ExternalEUtranCellFDD { get; set; }
    }

    public class EutranIntraFreqNCell : IEntity<ObjectId>, IHuaweiNeighborMongo
    {
        public ObjectId Id { get; set; }

        public bool IsTransient()
        {
            return false;
        }

        public string iDate { get; set; }

        public int eNodeB_Id { get; set; }

        public string eNodeBId_Name { get; set; }

        public int CtrlMode { get; set; }

        public int CellMeasPriority { get; set; }

        public string NeighbourCellName { get; set; }

        public int LocalCellId { get; set; }

        public int AttachCellSwitch { get; set; }

        public int NoHoFlag { get; set; }

        public int CellId { get; set; }

        public string LocalCellName { get; set; }

        public int CellRangeExpansion { get; set; }

        public int Mnc { get; set; }

        public int Mcc { get; set; }

        public int NCellClassLabel { get; set; }

        public int CellQoffset { get; set; }

        public int NoRmvFlag { get; set; }

        public int eNodeBId { get; set; }

        public int CellIndividualOffset { get; set; }

        public int AnrFlag { get; set; }

        public int? VectorCellFlag { get; set; }

        public int? HighSpeedCellIndOffset { get; set; }
    }

    public class EutranInterFreqNCell : IEntity<ObjectId>, IHuaweiNeighborMongo
    {
        public bool IsTransient()
        {
            return false;
        }

        public ObjectId Id { get; set; }

        public string iDate { get; set; }

        public int eNodeB_Id { get; set; }

        public string eNodeBId_Name { get; set; }

        public int LocalCellId { get; set; }

        public int Mnc { get; set; }

        public int NoRmvFlag { get; set; }

        public int Mcc { get; set; }

        public int CellMeasPriority { get; set; }

        public int CtrlMode { get; set; }

        public int eNodeBId { get; set; }

        public int CellId { get; set; }

        public int AnrFlag { get; set; }

        public int BlindHoPriority { get; set; }

        public int NCellClassLabel { get; set; }

        public int CellQoffset { get; set; }

        public int CellIndividualOffset { get; set; }

        public int OverlapRange { get; set; }

        public string LocalCellName { get; set; }

        public int OverlapInd { get; set; }

        public int NoHoFlag { get; set; }

        public string NeighbourCellName { get; set; }
    }

    public class EutranInterNFreq : IEntity<ObjectId>, IHuaweiCellMongo
    {
        public string iDate { get; set; }

        public int eNodeB_Id { get; set; }

        public string eNodeBId_Name { get; set; }

        public int ThreshXlow { get; set; }

        public int MeasFreqPriority { get; set; }

        public int EutranReselTime { get; set; }

        public int LocalCellId { get; set; }

        public int FreqPriBasedHoMeasFlag { get; set; }

        public int MasterBandFlag { get; set; }

        public int CtrlMode { get; set; }

        public int PmaxCfgInd { get; set; }

        public int UlTrafficMlbTargetInd { get; set; }

        public int FreqPriorityForAnr { get; set; }

        public int MlbTargetInd { get; set; }

        public int InterFreqHighSpeedFlag { get; set; }

        public int ConnFreqPriority { get; set; }

        public int CellReselPriorityCfgInd { get; set; }

        public int VoipPriority { get; set; }

        public int SpeedDependSPCfgInd { get; set; }

        public int QqualMinCfgInd { get; set; }

        public int MlbFreqPriority { get; set; }

        public int DlEarfcn { get; set; }

        public int UlTrafficMlbPriority { get; set; }

        public int AnrInd { get; set; }

        public int ThreshXlowQ { get; set; }

        public int InterFreqHoEventType { get; set; }

        public int IdleMlbUEReleaseRatio { get; set; }

        public int QRxLevMin { get; set; }

        public int UlEarfcnCfgInd { get; set; }

        public int NeighCellConfig { get; set; }

        public int QoffsetFreqConn { get; set; }

        public int MeasBandWidth { get; set; }

        public int PresenceAntennaPort1 { get; set; }

        public int BackoffTargetInd { get; set; }

        public int ThreshXhigh { get; set; }

        public int MlbInterFreqHoEventType { get; set; }

        public int ThreshXhighQ { get; set; }

        public int IfHoThdRsrpOffset { get; set; }

        public int InterFreqRanSharingInd { get; set; }

        public int IfMlbThdRsrpOffset { get; set; }

        public int QoffsetFreq { get; set; }

        public int? CellReselPriority { get; set; }

        public ObjectId Id { get; set; }

        public bool IsTransient()
        {
            return false;
        }

        public int? InterFreqMlbDlPrbOffset { get; set; }

        public int? IfSrvHoThdRsrqOffset { get; set; }

        public int? MeasPerformanceDemand { get; set; }

        public int? InterFreqMlbUlPrbOffset { get; set; }

        public int? MlbInterFreqHoA3Offset { get; set; }

        public int? IfSrvHoThdRsrpOffset { get; set; }

        public int? PsPriority { get; set; }

        public int? NcellNumForAnr { get; set; }

        public int? MlbFreqUlPriority { get; set; }

        public int? SnrBasedUeSelectionMode { get; set; }

        public int? VolteHoTargetInd { get; set; }

        public int? MlbInterFreqEffiRatio { get; set; }

        public int? MobilityTargetInd { get; set; }
    }
}