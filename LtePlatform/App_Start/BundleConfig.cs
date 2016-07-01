﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace LtePlatform
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*"));
            
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                "~/Scripts/knockout-{version}.js",
                "~/Scripts/knockout.validation.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/dtList").Include(
                "~/Scripts/sammy-{version}.js",
                "~/Scripts/app/common.js",
                "~/Scripts/app/app.datamodel.js",
                "~/Scripts/app/app.viewmodel.js",
                "~/Scripts/dt/list.viewmodel.js",
                "~/Scripts/app/_run.js"));
           
            bundles.Add(new ScriptBundle("~/bundles/parametersAlarmImport").Include(
                "~/Scripts/sammy-{version}.js",
                "~/Scripts/app/common.js",
                "~/Scripts/app/app.datamodel.js",
                "~/Scripts/app/app.viewmodel.js",
                "~/Scripts/parameters/common.controller.js",
                "~/Scripts/kpi/common.controller.js",
                "~/Scripts/parameters/alarmimport.controller.js",
                "~/Scripts/parameters/alarmimport.viewmodel.js",
                "~/Scripts/app/_run.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/webapiBasicPost").Include(
                "~/Scripts/sammy-{version}.js",
                "~/Scripts/app/common.js",
                "~/Scripts/app/app.datamodel.js",
                "~/Scripts/app/app.viewmodel.js",
                "~/Areas/TestPage/Scripts/basicpost.viewmodel.js",
                "~/Scripts/app/_run.js"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/bootstrap.css",
                 "~/Content/bootstrap-theme.css",
                "~/Content/themes/ui-bootstrap.css",
                 "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                "~/Content/themes/base/all.css"));
            
            bundles.Add(new StyleBundle("~/Content/HelpPage").Include(
                "~/Areas/HelpPage/HelpPage.css"));
        }
    }
}
