﻿using log4net.Core;
using Lte.Domain.LinqToExcel;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace Lte.Domain.Test.LinqToExcel.ColumnFormat
{
    public class ChineseClassTest_ColumnAnnotation : SQLLogStatements_Helper
    {
        ExcelQueryFactory _repo;
        string _excelFileName;
        string _worksheetName;
        
        public ChineseClassTest_ColumnAnnotation() : base()
        {
            var testDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var excelFilesDirectory = Path.Combine(testDirectory, "ExcelFiles");
            _excelFileName = Path.Combine(excelFilesDirectory, "ChineseNameColumn.xlsx");
        }

        [SetUp]
        public void s()
        {
            _repo = new ExcelQueryFactory { FileName = _excelFileName };
        }

        [Test]
        public void test()
        {
            _worksheetName = "中文表格";
            _loggedEvents.Clear();

            var rows = (from c in _repo.Worksheet<ChineseClassWithColumnAnnotation>(_worksheetName)
                        select c).ToList();

            Assert.AreEqual(rows.Count, 2);
            Assert.AreEqual(rows[0].ChineseColumn, "第1行（2）");
            Assert.AreEqual(rows[1].ChineseColumn, "第二行-……");
            Assert.AreEqual(rows[0].DoubleColumn, 3.8);
            Assert.AreEqual(rows[1].DoubleColumn, 27);
            Assert.AreEqual(rows[0].ThirdColumn, 1);
            Assert.AreEqual(rows[1].ThirdColumn, 2);

            var events = _loggedEvents.GetEvents();
            Console.Write("0:{0}\n1:{1}\n2:{2}",
                events[0].RenderedMessage, events[1].RenderedMessage, events[2].RenderedMessage);
            var warningEvents = events.Where(x => x.Level == Level.Warn || x.Level == Level.Alert);
            Assert.AreEqual(warningEvents.Count(), 0);
        }
    }
}
