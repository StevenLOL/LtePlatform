﻿using System;
using Lte.Domain.Common;

namespace TraceParser.Eutra
{
    [Serializable]
    public class BandClassInfoCDMA2000
    {
        public void InitDefaults()
        {
        }

        public BandclassCDMA2000 bandClass { get; set; }

        public long? cellReselectionPriority { get; set; }

        public long threshX_High { get; set; }

        public long threshX_Low { get; set; }

        public class PerDecoder
        {
            public static readonly PerDecoder Instance = new PerDecoder();

            public BandClassInfoCDMA2000 Decode(BitArrayInputStream input)
            {
                BandClassInfoCDMA2000 ocdma = new BandClassInfoCDMA2000();
                ocdma.InitDefaults();
                BitMaskStream stream = (input.ReadBit() != 0) ? new BitMaskStream(input, 1) : new BitMaskStream(input, 1);
                int nBits = (input.ReadBit() == 0) ? 5 : 5;
                ocdma.bandClass = (BandclassCDMA2000)input.ReadBits(nBits);
                if (stream.Read())
                {
                    ocdma.cellReselectionPriority = input.ReadBits(3);
                }
                ocdma.threshX_High = input.ReadBits(6);
                ocdma.threshX_Low = input.ReadBits(6);
                return ocdma;
            }
        }
    }

    [Serializable]
    public class BandClassPriority1XRTT
    {
        public void InitDefaults()
        {
        }

        public BandclassCDMA2000 bandClass { get; set; }

        public long cellReselectionPriority { get; set; }

        public class PerDecoder
        {
            public static readonly PerDecoder Instance = new PerDecoder();

            public BandClassPriority1XRTT Decode(BitArrayInputStream input)
            {
                BandClassPriority1XRTT priorityxrtt = new BandClassPriority1XRTT();
                priorityxrtt.InitDefaults();
                int nBits = (input.ReadBit() == 0) ? 5 : 5;
                priorityxrtt.bandClass = (BandclassCDMA2000)input.ReadBits(nBits);
                priorityxrtt.cellReselectionPriority = input.ReadBits(3);
                return priorityxrtt;
            }
        }
    }

    [Serializable]
    public class BandClassPriorityHRPD
    {
        public void InitDefaults()
        {
        }

        public BandclassCDMA2000 bandClass { get; set; }

        public long cellReselectionPriority { get; set; }

        public class PerDecoder
        {
            public static readonly PerDecoder Instance = new PerDecoder();

            public BandClassPriorityHRPD Decode(BitArrayInputStream input)
            {
                BandClassPriorityHRPD yhrpd = new BandClassPriorityHRPD();
                yhrpd.InitDefaults();
                int nBits = (input.ReadBit() == 0) ? 5 : 5;
                yhrpd.bandClass = (BandclassCDMA2000)input.ReadBits(nBits);
                yhrpd.cellReselectionPriority = input.ReadBits(3);
                return yhrpd;
            }
        }
    }

    [Serializable]
    public class SupportedBandEUTRA
    {
        public void InitDefaults()
        {
        }

        public long bandEUTRA { get; set; }

        public bool halfDuplex { get; set; }

        public class PerDecoder
        {
            public static readonly PerDecoder Instance = new PerDecoder();

            public SupportedBandEUTRA Decode(BitArrayInputStream input)
            {
                SupportedBandEUTRA deutra = new SupportedBandEUTRA();
                deutra.InitDefaults();
                deutra.bandEUTRA = input.ReadBits(6) + 1;
                deutra.halfDuplex = input.ReadBit() == 1;
                return deutra;
            }
        }
    }

    [Serializable]
    public class SupportedBandEUTRA_v9e0
    {
        public void InitDefaults()
        {
        }

        public long? bandEUTRA_v9e0 { get; set; }

        public class PerDecoder
        {
            public static readonly PerDecoder Instance = new PerDecoder();

            public SupportedBandEUTRA_v9e0 Decode(BitArrayInputStream input)
            {
                SupportedBandEUTRA_v9e0 _ve = new SupportedBandEUTRA_v9e0();
                _ve.InitDefaults();
                BitMaskStream stream = new BitMaskStream(input, 1);
                if (stream.Read())
                {
                    _ve.bandEUTRA_v9e0 = input.ReadBits(8) + 0x41;
                }
                return _ve;
            }
        }
    }

}
