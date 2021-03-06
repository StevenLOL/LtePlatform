﻿using Lte.Domain.Common;
using Lte.Domain.LinqToExcel;
using Lte.Domain.Test.LinqToExcel;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace Lte.Parameters.Test.Excel
{
    [TestFixture]
    public class CdmaCellExcelTest : SQLLogStatements_Helper
    {
        ExcelQueryFactory _repo;
        string _excelFileName;
        string _worksheetName;
        
        public CdmaCellExcelTest()
        {
            var testDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var excelFilesDirectory = Path.Combine(testDirectory, "ExcelFiles");
            _excelFileName = Path.Combine(excelFilesDirectory, "CdmaCell.xls");
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
            Assert.AreEqual(info.Count, 5);
        }
    }
}
