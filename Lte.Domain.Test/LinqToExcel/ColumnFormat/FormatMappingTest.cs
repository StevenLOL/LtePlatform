﻿using log4net.Core;
using Lte.Domain.LinqToExcel;
using Lte.Domain.Regular;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace Lte.Domain.Test.LinqToExcel.ColumnFormat
{
    [TestFixture]
    public class FormatMappingTest : SQLLogStatements_Helper
    {
        ExcelQueryFactory _repo;
        string _excelFileName;
        string _worksheetName;
        
        public FormatMappingTest()
        {
            var testDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var excelFilesDirectory = Path.Combine(testDirectory, "ExcelFiles");
            _excelFileName = Path.Combine(excelFilesDirectory, "IntegerFormat.xls");
        }

        [SetUp]
        public void s()
        {
            _repo = new ExcelQueryFactory { FileName = _excelFileName };
            _repo.AddMapping<IntegerColumnClass>(x => x.StringColumn, "String Column");
        }

        [Test]
        public void Test_SaftyChangeTransform()
        {
            _repo.AddMapping<IntegerColumnClass>(x => x.IntegerColumn, "Integer Column",
                x => x.ToString().ConvertToInt(0));

            _worksheetName = "DoublePoints";
            _loggedEvents.Clear();

            var rows = (from c in _repo.Worksheet<IntegerColumnClass>(_worksheetName)
                        select c).ToList();

            Assert.AreEqual(rows.Count, 2);
            Assert.AreEqual(rows[0].IntegerColumn, 0);
            Assert.AreEqual(rows[1].IntegerColumn, 0);

            var events = _loggedEvents.GetEvents();
            Console.Write("0:{0}\n1:{1}\n2:{2}",
                events[0].RenderedMessage, events[1].RenderedMessage, events[2].RenderedMessage);
            var warningEvents = events.Where(x => x.Level == Level.Warn || x.Level == Level.Alert);
            Assert.AreEqual(warningEvents.Count(), 0);
        }

        [Test]
        public void Test_RemovePointsSaveTransform()
        {
            _repo.AddMapping<IntegerColumnClass>(x => x.IntegerColumn, "Integer Column",
                x => x.ToString().Replace(".", "").ConvertToInt(0));

            _worksheetName = "DoublePoints";
            _loggedEvents.Clear();

            var rows = (from c in _repo.Worksheet<IntegerColumnClass>(_worksheetName)
                        select c).ToList();

            Assert.AreEqual(rows.Count, 2);
            Assert.AreEqual(rows[0].IntegerColumn, 25);
            Assert.AreEqual(rows[1].IntegerColumn, 7);

            var events = _loggedEvents.GetEvents();
            Console.Write("0:{0}\n1:{1}\n2:{2}",
                events[0].RenderedMessage, events[1].RenderedMessage, events[2].RenderedMessage);
            var warningEvents = events.Where(x => x.Level == Level.Warn || x.Level == Level.Alert);
            Assert.AreEqual(warningEvents.Count(), 0);
        }
    }
}
