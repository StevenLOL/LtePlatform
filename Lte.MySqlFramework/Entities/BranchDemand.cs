﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.EntityFramework.AutoMapper;
using Lte.Domain.Common;
using Lte.Domain.Common.Geo;
using Lte.Domain.Common.Wireless;
using Lte.Domain.Regular.Attributes;

namespace Lte.MySqlFramework.Entities
{
    [AutoMapFrom(typeof(BranchDemandExcel), typeof(BranchDemandDto))]
    public class BranchDemand : Entity, ITownId
    {
        public DateTime BeginDate { get; set; }

        public string SerialNumber { get; set; }

        public int TownId { get; set; }

        public string ComplainContents { get; set; }

        [AutoMapPropertyResolve("FirstContents", typeof(BranchDemandExcel), typeof(DateTimeNowLabelTransform))]
        public string ProcessContents { get; set; }

        [AutoMapPropertyResolve("SolveFunctionDescription", typeof(BranchDemandExcel), typeof(SolveFunctionTransform))]
        [AutoMapPropertyResolve("SolveFunctionDescription", typeof(BranchDemandDto), typeof(SolveFunctionTransform))]
        public SolveFunction SolveFunction { get; set; }

        [AutoMapPropertyResolve("IsSolvedDescription", typeof(BranchDemandExcel), typeof(YesToBoolTransform))]
        [AutoMapPropertyResolve("IsSolvedDescription", typeof(BranchDemandDto), typeof(YesToBoolTransform))]
        public bool IsSolved { get; set; }

        public DateTime? EndDate { get; set; }

        public double Lontitute { get; set; }
        
        public double Lattitute { get; set; }

        public string SubscriberInfo { get; set; }
        
        public string ManagerInfo { get; set; }
    }

    [AutoMapFrom(typeof(BranchDemand))]
    public class BranchDemandDto
    {
        public DateTime BeginDate { get; set; }

        public string SerialNumber { get; set; }

        public int TownId { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public string Town { get; set; }

        public string ComplainContents { get; set; }

        public string ProcessContents { get; set; }

        [AutoMapPropertyResolve("SolveFunction", typeof(BranchDemand), typeof(SolveFunctionDescriptionTransform))]
        public string SolveFunctionDescription { get; set; }

        [AutoMapPropertyResolve("IsSolved", typeof(BranchDemand), typeof(YesNoTransform))]
        public string IsSolvedDescription { get; set; }

        public DateTime? EndDate { get; set; }

        public double Lontitute { get; set; }

        public double Lattitute { get; set; }

        public string SubscriberInfo { get; set; }

        public string ManagerInfo { get; set; }
    }

    public class BranchDemandExcel : IDistrictTown
    {
        [ExcelColumn("用户申告时间")]
        public DateTime BeginDate { get; set; }

        [ExcelColumn("序号")]
        public string SerialNumber { get; set; }

        [ExcelColumn("所属区域")]
        public string District { get; set; }

        [ExcelColumn("所属镇")]
        public string Town { get; set; }
        
        [ExcelColumn("用户申告内容描述")]
        public string ComplainContents { get; set; }

        [ExcelColumn("处理情况")]
        public string FirstContents { get; set; }

        [ExcelColumn("解决方式（解决措施分类）")]
        public string SolveFunctionDescription { get; set; }

        [ExcelColumn("问题是否解决")]
        public string IsSolvedDescription { get; set; }

        [ExcelColumn("解决时间")]
        public DateTime? EndDate { get; set; }

        [ExcelColumn("经度")]
        public double Lontitute { get; set; }

        [ExcelColumn("纬度")]
        public double Lattitute { get; set; }

        [ExcelColumn("用户姓名及电话号码")]
        public string SubscriberInfo { get; set; }

        [ExcelColumn("客户经理姓名")]
        public string ManagerInfo { get; set; }
    }
}
