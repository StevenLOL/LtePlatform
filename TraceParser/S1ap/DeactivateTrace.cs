﻿using Lte.Domain.Common;
using System;
using System.Collections.Generic;

namespace TraceParser.S1ap
{
    [Serializable]
    public class DeactivateTrace
    {
        public void InitDefaults()
        {
        }

        public List<ProtocolIE_Field> protocolIEs { get; set; }

        public class PerDecoder
        {
            public static readonly PerDecoder Instance = new PerDecoder();

            public DeactivateTrace Decode(BitArrayInputStream input)
            {
                DeactivateTrace trace = new DeactivateTrace();
                trace.InitDefaults();
                input.ReadBit();
                input.skipUnreadedBits();
                trace.protocolIEs = new List<ProtocolIE_Field>();
                const int nBits = 0x10;
                int num5 = input.ReadBits(nBits);
                for (int i = 0; i < num5; i++)
                {
                    ProtocolIE_Field item = ProtocolIE_Field.PerDecoder.Instance.Decode(input);
                    trace.protocolIEs.Add(item);
                }
                return trace;
            }
        }
    }
}
