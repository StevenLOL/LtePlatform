﻿using Lte.Domain.LinqToExcel;
using Lte.Domain.Test.LinqToExcel;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using Lte.Domain.Common;

namespace Lte.Parameters.Test.Kpi
{
    [TestFixture]
    public class CdmaRegionStatExcelTest : SQLLogStatements_Helper
    {
        ExcelQueryFactory _repo;
        readonly string _excelFileName;
        
        public CdmaRegionStatExcelTest()
        {
            var testDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var excelFilesDirectory = Path.Combine(testDirectory, "ExcelFiles");
            _excelFileName = Path.Combine(excelFilesDirectory, "佛山.xls");
        }

        [SetUp]
        public void s()
        {
            _repo = new ExcelQueryFactory { FileName = _excelFileName };
        }

        [TestCase("佛山1区", 30)]
        [TestCase("佛山2区", 30)]
        [TestCase("佛山3区", 30)]
        [TestCase("佛山4区", 30)]
        public void Test_Read_Sheet(string sheetName, int count)
        {
            var info = (from c in _repo.Worksheet<CdmaRegionStatExcel>(sheetName)
                        select c).ToList();

            Assert.IsNotNull(info);
            Assert.AreEqual(info.Count, count);
        }

        [TestCase("2015-10-4", 24, "佛山1区")]
        [TestCase("2015-10-15", 13, "佛山1区")]
        [TestCase("2015-10-4", 24, "佛山2区")]
        [TestCase("2015-10-15", 13, "佛山2区")]
        [TestCase("2015-10-4", 24, "佛山3区")]
        [TestCase("2015-10-15", 13, "佛山3区")]
        [TestCase("2015-10-4", 24, "佛山4区")]
        [TestCase("2015-10-15", 13, "佛山4区")]
        public void Test_Read_ByDate(string beginDate, int lines, string sheetName)
        {
            var begin = DateTime.Parse(beginDate);
            var info = (from c in _repo.Worksheet<CdmaRegionStatExcel>(sheetName)
                        where c.StatDate > begin
                        select c).ToList();

            Assert.IsNotNull(info);
            Assert.AreEqual(info.Count, lines);
        }

        [TestCase("2015-10-4", "2015-11-4", 24, "佛山1区")]
        [TestCase("2015-10-4", "2015-10-10", 7, "佛山1区")]
        [TestCase("2015-10-15", "2015-11-4", 13, "佛山1区")]
        [TestCase("2015-10-15", "2015-10-20", 6, "佛山1区")]
        [TestCase("2015-10-4", "2015-11-4", 24, "佛山2区")]
        [TestCase("2015-10-4", "2015-10-10", 7, "佛山2区")]
        [TestCase("2015-10-15", "2015-11-4", 13, "佛山2区")]
        [TestCase("2015-10-15", "2015-10-20", 6, "佛山2区")]
        [TestCase("2015-10-4", "2015-11-4", 24, "佛山3区")]
        [TestCase("2015-10-4", "2015-10-10", 7, "佛山3区")]
        [TestCase("2015-10-15", "2015-11-4", 13, "佛山3区")]
        [TestCase("2015-10-15", "2015-10-20", 6, "佛山3区")]
        [TestCase("2015-10-4", "2015-11-4", 24, "佛山4区")]
        [TestCase("2015-10-4", "2015-10-10", 7, "佛山4区")]
        [TestCase("2015-10-15", "2015-11-4", 13, "佛山4区")]
        [TestCase("2015-10-15", "2015-10-20", 6, "佛山4区")]
        public void Test_Read_ByDateSpan(string beginDate, string endDate, int lines, string sheetName)
        {
            var begin = DateTime.Parse(beginDate);
            var end = DateTime.Parse(endDate);
            var info = (from c in _repo.Worksheet<CdmaRegionStatExcel>(sheetName)
                        where c.StatDate > begin && c.StatDate <= end.AddDays(1)
                        select c).ToList();

            Assert.IsNotNull(info);
            Assert.AreEqual(info.Count, lines);
        }
    }
}
