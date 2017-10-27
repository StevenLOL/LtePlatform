using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lte.Domain.LinqToCsv;
using Lte.Domain.LinqToCsv.Context;
using Lte.Domain.LinqToCsv.Description;
using Lte.Domain.Regular;
using Lte.Domain.Regular.Attributes;

namespace Lte.Domain.Common
{
    public class FlowHuaweiCsv
    {
        [CsvColumn(Name = "��ʼʱ��")]
        [ArraySumProtection]
        public DateTime StatTime { get; set; }

        [CsvColumn(Name = "С��")]
        [ArraySumProtection]
        public string CellInfo { get; set; }
        
        public int ENodebId
        {
            get
            {
                if (string.IsNullOrEmpty(CellInfo)) return 0;
                var fields = CellInfo.GetSplittedFields(", ");
                return fields.Length < 4 ? 0 : fields[3].GetSplittedFields('=')[1].ConvertToInt(0);
            }
        }
        
        public byte LocalCellId
        {
            get
            {
                if (string.IsNullOrEmpty(CellInfo)) return 0;
                var fields = CellInfo.GetSplittedFields(", ");
                return fields.Length < 4 ? (byte)0 : fields[1].GetSplittedFields('=')[1].ConvertToByte(0);
            }
        }

        public byte SectorId
        {
            get
            {
                if (string.IsNullOrEmpty(CellInfo)) return 0;
                var fields = CellInfo.GetSplittedFields(", ");
                return fields.Length < 6 ? (byte)0 : fields[5].GetSplittedFields('=')[1].ConvertToByte(0);
            }
        }

        [CsvColumn(Name = "С��PDCP�������͵��������ݵ��������� (����)")]
        public long PdcpDownlinkFlowInByte { get; set; }

        [CsvColumn(Name = "С��PDCP�������յ����������ݵ��������� (����)")]
        public long PdcpUplinkFlowInByte { get; set; }

        [CsvColumn(Name = "С���ڵ�ƽ���û��� (��)")]
        [ArrayAverage]
        public double AverageUsers { get; set; }

        [CsvColumn(Name = "С���ڵ�����û��� (��)")]
        [ArrayMax]
        public int MaxUsers { get; set; }

        [CsvColumn(Name = "ƽ�������û��� (��)")]
        [ArrayAverage]
        public double AverageActiveUsers { get; set; }

        [CsvColumn(Name = "��󼤻��û��� (��)")]
        [ArrayMax]
        public int MaxActiveUsers { get; set; }

        [CsvColumn(Name = "����ƽ�������û��� (��)")]
        [ArrayAverage]
        public double UplinkAverageUsers { get; set; }

        [CsvColumn(Name = "������󼤻��û��� (��)")]
        [ArrayMax]
        public int UplinkMaxUsers { get; set; }

        [CsvColumn(Name = "����ƽ�������û��� (��)")]
        [ArrayAverage]
        public double DownlinkAverageUsers { get; set; }

        [CsvColumn(Name = "������󼤻��û��� (��)")]
        [ArrayMax]
        public int DownlinkMaxUsers { get; set; }

        [CsvColumn(Name = "С�����������ݴ�����ʱ��(1ms����) (����)")]
        [ArraySumProtection]
        public string DownlinkDurationInMsString { get; set; }

        public int DownlinkDurationInMs
        {
            get { return DownlinkDurationInMsString.ConvertToInt(0); }
            set { DownlinkDurationInMsString = value.ToString(); }
        }

        [CsvColumn(Name = "С�����������ݴ�����ʱ��(1ms����) (����)")]
        [ArraySumProtection]
        public string UplinkDurationInMsString { get; set; }

        public int UplinkDurationInMs
        {
            get { return UplinkDurationInMsString.ConvertToInt(0); }
            set { UplinkDurationInMsString = value.ToString(); }
        }

        [CsvColumn(Name = "С��Uu�ӿ�Ѱ���û����� (��)")]
        public string PagingUsersString { get; set; }

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
        [ArraySumProtection]
        public string LastTtiUplinkFlowInByteString { get; set; }

        public long LastTtiUplinkFlowInByte
        {
            get { return LastTtiUplinkFlowInByteString.ConvertToLong(0); }
            set { LastTtiUplinkFlowInByteString = value.ToString(); }
        }
        

        [CsvColumn(Name = "�۳�ʹUE����Ϊ�յ����һ��TTI֮�����������ʱ�� (����)")]
        [ArraySumProtection]
        public string ButLastUplinkDurationInMsString { get; set; }

        public int ButLastUplinkDurationInMs
        {
            get { return ButLastUplinkDurationInMsString.ConvertToInt(0); }
            set { ButLastUplinkDurationInMsString = value.ToString(); }
        } 

        [CsvColumn(Name = "ʹ����Ϊ�յ����һ��TTI����������PDCP������ (����)")]
        [ArraySumProtection]
        public string LastTtiDownlinkFlowInByteString { get; set; }

        public long LastTtiDownlinkFlowInByte {
            get { return LastTtiDownlinkFlowInByteString.ConvertToLong(0); }
            set { LastTtiDownlinkFlowInByteString = value.ToString(); }
        }

        [CsvColumn(Name = "�۳�ʹ���л���Ϊ�յ����һ��TTI֮�������ʱ�� (����)")]
        [ArraySumProtection]
        public string ButLastDownlinkDurationInMsString { get; set; }

        public int ButLastDownlinkDurationInMs
        {
            get { return ButLastDownlinkDurationInMsString.ConvertToInt(0); }
            set { ButLastDownlinkDurationInMsString = value.ToString(); }
        }

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

        [CsvColumn(Name = "����Physical Resource Block��ʹ�õ�ƽ������ (��)")]
        public double PdschPrbs { get; set; }

        [CsvColumn(Name = "����PDSCH DRB��Physical Resource Block��ʹ�õ�ƽ������ (��)")]
        public double DownlinkDtchPrbNumber { get; set; }

        [CsvColumn(Name = "���п��õ�PRB���� (��)")]
        public int DownlinkPrbSubframe { get; set; }

        [CsvColumn(Name = "����Physical Resource Block��ʹ�õ�ƽ������ (��)")]
        public double PuschPrbs { get; set; }

        [CsvColumn(Name = "����PUSCH DRB��Physical Resource Block��ʹ�õ�ƽ������ (��)")]
        public double UplinkDtchPrbNumber { get; set; }

        [CsvColumn(Name = "���п��õ�PRB���� (��)")]
        public int UplinkPrbSubframe { get; set; }

        [CsvColumn(Name = "PDSCH��PRB��Դ������������0�ڵ������� (��)")]
        public double PdschUsageInterval0Seconds { get; set; }

        [CsvColumn(Name = "PDSCH��PRB��Դ������������1�ڵ������� (��)")]
        public double PdschUsageInterval10Seconds { get; set; }

        [CsvColumn(Name = "PDSCH��PRB��Դ������������2�ڵ������� (��)")]
        public double PdschUsageInterval20Seconds { get; set; }

        [CsvColumn(Name = "PDSCH��PRB��Դ������������3�ڵ������� (��)")]
        public double PdschUsageInterval30Seconds { get; set; }

        [CsvColumn(Name = "PDSCH��PRB��Դ������������4�ڵ������� (��)")]
        public double PdschUsageInterval40Seconds { get; set; }

        [CsvColumn(Name = "PDSCH��PRB��Դ������������5�ڵ������� (��)")]
        public double PdschUsageInterval50Seconds { get; set; }

        [CsvColumn(Name = "PDSCH��PRB��Դ������������6�ڵ������� (��)")]
        public double PdschUsageInterval60Seconds { get; set; }

        [CsvColumn(Name = "PDSCH��PRB��Դ������������7�ڵ������� (��)")]
        public double PdschUsageInterval70Seconds { get; set; }

        [CsvColumn(Name = "PDSCH��PRB��Դ������������8�ڵ������� (��)")]
        public double PdschUsageInterval80Seconds { get; set; }

        [CsvColumn(Name = "PDSCH��PRB��Դ������������9�ڵ������� (��)")]
        public double PdschUsageInterval90Seconds { get; set; }

        [CsvColumn(Name = "PUSCH��PRB��Դ������������0�ڵ������� (��)")]
        public double PuschUsageInterval0Seconds { get; set; }

        [CsvColumn(Name = "PUSCH��PRB��Դ������������1�ڵ������� (��)")]
        public double PuschUsageInterval10Seconds { get; set; }

        [CsvColumn(Name = "PUSCH��PRB��Դ������������2�ڵ������� (��)")]
        public double PuschUsageInterval20Seconds { get; set; }

        [CsvColumn(Name = "PUSCH��PRB��Դ������������3�ڵ������� (��)")]
        public double PuschUsageInterval30Seconds { get; set; }

        [CsvColumn(Name = "PUSCH��PRB��Դ������������4�ڵ������� (��)")]
        public double PuschUsageInterval40Seconds { get; set; }

        [CsvColumn(Name = "PUSCH��PRB��Դ������������5�ڵ������� (��)")]
        public double PuschUsageInterval50Seconds { get; set; }

        [CsvColumn(Name = "PUSCH��PRB��Դ������������6�ڵ������� (��)")]
        public double PuschUsageInterval60Seconds { get; set; }

        [CsvColumn(Name = "PUSCH��PRB��Դ������������7�ڵ������� (��)")]
        public double PuschUsageInterval70Seconds { get; set; }

        [CsvColumn(Name = "PUSCH��PRB��Դ������������8�ڵ������� (��)")]
        public double PuschUsageInterval80Seconds { get; set; }

        [CsvColumn(Name = "PUSCH��PRB��Դ������������9�ڵ������� (��)")]
        public double PuschUsageInterval90Seconds { get; set; }

        public static List<FlowHuaweiCsv> ReadFlowHuaweiCsvs(StreamReader reader)
        {
            return CsvContext.Read<FlowHuaweiCsv>(reader, CsvFileDescription.CommaDescription).ToList();
        }
    }
}