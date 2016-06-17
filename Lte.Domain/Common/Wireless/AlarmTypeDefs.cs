﻿using System;

namespace Lte.Domain.Common.Wireless
{
    public enum AlarmType : short
    {
        CeNotEnough,//0
        StarUnlocked,
        TrunkProblem,
        RssiProblem,
        CellDown,//4
        VswrProblem,
        VswrLte,
        Unimportant,
        LinkBroken,
        X2Broken,
        X2UserPlane,//10
        S1Broken,
        S1UserPlane,
        EthernetBroken,
        LteCellDown,
        LteCellError,//15
        SuperCellDown,
        ENodebDown,
        GnssStar,
        GnssFeed,
        PaDeactivate,//20
        RruBroken,
        RxChannel,
        SntpFail,
        VersionError,
        InitializationError,//25
        BoardInexist,
        BoardInitialize,
        BoardPowerDown,
        BoardCommunication,
        BoardSoftId,//30
        FiberReceiver,
        FiberModule,
        BbuInitialize,
        Temperature,
        FanTemperature,//35
        NoClock,
        InnerError,
        SoftwareAbnormal,
        ApparatusPowerDown,
        InputVolte,//40
        OuterApparatus,
        ParametersConfiguation,
        BadPerformance,//43
        Others,
        DatabaseDelay,
        PciCrack,//46
        RruRtwp,
        BbuCpriInterface,
        BbuCpriLost,
        EletricAntenna,
        RfAld,
        RruCpriInterface,
        RruInterfacePerformance,
        RruPowerDown,
        RruRtwpUnbalance,
        RruClock,
        RruOmcLink,
        ClockReference,
        Database,
        AntennaLink,
        UserPlane,
        RemoteOmc,
        LoginError,
        AnalogLoad
    }

    [EnumTypeDescription(typeof(Tuple<AlarmLevel, string>[]), "AlarmLevelDescription")]
    public enum AlarmLevel : byte
    {
        Serious,
        Primary,
        Secondary,
        Warning,
        Urgent,
        Important,
        Tips
    }

    public enum AlarmCategory : byte
    {
        Communication,
        Qos,
        ProcessError,
        Environment,
        Apparatus,
        Huawei
    }
}
