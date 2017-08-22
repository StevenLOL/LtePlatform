using Lte.Domain.LinqToCsv;
using Lte.Domain.LinqToCsv.Context;
using Lte.Domain.LinqToCsv.Description;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Abp.Domain.Entities;
using Lte.Domain.Regular.Attributes;

namespace Lte.Parameters.Entities.Neighbor
{
    [Table("dbo.LteNeighborCells")]
    public class NearestPciCell : LteNeighborCell
    {
        public short Pci { get; set; }
        
        public int TotalTimes { get; set; }

    }

    public class NeighborCellZteCsv
    {
        [CsvColumn(Name = "��Ԫ")]
        public int ENodebId { get; set; }

        [CsvColumn(Name = "С��")]
        public byte SectorId { get; set; }

        [CsvColumn(Name = "������ϵ")]
        public string NeighborRelation { get; set; }

        [CsvColumn(Name = "[FDD]ϵͳ��ͬƵ�л������������С���ԣ�")]
        public int IntraSystemTimes { get; set; }

        [CsvColumn(Name = "[FDD]ϵͳ����Ƶ�л����������(С����)")]
        public int InterSystemTimes { get; set; }

        public static List<NeighborCellZteCsv> ReadNeighborCellZteCsvs(StreamReader reader)
        {
            var infos = CsvContext.Read<NeighborCellZteCsv>(reader, CsvFileDescription.CommaDescription);
            var groupInfos = (from info in infos
                              group info by new { info.ENodebId, info.SectorId, info.NeighborRelation }
                into g
                              select new NeighborCellZteCsv
                              {
                                  ENodebId = g.Key.ENodebId,
                                  SectorId = g.Key.SectorId,
                                  NeighborRelation = g.Key.NeighborRelation,
                                  IntraSystemTimes = g.Sum(x => x.IntraSystemTimes),
                                  InterSystemTimes = g.Sum(x => x.InterSystemTimes)
                              }).ToList();
            return groupInfos;
        }

    }

    public class NeighborCellHwCsv
    {
        [CsvColumn(Name = "��С��")]
        public string CellRelation { get; set; }

        [CsvColumn(Name = "�ض���С�����л������Դ��� (��)")]
        public int TotalTimes { get; set; }

        public static List<NeighborCellHwCsv> ReadNeighborCellHwCsvs(StreamReader reader)
        {
            var infos = CsvContext.Read<NeighborCellHwCsv>(reader, CsvFileDescription.CommaDescription);
            var groupInfos = (from info in infos
                              group info by info.CellRelation
                into g
                              select new NeighborCellHwCsv
                              {
                                  CellRelation = g.Key,
                                  TotalTimes = g.Sum(x => x.TotalTimes)
                              }).ToList();
            return groupInfos;
        }
    }

    [Table("dbo.LteNeighborCells")]
    [KnownType(typeof(NearestPciCell))]
    [TypeDoc("LTE������ϵ����")]
    public class LteNeighborCell : Entity
    {
        [MemberDoc("С����ţ�����LTE��˵���ǻ�վ��ţ�")]
        public int CellId { get; set; }

        [MemberDoc("�������")]
        public byte SectorId { get; set; }

        [MemberDoc("����С�����")]
        public int NearestCellId { get; set; }

        [MemberDoc("�����������")]
        public byte NearestSectorId { get; set; }
    }
}