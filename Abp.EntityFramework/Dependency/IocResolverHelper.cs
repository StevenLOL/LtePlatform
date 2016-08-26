﻿using System.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;

namespace Abp.EntityFramework.Dependency
{
    public static class IocResolverHelper
    {
        public static string GetNameOrConnectionStringOrNull(this IIocResolver iocResolver)
        {
            if (!iocResolver.IsRegistered<IAbpStartupConfiguration>())
                return ConfigurationManager.ConnectionStrings["Default"] != null ? "Default" : null;
            var defaultConnectionString = iocResolver.Resolve<IAbpStartupConfiguration>().DefaultNameOrConnectionString;
            if (!string.IsNullOrWhiteSpace(defaultConnectionString))
            {
                return defaultConnectionString;
            }

            return ConfigurationManager.ConnectionStrings["Default"] != null ? "Default" : null;
        }
    }
}
