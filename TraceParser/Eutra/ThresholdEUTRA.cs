﻿using Lte.Domain.Common;
using System;

namespace TraceParser.Eutra
{
    [Serializable]
    public class ThresholdEUTRA
    {
        public void InitDefaults()
        {
        }

        public long threshold_RSRP { get; set; }

        public long threshold_RSRQ { get; set; }

        public class PerDecoder
        {
            public static readonly PerDecoder Instance = new PerDecoder();

            public ThresholdEUTRA Decode(BitArrayInputStream input)
            {
                ThresholdEUTRA deutra = new ThresholdEUTRA();
                deutra.InitDefaults();
                switch (input.ReadBits(1))
                {
                    case 0:
                        deutra.threshold_RSRP = input.ReadBits(7);
                        return deutra;

                    case 1:
                        deutra.threshold_RSRQ = input.ReadBits(6);
                        return deutra;
                }
                throw new Exception(GetType().Name + ":NoChoice had been choose");
            }
        }
    }

    [Serializable]
    public class ThresholdUTRA
    {
        public void InitDefaults()
        {
        }

        public long utra_EcN0 { get; set; }

        public long utra_RSCP { get; set; }

        public class PerDecoder
        {
            public static readonly PerDecoder Instance = new PerDecoder();

            public ThresholdUTRA Decode(BitArrayInputStream input)
            {
                ThresholdUTRA dutra = new ThresholdUTRA();
                dutra.InitDefaults();
                switch (input.ReadBits(1))
                {
                    case 0:
                        dutra.utra_RSCP = input.ReadBits(7) + -5;
                        return dutra;

                    case 1:
                        dutra.utra_EcN0 = input.ReadBits(6);
                        return dutra;
                }
                throw new Exception(GetType().Name + ":NoChoice had been choose");
            }
        }
    }

}
