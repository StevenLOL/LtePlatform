﻿using Lte.Domain.Common;
using System;

namespace TraceParser.X2ap
{
    [Serializable]
    public class InitiatingMessage
    {
        public void InitDefaults()
        {
        }

        public Criticality criticality { get; set; }

        public long procedureCode { get; set; }

        public object value { get; set; }

        public class PerDecoder
        {
            public static readonly PerDecoder Instance = new PerDecoder();

            public InitiatingMessage Decode(BitArrayInputStream input)
            {
                InitiatingMessage message = new InitiatingMessage();
                message.InitDefaults();
                input.skipUnreadedBits();
                message.procedureCode = input.ReadBits(8);
                const int num4 = 2;
                message.criticality = (Criticality)input.ReadBits(num4);
                input.skipUnreadedBits();
                int nBits = 0;
                while (true)
                {
                    switch (input.ReadBit())
                    {
                        case 0:
                            nBits += input.ReadBits(7);
                            goto Label_00CF;

                        case 1:
                            switch (input.ReadBit())
                            {
                                case 0:
                                    nBits += input.ReadBits(14);
                                    goto Label_00CF;

                                case 1:
                                    input.ReadBits(2);
                                    nBits += input.ReadBits(4) * 0x400;
                                    break;
                            }
                            break;
                    }
                }
            Label_00CF:
                long num3 = input.Position;
                try
                {
                    message.value = X2AP_ELEMENTARY_PROCEDURE.Switcher(message.procedureCode, "InitiatingMessage", input);
                    input.skipUnreadedBits();
                }
                catch (Exception)
                {
                    input.skipUnreadedBits();
                    input.Position = num3;
                    message.value = input.readOctetString(nBits);
                }
                if (input.Position != (num3 + nBits))
                {
                    input.Position = num3 + nBits;
                }
                return message;
            }
        }
    }
}
