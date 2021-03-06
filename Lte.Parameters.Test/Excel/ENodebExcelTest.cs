﻿using Lte.Domain.LinqToExcel;
using Lte.Domain.Test.LinqToExcel;
using NUnit.Framework;
using Shouldly;
using System;
using System.IO;
using System.Linq;
using Lte.Domain.Common;

namespace Lte.Parameters.Test.Excel
{
    [TestFixture]
    public class ENodebExcelTest : SQLLogStatements_Helper
    {
        ExcelQueryFactory _repo;
        string _excelFileName;
        string _worksheetName;
        
        public ENodebExcelTest()
        {
            var testDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var excelFilesDirectory = Path.Combine(testDirectory, "ExcelFiles");
            _excelFileName = Path.Combine(excelFilesDirectory, "ENodeb.xlsx");
            _worksheetName = "ENodeb";
        }

        [SetUp]
        public void s()
        {
            _repo = new ExcelQueryFactory { FileName = _excelFileName };
        }

        [Test]
        public void Test_Read_Sheet()
        {
            var info = (from c in _repo.Worksheet<ENodebExcel>(_worksheetName)
                        select c).ToList();

            Assert.IsNotNull(info);
            Assert.AreEqual(info.Count, 5);
            info[0].Name.ShouldBe("大良东苑");
            info[0].Ip.AddressString.ShouldBe("8.142.15.4");
            info[2].Address.ShouldBe("佛山市顺德区大良镇锦岩路六巷5号");
            info[3].OpenDate.ShouldBe(DateTime.Parse("2013/8/30"));
            info[4].GatewayIp.AddressString.ShouldBe("8.142.15.1");
        }

    }
}
