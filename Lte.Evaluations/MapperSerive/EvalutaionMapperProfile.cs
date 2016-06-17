﻿using Abp.EntityFramework.AutoMapper;
using Abp.Reflection;
using AutoMapper;
using Lte.Evaluations.Policy;
using Lte.MySqlFramework.Abstract;
using Lte.Parameters.MockOperations;

namespace Lte.Evaluations.MapperSerive
{
    public class EvalutaionMapperProfile : Profile
    {
        private readonly ITypeFinder _typeFinder = new TypeFinder
        {
            AssemblyFinder = new MyAssemblyFinder()
        };

        protected override void Configure()
        {
            KpiMapperService.MapDtItems();

            ParametersDumpMapperService.MapFromENodebContainerService();
            ParametersDumpMapperService.MapFromBtsContainerService();
            ParametersDumpMapperService.MapENodebBtsIdService();
            
            InfrastructureMapperService.MapCdmaCell();
            InfrastructureMapperService.MapCell();
            InfrastructureMapperService.MapHoParametersService();

            KpiMapperService.MapCdmaRegionStat();
            KpiMapperService.MapFlow();
            KpiMapperService.MapAlarmStat();
            KpiMapperService.MapTopKpi();
            KpiMapperService.MapPreciseStat();
            KpiMapperService.MapWorkItem();

            BaiduMapperService.MapCdmaCellView();
            BaiduMapperService.MapCellView();
            BaiduMapperService.MapDtViews();
            
            var module = new AbpAutoMapperModule(_typeFinder);
            module.PostInitialize();
        }
    }
}
