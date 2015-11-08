﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lte.Domain.Common
{
    public enum AlarmType : short
    {
        CeNotEnough,
        StarUnlocked,
        TrunkProblem,
        RssiProblem,
        CellDown,
        VswrProblem,
        VswrLte,
        Unimportant,
        LinkBroken,
        X2Broken,
        X2UserPlane,
        S1Broken,
        S1UserPlane,
        EthernetBroken,
        LteCellDown,
        LteCellError,
        SuperCellDown,
        ENodebDown,
        GnssStar,
        GnssFeed,
        PaDeactivate,
        RruBroken,
        RxChannel,
        SntpFail,
        VersionError,
        InitializationError,
        BoardInexist,
        BoardInitialize,
        BoardPowerDown,
        BoardCommunication,
        BoardSoftId,
        FiberReceiver,
        FiberModule,
        BbuInitialize,
        Temperature,
        FanTemperature,
        NoClock,
        InnerError,
        SoftwareAbnormal,
        ApparatusPowerDown,
        InputVolte,
        OuterApparatus,
        ParametersConfiguation,
        BadPerformance,
        Others,
        DatabaseDelay,
        PciCrack,
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
