﻿using System;
using System.Linq;

namespace Lte.Domain.Common.Wireless
{
    public static class AlarmTypeQueries
    {
        private static readonly Tuple<AlarmType, string>[] AlarmTypeDescriptionList =
        {
            new Tuple<AlarmType, string>(AlarmType.CeNotEnough, "CE不足"),
            new Tuple<AlarmType, string>(AlarmType.StarUnlocked, "锁星问题"),
            new Tuple<AlarmType, string>(AlarmType.TrunkProblem, "传输问题"),
            new Tuple<AlarmType, string>(AlarmType.RssiProblem, "RSSI问题"),
            new Tuple<AlarmType, string>(AlarmType.CellDown, "小区退服"),
            new Tuple<AlarmType, string>(AlarmType.VswrProblem, "驻波比问题"),
            new Tuple<AlarmType, string>(AlarmType.VswrLte, "天馈驻波比异常(198098465)"),
            new Tuple<AlarmType, string>(AlarmType.Unimportant, "不影响业务问题"),
            new Tuple<AlarmType, string>(AlarmType.LinkBroken, "网元断链告警(198099803)"),
            new Tuple<AlarmType, string>(AlarmType.X2Broken, "X2断链告警(198094421)"),
            new Tuple<AlarmType, string>(AlarmType.X2UserPlane, "X2用户面路径不可用(198094467)"),
            new Tuple<AlarmType, string>(AlarmType.S1Broken, "S1断链告警(198094420)"),
            new Tuple<AlarmType, string>(AlarmType.S1UserPlane, "S1用户面路径不可用(198094466)"),
            new Tuple<AlarmType, string>(AlarmType.EthernetBroken, "以太网物理连接断(198098252)"),
            new Tuple<AlarmType, string>(AlarmType.LteCellDown, "LTE小区退出服务(198094419)"),
            new Tuple<AlarmType, string>(AlarmType.LteCellError, "小区关断告警(198094461)"),
            new Tuple<AlarmType, string>(AlarmType.SuperCellDown, "超级小区CP退出服务(198094440)"),
            new Tuple<AlarmType, string>(AlarmType.ENodebDown, "基站退出服务(198094422)"),
            new Tuple<AlarmType, string>(AlarmType.GnssStar, "GNSS接收机搜星故障(198096837)"),
            new Tuple<AlarmType, string>(AlarmType.GnssFeed, "GNSS天馈链路故障(198096836)"),
            new Tuple<AlarmType, string>(AlarmType.PaDeactivate, "PA去使能(198098440)"),
            new Tuple<AlarmType, string>(AlarmType.RruBroken, "RRU链路断(198097605)"),
            new Tuple<AlarmType, string>(AlarmType.RxChannel, "RX通道异常(198098469)"),
            new Tuple<AlarmType, string>(AlarmType.SntpFail, "SNTP对时失败(198092014)"),
            new Tuple<AlarmType, string>(AlarmType.VersionError, "版本包故障(198097567)"),
            new Tuple<AlarmType, string>(AlarmType.InitializationError, "初始化失败(198092070)"),
            new Tuple<AlarmType, string>(AlarmType.BoardInexist, "单板不在位(198092072)"),
            new Tuple<AlarmType, string>(AlarmType.BoardInitialize, "单板处于初始化状态(198092348)"),
            new Tuple<AlarmType, string>(AlarmType.BoardPowerDown, "单板电源关断(198092057)"),
            new Tuple<AlarmType, string>(AlarmType.BoardCommunication, "单板通讯链路断(198097060)"),
            new Tuple<AlarmType, string>(AlarmType.BoardSoftId, "找不到单板软件标识(198092397)"),
            new Tuple<AlarmType, string>(AlarmType.FiberReceiver, "光口接收链路故障(198098319)"),
            new Tuple<AlarmType, string>(AlarmType.FiberModule, "光模块不可用(198098318)"),
            new Tuple<AlarmType, string>(AlarmType.BbuInitialize, "基带单元处于初始化状态(198097050)"),
            new Tuple<AlarmType, string>(AlarmType.Temperature, "温度异常(198097061)"),
            new Tuple<AlarmType, string>(AlarmType.FanTemperature, "进风口温度异常(198092042)"),
            new Tuple<AlarmType, string>(AlarmType.NoClock, "没有可用的空口时钟源(198092217)"),
            new Tuple<AlarmType, string>(AlarmType.InnerError, "内部故障(198098467)"),
            new Tuple<AlarmType, string>(AlarmType.SoftwareAbnormal, "软件运行异常(198097604)"),
            new Tuple<AlarmType, string>(AlarmType.ApparatusPowerDown, "设备掉电(198092295)"),
            new Tuple<AlarmType, string>(AlarmType.InputVolte, "输入电压异常(198092053)"),
            new Tuple<AlarmType, string>(AlarmType.OuterApparatus, "外部扩展设备故障(198098468)"),
            new Tuple<AlarmType, string>(AlarmType.ParametersConfiguation, "网元不支持配置的参数(198097510)"),
            new Tuple<AlarmType, string>(AlarmType.BadPerformance, "性能门限越界(1513)"),
            new Tuple<AlarmType, string>(AlarmType.Others, "其他告警"),
            new Tuple<AlarmType, string>(AlarmType.DatabaseDelay, "性能数据入库延迟(15010001)")
        };

        public static string GetAlarmTypeDescription(this AlarmType type)
        {
            var tuple = AlarmTypeDescriptionList.FirstOrDefault(x => x.Item1 == type);
            return (tuple != null) ? tuple.Item2 : type.GetAlarmTypeHuawei();
        }

        public static AlarmType GetAlarmType(this string description)
        {
            var tuple = AlarmTypeDescriptionList.FirstOrDefault(x => x.Item2 == description);
            return tuple?.Item1 ?? AlarmType.Others;
        }
        
        public static string GetAlarmTypeHuawei(this AlarmType type)
        {
            return type.GetEnumDescription();
        }
        
        public static readonly Tuple<AlarmLevel, string>[] AlarmLevelDescriptionList =
        {
            new Tuple<AlarmLevel, string>(AlarmLevel.Serious, "严重"),
            new Tuple<AlarmLevel, string>(AlarmLevel.Primary, "主要"),
            new Tuple<AlarmLevel, string>(AlarmLevel.Secondary, "次要"),
            new Tuple<AlarmLevel, string>(AlarmLevel.Warning, "警告"),
            new Tuple<AlarmLevel, string>(AlarmLevel.Urgent, "紧急"),
            new Tuple<AlarmLevel, string>(AlarmLevel.Important, "重要"),
            new Tuple<AlarmLevel, string>(AlarmLevel.Tips, "提示"),
        };

        public static string GetAlarmLevelDescription(this AlarmLevel level)
        {
            var tuple = AlarmLevelDescriptionList.FirstOrDefault(x => x.Item1 == level);
            return (tuple != null) ? tuple.Item2 : "次要";
        }

        public static AlarmLevel GetAlarmLevel(this string description)
        {
            var tuple = AlarmLevelDescriptionList.FirstOrDefault(x => x.Item2 == description);
            return (tuple != null) ? tuple.Item1 : AlarmLevel.Secondary;
        }

        private static readonly Tuple<AlarmCategory, string>[] AlarmCategoryDescriptionList =
        {
            new Tuple<AlarmCategory, string>(AlarmCategory.Communication, "通信告警"),
            new Tuple<AlarmCategory, string>(AlarmCategory.Qos, "服务质量告警"),
            new Tuple<AlarmCategory, string>(AlarmCategory.ProcessError, "处理错误告警"),
            new Tuple<AlarmCategory, string>(AlarmCategory.Environment, "环境告警"),
            new Tuple<AlarmCategory, string>(AlarmCategory.Apparatus, "设备告警"),
            new Tuple<AlarmCategory, string>(AlarmCategory.Huawei, "华为告警"),
        };

        public static string GetAlarmCategoryDescription(this AlarmCategory category)
        {
            var tuple = AlarmCategoryDescriptionList.FirstOrDefault(x => x.Item1 == category);
            return (tuple != null) ? tuple.Item2 : "服务质量告警";
        }

        public static AlarmCategory GetCategory(this string description)
        {
            var tuple = AlarmCategoryDescriptionList.FirstOrDefault(x => x.Item2 == description);
            return (tuple != null) ? tuple.Item1 : AlarmCategory.Qos;
        }
    }
}
