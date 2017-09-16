using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lte.Domain.LinqToCsv;
using Lte.Domain.LinqToCsv.Context;
using Lte.Domain.LinqToCsv.Description;
using Lte.Domain.Regular;

namespace Lte.Domain.Common
{
    public class FlowHuaweiCsv
    {
        [CsvColumn(Name = "��ʼʱ��")]
        public DateTime StatTime { get; set; }

        [CsvColumn(Name = "С��")]
        public string CellInfo { get; set; }

        public int ENodebId
        {
            get
            {
                if (string.IsNullOrEmpty(CellInfo)) return 0;
                var fields = CellInfo.GetSplittedFields(", ");
                return fields.Length == 0 ? 0 : fields[3].GetSplittedFields('=')[1].ConvertToInt(0);
            }
        }

        public byte LocalCellId
        {
            get
            {
                if (string.IsNullOrEmpty(CellInfo)) return 0;
                var fields = CellInfo.GetSplittedFields(", ");
                return fields.Length == 0 ? (byte)0 : fields[1].GetSplittedFields('=')[1].ConvertToByte(0);
            }
        }

        [CsvColumn(Name = "С��PDCP�������͵��������ݵ��������� (����)")]
        public long PdcpDownlinkFlowInByte { get; set; }

        [CsvColumn(Name = "С��PDCP�������յ����������ݵ��������� (����)")]
        public long PdcpUplinkFlowInByte { get; set; }

        [CsvColumn(Name = "С���ڵ�ƽ���û��� (��)")]
        public double AverageUsers { get; set; }

        [CsvColumn(Name = "С���ڵ�����û��� (��)")]
        public int MaxUsers { get; set; }

        [CsvColumn(Name = "ƽ�������û��� (��)")]
        public double AverageActiveUsers { get; set; }

        [CsvColumn(Name = "��󼤻��û��� (��)")]
        public int MaxActiveUsers { get; set; }

        [CsvColumn(Name = "����ƽ�������û��� (��)")]
        public double UplinkAverageUsers { get; set; }

        [CsvColumn(Name = "������󼤻��û��� (��)")]
        public int UplinkMaxUsers { get; set; }

        [CsvColumn(Name = "����ƽ�������û��� (��)")]
        public double DownlinkAverageUsers { get; set; }

        [CsvColumn(Name = "������󼤻��û��� (��)")]
        public int DownlinkMaxUsers { get; set; }

        [CsvColumn(Name = "С�����������ݴ�����ʱ��(1ms����) (����)")]
        public int DownlinkDurationInMs { get; set; }

        [CsvColumn(Name = "С�����������ݴ�����ʱ��(1ms����) (����)")]
        public int UplinkDurationInMs { get; set; }

        [CsvColumn(Name = "С��Uu�ӿ�Ѱ���û����� (��)")]
        public string PagingUsersString { get; set; }

        [CsvColumn(Name = "����Physical Resource Block��ʹ�õ�ƽ������ (��)")]
        public double DownlinkAveragePrbs { get; set; }

        [CsvColumn(Name = "����PDSCH DRB��Physical Resource Block��ʹ�õ�ƽ������ (��)")]
        public double DownlinkDrbPbs { get; set; }

        [CsvColumn(Name = "����Physical Resource Block��ʹ�õ�ƽ������ (��)")]
        public double UplinkAveragePrbs { get; set; }

        [CsvColumn(Name = "����PUSCH DRB��Physical Resource Block��ʹ�õ�ƽ������ (��)")]
        public double UplinkDrbPbs { get; set; }

        [CsvColumn(Name = "С�����յ�����Group A��Preamble��Ϣ���� (��)")]
        public int GroupAPreambles { get; set; }

        [CsvColumn(Name = "С�����յ�����Group B��Preamble��Ϣ�Ĵ��� (��)")]
        public int GroupBPreambles { get; set; }

        [CsvColumn(Name = "С�����յ�ר��ǰ����Ϣ�Ĵ��� (��)")]
        public int DedicatedPreambles { get; set; }

        [CsvColumn(Name = "ͳ������������DCI��ʹ�õ�PDCCH CCE���� (��)")]
        public long UplinkDciCces { get; set; }

        public double UplinkDciCceRate => TotalCces == 0 ? 0 : (double)UplinkDciCces / TotalCces;

        [CsvColumn(Name = "ͳ������������DCI��ʹ�õ�PDCCH CCE���� (��)")]
        public long DownlinkDciCces { get; set; }

        public double DownlinkDciCceRate => TotalCces == 0 ? 0 : (double)DownlinkDciCces / TotalCces;

        [CsvColumn(Name = "ͳ�������ڿ��õ�PDCCH CCE�ĸ��� (��)")]
        public long TotalCces { get; set; }

        [CsvColumn(Name = "PUCCH��PRB��Դ�����ƽ��ֵ (��)")]
        public string PucchPrbsString { get; set; }

        [CsvColumn(Name = "ʹUE����Ϊ�յ����һ��TTI����������PDCP������ (����)")]
        public long LastTtiUplinkFlowInByte { get; set; }

        [CsvColumn(Name = "�۳�ʹUE����Ϊ�յ����һ��TTI֮�����������ʱ�� (����)")]
        public int ButLastUplinkDurationInMs { get; set; }

        [CsvColumn(Name = "ʹ����Ϊ�յ����һ��TTI����������PDCP������ (����)")]
        public long LastTtiDownlinkFlowInByte { get; set; }

        [CsvColumn(Name = "�۳�ʹ���л���Ϊ�յ����һ��TTI֮�������ʱ�� (����)")]
        public int ButLastDownlinkDurationInMs { get; set; }

        [CsvColumn(Name = "RANK1���ϱ����� (��)")]
        public string SchedulingRank1String { get; set; }

        [CsvColumn(Name = "RANK2���ϱ����� (��)")]
        public string SchedulingRank2String { get; set; }

        [CsvColumn(Name = "�ض���CDMA2000 HRPD���ܴ��� (��)")]
        public int RedirectCdma2000 { get; set; }

        [CsvColumn(Name = "emergencyԭ���RRC���ӽ������Դ��� (��)")]
        public int EmergencyRrcRequest { get; set; }

        [CsvColumn(Name = "emergencyԭ���RRC���ӽ������Դ���(�����ط�) (��)")]
        public int EmergencyRrcRequestAll { get; set; }

        [CsvColumn(Name = "emergencyԭ���RRC���ӽ����ɹ����� (��)")]
        public int EmergencyRrcSuccess { get; set; }

        [CsvColumn(Name = "highPriorityAccessԭ���RRC���ӽ������Դ��� (��)")]
        public int HighPriorityRrcRequest { get; set; }

        [CsvColumn(Name = "highPriorityAccessԭ���RRC���ӽ������Դ���(�����ط�) (��)")]
        public int HighPriorityRrcRequestAll { get; set; }

        [CsvColumn(Name = "highPriorityAccessԭ���RRC���ӽ����ɹ����� (��)")]
        public int HighPriorityRrcSuccess { get; set; }

        [CsvColumn(Name = "mo-Dataԭ���RRC���ӽ������Դ��� (��)")]
        public int MoDataRrcRequest { get; set; }

        [CsvColumn(Name = "mo-Dataԭ���RRC���ӽ������Դ���(�����ط�) (��)")]
        public int MoDataRrcRequestAll { get; set; }

        [CsvColumn(Name = "mo-Dataԭ���RRC���ӽ����ɹ����� (��)")]
        public int MoDataRrcSuccess { get; set; }

        [CsvColumn(Name = "mo-Signallingԭ���RRC���ӽ������Դ��� (��)")]
        public int MoSignallingRrcRequest { get; set; }

        [CsvColumn(Name = "mo-Signallingԭ���RRC���ӽ������Դ���(�����ط�) (��)")]
        public int MoSignallingRrcRequestAll { get; set; }

        [CsvColumn(Name = "mo-Signallingԭ���RRC���ӽ����ɹ����� (��)")]
        public int MoSignallingRrcSuccess { get; set; }

        [CsvColumn(Name = "mt-Accessԭ���RRC���ӽ������Դ��� (��)")]
        public int MtAccessRrcRequest { get; set; }

        [CsvColumn(Name = "mt-Accessԭ���RRC���ӽ������Դ���(�����ط�) (��)")]
        public int MtAccessRrcRequestAll { get; set; }

        [CsvColumn(Name = "mt-Accessԭ���RRC���ӽ����ɹ����� (��)")]
        public int MtAccessRrcSuccess { get; set; }

        [CsvColumn(Name = "������Դ����ʧ�ܶ�����RRC���ӽ���ʧ�ܵĴ��� (��)")]
        public int RrcFailOtherResource { get; set; }

        [CsvColumn(Name = "�û���������޵��µ�RRC���ӽ���ʧ�ܴ��� (��)")]
        public int RrcFailUserLimit { get; set; }

        [CsvColumn(Name = "����ԭ���µ�RRC���ӽ������ܾ��Ĵ��� (��)")]
        public int RrcRejectFail { get; set; }

        [CsvColumn(Name = "MME���ص��µķ���RRC Connection Reject��Ϣ���� (��)")]
        public int RrcRejectOverload { get; set; }

        [CsvColumn(Name = "���ص��µ�RRC�ؽ�������Ϣ�������� (��)")]
        public int RrcReconstructionLostFlowControl { get; set; }

        [CsvColumn(Name = "���ص��µ�RRC Connection Request ��Ϣ�������� (��)")]
        public int RrcRequestLostFlowControl { get; set; }

        [CsvColumn(Name = "��Դ����ʧ�ܶ�����RRC���ӽ���ʧ�ܴ��� (��)")]
        public int RrcFailResourceAssignment { get; set; }

        [CsvColumn(Name = "UE��Ӧ�������RRC���ӽ���ʧ�ܴ��� (��)")]
        public int RrcFailUeNoAnswer { get; set; }

        [CsvColumn(Name = "��ΪSRS��Դ����ʧ�ܶ�����RRC���ӽ���ʧ�ܵĴ��� (��)")]
        public int RrcFailSrsAssignment { get; set; }

        [CsvColumn(Name = "��ΪPUCCH��Դ����ʧ�ܶ�����RRC���ӽ���ʧ�ܵĴ��� (��)")]
        public int RrcFailPucchAssignment { get; set; }

        [CsvColumn(Name = "���ص��µķ���RRC Connection Reject��Ϣ���� (��)")]
        public int RrcRejectFlowControl { get; set; }

        [CsvColumn(Name = "�տ��ϱ�ȫ����CQIΪ0�Ĵ��� (��)")]
        public int Cqi0Times { get; set; }

        [CsvColumn(Name = "�տ��ϱ�ȫ����CQIΪ1�Ĵ��� (��)")]
        public int Cqi1Times { get; set; }

        [CsvColumn(Name = "�տ��ϱ�ȫ����CQIΪ2�Ĵ��� (��)")]
        public int Cqi2Times { get; set; }

        [CsvColumn(Name = "�տ��ϱ�ȫ����CQIΪ3�Ĵ��� (��)")]
        public int Cqi3Times { get; set; }

        [CsvColumn(Name = "�տ��ϱ�ȫ����CQIΪ4�Ĵ��� (��)")]
        public int Cqi4Times { get; set; }

        [CsvColumn(Name = "�տ��ϱ�ȫ����CQIΪ5�Ĵ��� (��)")]
        public int Cqi5Times { get; set; }

        [CsvColumn(Name = "�տ��ϱ�ȫ����CQIΪ6�Ĵ��� (��)")]
        public int Cqi6Times { get; set; }

        [CsvColumn(Name = "�տ��ϱ�ȫ����CQIΪ7�Ĵ��� (��)")]
        public int Cqi7Times { get; set; }

        [CsvColumn(Name = "�տ��ϱ�ȫ����CQIΪ8�Ĵ��� (��)")]
        public int Cqi8Times { get; set; }

        [CsvColumn(Name = "�տ��ϱ�ȫ����CQIΪ9�Ĵ��� (��)")]
        public int Cqi9Times { get; set; }

        [CsvColumn(Name = "�տ��ϱ�ȫ����CQIΪ10�Ĵ��� (��)")]
        public int Cqi10Times { get; set; }

        [CsvColumn(Name = "�տ��ϱ�ȫ����CQIΪ11�Ĵ��� (��)")]
        public int Cqi11Times { get; set; }

        [CsvColumn(Name = "�տ��ϱ�ȫ����CQIΪ12�Ĵ��� (��)")]
        public int Cqi12Times { get; set; }

        [CsvColumn(Name = "�տ��ϱ�ȫ����CQIΪ13�Ĵ��� (��)")]
        public int Cqi13Times { get; set; }

        [CsvColumn(Name = "�տ��ϱ�ȫ����CQIΪ14�Ĵ��� (��)")]
        public int Cqi14Times { get; set; }

        [CsvColumn(Name = "�տ��ϱ�ȫ����CQIΪ15�Ĵ��� (��)")]
        public int Cqi15Times { get; set; }

        public static List<FlowHuaweiCsv> ReadFlowHuaweiCsvs(StreamReader reader)
        {
            return CsvContext.Read<FlowHuaweiCsv>(reader, CsvFileDescription.CommaDescription).ToList();
        }
    }
}