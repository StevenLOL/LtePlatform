﻿using Lte.Domain.Common;
using System;

namespace TraceParser.Eutra
{
    [Serializable]
    public class RRCConnectionReestablishmentComplete
    {
        public void InitDefaults()
        {
        }

        public criticalExtensions_Type criticalExtensions { get; set; }

        public long rrc_TransactionIdentifier { get; set; }

        [Serializable]
        public class criticalExtensions_Type
        {
            public void InitDefaults()
            {
            }

            public criticalExtensionsFuture_Type criticalExtensionsFuture { get; set; }

            public RRCConnectionReestablishmentComplete_r8_IEs rrcConnectionReestablishmentComplete_r8 { get; set; }

            [Serializable]
            public class criticalExtensionsFuture_Type
            {
                public void InitDefaults()
                {
                }

                public class PerDecoder
                {
                    public static readonly PerDecoder Instance = new PerDecoder();

                    public criticalExtensionsFuture_Type Decode(BitArrayInputStream input)
                    {
                        criticalExtensionsFuture_Type type = new criticalExtensionsFuture_Type();
                        type.InitDefaults();
                        return type;
                    }
                }
            }

            public class PerDecoder
            {
                public static readonly PerDecoder Instance = new PerDecoder();

                public criticalExtensions_Type Decode(BitArrayInputStream input)
                {
                    criticalExtensions_Type type = new criticalExtensions_Type();
                    type.InitDefaults();
                    switch (input.ReadBits(1))
                    {
                        case 0:
                            type.rrcConnectionReestablishmentComplete_r8 
                                = RRCConnectionReestablishmentComplete_r8_IEs.PerDecoder.Instance.Decode(input);
                            return type;

                        case 1:
                            type.criticalExtensionsFuture = criticalExtensionsFuture_Type.PerDecoder.Instance.Decode(input);
                            return type;
                    }
                    throw new Exception(GetType().Name + ":NoChoice had been choose");
                }
            }
        }

        public class PerDecoder
        {
            public static readonly PerDecoder Instance = new PerDecoder();

            public RRCConnectionReestablishmentComplete Decode(BitArrayInputStream input)
            {
                RRCConnectionReestablishmentComplete complete = new RRCConnectionReestablishmentComplete();
                complete.InitDefaults();
                complete.rrc_TransactionIdentifier = input.ReadBits(2);
                complete.criticalExtensions = criticalExtensions_Type.PerDecoder.Instance.Decode(input);
                return complete;
            }
        }
    }

    [Serializable]
    public class RRCConnectionReestablishmentComplete_r8_IEs
    {
        public void InitDefaults()
        {
        }

        public RRCConnectionReestablishmentComplete_v920_IEs nonCriticalExtension { get; set; }

        public class PerDecoder
        {
            public static readonly PerDecoder Instance = new PerDecoder();

            public RRCConnectionReestablishmentComplete_r8_IEs Decode(BitArrayInputStream input)
            {
                RRCConnectionReestablishmentComplete_r8_IEs es = new RRCConnectionReestablishmentComplete_r8_IEs();
                es.InitDefaults();
                BitMaskStream stream = new BitMaskStream(input, 1);
                if (stream.Read())
                {
                    es.nonCriticalExtension = RRCConnectionReestablishmentComplete_v920_IEs.PerDecoder.Instance.Decode(input);
                }
                return es;
            }
        }
    }

    [Serializable]
    public class RRCConnectionReestablishmentComplete_v1020_IEs
    {
        public void InitDefaults()
        {
        }

        public logMeasAvailable_r10_Enum? logMeasAvailable_r10 { get; set; }

        public RRCConnectionReestablishmentComplete_v1130_IEs nonCriticalExtension { get; set; }

        public enum logMeasAvailable_r10_Enum
        {
            _true
        }

        public class PerDecoder
        {
            public static readonly PerDecoder Instance = new PerDecoder();

            public RRCConnectionReestablishmentComplete_v1020_IEs Decode(BitArrayInputStream input)
            {
                RRCConnectionReestablishmentComplete_v1020_IEs es = new RRCConnectionReestablishmentComplete_v1020_IEs();
                es.InitDefaults();
                BitMaskStream stream = new BitMaskStream(input, 2);
                if (stream.Read())
                {
                    const int nBits = 1;
                    es.logMeasAvailable_r10 = (logMeasAvailable_r10_Enum)input.ReadBits(nBits);
                }
                if (stream.Read())
                {
                    es.nonCriticalExtension = RRCConnectionReestablishmentComplete_v1130_IEs.PerDecoder.Instance.Decode(input);
                }
                return es;
            }
        }
    }

    [Serializable]
    public class RRCConnectionReestablishmentComplete_v1130_IEs
    {
        public void InitDefaults()
        {
        }

        public connEstFailInfoAvailable_r11_Enum? connEstFailInfoAvailable_r11 { get; set; }

        public nonCriticalExtension_Type nonCriticalExtension { get; set; }

        public enum connEstFailInfoAvailable_r11_Enum
        {
            _true
        }

        [Serializable]
        public class nonCriticalExtension_Type
        {
            public void InitDefaults()
            {
            }

            public class PerDecoder
            {
                public static readonly PerDecoder Instance = new PerDecoder();

                public nonCriticalExtension_Type Decode(BitArrayInputStream input)
                {
                    nonCriticalExtension_Type type = new nonCriticalExtension_Type();
                    type.InitDefaults();
                    return type;
                }
            }
        }

        public class PerDecoder
        {
            public static readonly PerDecoder Instance = new PerDecoder();

            public RRCConnectionReestablishmentComplete_v1130_IEs Decode(BitArrayInputStream input)
            {
                RRCConnectionReestablishmentComplete_v1130_IEs es = new RRCConnectionReestablishmentComplete_v1130_IEs();
                es.InitDefaults();
                BitMaskStream stream = new BitMaskStream(input, 2);
                if (stream.Read())
                {
                    const int nBits = 1;
                    es.connEstFailInfoAvailable_r11 = (connEstFailInfoAvailable_r11_Enum)input.ReadBits(nBits);
                }
                if (stream.Read())
                {
                    es.nonCriticalExtension = nonCriticalExtension_Type.PerDecoder.Instance.Decode(input);
                }
                return es;
            }
        }
    }

    [Serializable]
    public class RRCConnectionReestablishmentComplete_v8a0_IEs
    {
        public void InitDefaults()
        {
        }

        public string lateNonCriticalExtension { get; set; }

        public RRCConnectionReestablishmentComplete_v1020_IEs nonCriticalExtension { get; set; }

        public class PerDecoder
        {
            public static readonly PerDecoder Instance = new PerDecoder();

            public RRCConnectionReestablishmentComplete_v8a0_IEs Decode(BitArrayInputStream input)
            {
                RRCConnectionReestablishmentComplete_v8a0_IEs es = new RRCConnectionReestablishmentComplete_v8a0_IEs();
                es.InitDefaults();
                BitMaskStream stream = new BitMaskStream(input, 2);
                if (stream.Read())
                {
                    int nBits = input.ReadBits(8);
                    es.lateNonCriticalExtension = input.readOctetString(nBits);
                }
                if (stream.Read())
                {
                    es.nonCriticalExtension = RRCConnectionReestablishmentComplete_v1020_IEs.PerDecoder.Instance.Decode(input);
                }
                return es;
            }
        }
    }

    [Serializable]
    public class RRCConnectionReestablishmentComplete_v920_IEs
    {
        public void InitDefaults()
        {
        }

        public RRCConnectionReestablishmentComplete_v8a0_IEs nonCriticalExtension { get; set; }

        public rlf_InfoAvailable_r9_Enum? rlf_InfoAvailable_r9 { get; set; }

        public class PerDecoder
        {
            public static readonly PerDecoder Instance = new PerDecoder();

            public RRCConnectionReestablishmentComplete_v920_IEs Decode(BitArrayInputStream input)
            {
                RRCConnectionReestablishmentComplete_v920_IEs es = new RRCConnectionReestablishmentComplete_v920_IEs();
                es.InitDefaults();
                BitMaskStream stream = new BitMaskStream(input, 2);
                if (stream.Read())
                {
                    const int nBits = 1;
                    es.rlf_InfoAvailable_r9 = (rlf_InfoAvailable_r9_Enum)input.ReadBits(nBits);
                }
                if (stream.Read())
                {
                    es.nonCriticalExtension = RRCConnectionReestablishmentComplete_v8a0_IEs.PerDecoder.Instance.Decode(input);
                }
                return es;
            }
        }

        public enum rlf_InfoAvailable_r9_Enum
        {
            _true
        }
    }

}
