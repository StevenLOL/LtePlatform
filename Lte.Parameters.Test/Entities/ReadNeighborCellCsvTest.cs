﻿using Lte.Domain.LinqToCsv.Context;
using Lte.Domain.LinqToCsv.Description;
using Lte.Parameters.Entities.Neighbor;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Abp.EntityFramework.AutoMapper;
using AutoMapper;
using Lte.Parameters.Entities.Dt;

namespace Lte.Parameters.Test.Entities
{
    [TestFixture]
    public class ReadNeighborCellCsvTest
    {
        [Test]
        public void Test_HuaweiFile()
        {
            var testDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var csvFilesDirectory = Path.Combine(testDirectory, "CsvFiles");
            var path = Path.Combine(csvFilesDirectory, "NeighborCellHw.csv");

            var reader = new StreamReader(path);
            var infos = CsvContext.Read<NeighborCellHwCsv>(reader, CsvFileDescription.CommaDescription).ToList();
            Assert.AreEqual(infos.Count, 999);
        }

        [Test]
        public void Test_HuaweiFile_Merge()
        {
            var testDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var csvFilesDirectory = Path.Combine(testDirectory, "CsvFiles");
            var path = Path.Combine(csvFilesDirectory, "NeighborCellHw.csv");

            var reader = new StreamReader(path);
            var infos = NeighborCellHwCsv.ReadNeighborCellHwCsvs(reader);
            Assert.AreEqual(infos.Count, 7);
        }

        [Test]
        public void Test_ZteFile1()
        {
            var testDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var csvFilesDirectory = Path.Combine(testDirectory, "CsvFiles");
            var path = Path.Combine(csvFilesDirectory, "NeighborCellZte1.csv");

            var reader = new StreamReader(path, Encoding.GetEncoding("GB2312"));
            var infos = CsvContext.Read<NeighborCellZteCsv>(reader, CsvFileDescription.CommaDescription).ToList();
            Assert.AreEqual(infos.Count, 1);
        }

        [Test]
        public void Test_ZteFile()
        {
            var testDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var csvFilesDirectory = Path.Combine(testDirectory, "CsvFiles");
            var path = Path.Combine(csvFilesDirectory, "NeighborCellZte.csv");

            var reader = new StreamReader(path, Encoding.GetEncoding("GB2312"));
            var infos = CsvContext.Read<NeighborCellZteCsv>(reader, CsvFileDescription.CommaDescription).ToList();
            Assert.AreEqual(infos.Count, 998);
        }

        [Test]
        public void Test_ZteFile_Merge()
        {
            var testDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var csvFilesDirectory = Path.Combine(testDirectory, "CsvFiles");
            var path = Path.Combine(csvFilesDirectory, "NeighborCellZte.csv");

            var reader = new StreamReader(path, Encoding.GetEncoding("GB2312"));
            var infos = NeighborCellZteCsv.ReadNeighborCellZteCsvs(reader);
            Assert.AreEqual(infos.Count, 998);
        }

    }

    [TestFixture]
    public class ReadFileRecordCsvTest
    {
        [TestFixtureSetUp]
        public void Ts()
        {
            Mapper.Initialize(cfg=>cfg.CreateMap<FileRecord3GCsv, FileRecord3G>());
        }

        [Test]
        public void Test_Read_2G()
        {
            var testDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var csvFilesDirectory = Path.Combine(testDirectory, "CsvFiles");
            var path = Path.Combine(csvFilesDirectory, "CDMA_20140114_禅城区_城区_短呼_姚华海_高基街_2.csv");

            var reader = new StreamReader(path);
            var infos = CsvContext.Read<FileRecord2GCsv>(reader, CsvFileDescription.CommaDescription).ToList();
            Assert.AreEqual(infos.Count, 610);
            Assert.AreEqual(((double?)null).ToString(), "");
            var legalInfos =
                infos.Where(x => x.Longtitute != null && x.Lattitute != null && x.RxAgc != null && x.EcIo != null);
            Assert.AreEqual(legalInfos.Count(), 212);
            Assert.AreEqual(legalInfos.ElementAt(0).ComputerTime.Date, new DateTime(2014,1,14));
        }

        [Test]
        public void Test_Read_3G()
        {
            var testDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var csvFilesDirectory = Path.Combine(testDirectory, "CsvFiles");
            var path = Path.Combine(csvFilesDirectory, "CDMA_20140114_禅城区_城区_短呼_姚华海_高基街_4.csv");

            var reader = new StreamReader(path);
            var infos = CsvContext.Read<FileRecord3GCsv>(reader, CsvFileDescription.CommaDescription).ToList();
            Assert.AreEqual(infos.Count, 2564);
            var legalInfos =
                infos.Where(
                    x =>
                        x.Longtitute != null && x.Lattitute != null && x.Pn != null);
            Assert.AreEqual(legalInfos.Count(), 1017);
            var PnStats = legalInfos.MapTo<List<FileRecord3G>>();
            Assert.AreEqual(PnStats[0].Pn, 282);
        }
    }
}
