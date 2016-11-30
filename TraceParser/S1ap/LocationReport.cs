﻿using Lte.Domain.Common;
using System;
using System.Collections.Generic;

namespace TraceParser.S1ap
{
    [Serializable]
    public class LocationReport
    {
        public void InitDefaults()
        {
        }

        public List<ProtocolIE_Field> protocolIEs { get; set; }

        public class PerDecoder
        {
            public static readonly PerDecoder Instance = new PerDecoder();

            public LocationReport Decode(BitArrayInputStream input)
            {
                LocationReport report = new LocationReport();
                report.InitDefaults();
                input.ReadBit();
                input.skipUnreadedBits();
                report.protocolIEs = new List<ProtocolIE_Field>();
                const int nBits = 0x10;
                int num5 = input.ReadBits(nBits);
                for (int i = 0; i < num5; i++)
                {
                    ProtocolIE_Field item = ProtocolIE_Field.PerDecoder.Instance.Decode(input);
                    report.protocolIEs.Add(item);
                }
                return report;
            }
        }
    }

    [Serializable]
    public class LocationReportingControl
    {
        public void InitDefaults()
        {
        }

        public List<ProtocolIE_Field> protocolIEs { get; set; }

        public class PerDecoder
        {
            public static readonly PerDecoder Instance = new PerDecoder();

            public LocationReportingControl Decode(BitArrayInputStream input)
            {
                LocationReportingControl control = new LocationReportingControl();
                control.InitDefaults();
                input.ReadBit();
                input.skipUnreadedBits();
                control.protocolIEs = new List<ProtocolIE_Field>();
                const int nBits = 0x10;
                int num5 = input.ReadBits(nBits);
                for (int i = 0; i < num5; i++)
                {
                    ProtocolIE_Field item = ProtocolIE_Field.PerDecoder.Instance.Decode(input);
                    control.protocolIEs.Add(item);
                }
                return control;
            }
        }
    }

    [Serializable]
    public class LocationReportingFailureIndication
    {
        public void InitDefaults()
        {
        }

        public List<ProtocolIE_Field> protocolIEs { get; set; }

        public class PerDecoder
        {
            public static readonly PerDecoder Instance = new PerDecoder();

            public LocationReportingFailureIndication Decode(BitArrayInputStream input)
            {
                LocationReportingFailureIndication indication = new LocationReportingFailureIndication();
                indication.InitDefaults();
               input.ReadBit();
                input.skipUnreadedBits();
                indication.protocolIEs = new List<ProtocolIE_Field>();
                int nBits = 0x10;
                int num5 = input.ReadBits(nBits);
                for (int i = 0; i < num5; i++)
                {
                    ProtocolIE_Field item = ProtocolIE_Field.PerDecoder.Instance.Decode(input);
                    indication.protocolIEs.Add(item);
                }
                return indication;
            }
        }
    }

}
