﻿using Lte.Domain.Common;
using Lte.Domain.LinqToExcel;
using Lte.Domain.Test.LinqToExcel;
using NUnit.Framework;
using Shouldly;
using System;
using System.IO;
using System.Linq;

namespace Lte.Parameters.Test.Excel
{
    [TestFixture]
    public class PracticalCdmaCellExcelTest : SQLLogStatements_Helper
    {
        ExcelQueryFactory _repo;
        string _excelFileName;
        string _worksheetName;
        
        public PracticalCdmaCellExcelTest()
        {
            var testDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var excelFilesDirectory = Path.Combine(testDirectory, "ExcelFiles");
            _excelFileName = Path.Combine(excelFilesDirectory, "WrongAntennaGainCdmaCell.xls");
            _worksheetName = "CdmaCell";
        }

        [SetUp]
        public void s()
        {
            _repo = new ExcelQueryFactory { FileName = _excelFileName };
        }

        [Test]
        public void Test_Read_Sheet()
        {
            var info = (from c in _repo.Worksheet<CdmaCellExcel>(_worksheetName)
                        select c).ToList();

            Assert.IsNotNull(info);
            Assert.AreEqual(info.Count, 21);
            info[12].AntennaGain.ShouldBe(0.0);
        }
    }
}
