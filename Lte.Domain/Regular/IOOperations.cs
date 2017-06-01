﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Lte.Domain.Regular
{
    public static class IOOperations
    {
        public static StreamReader GetStreamReader(this string source)
        {
            byte[] stringAsByteArray = Encoding.UTF8.GetBytes(source);
            Stream stream = new MemoryStream(stringAsByteArray);

            var streamReader = new StreamReader(stream, Encoding.UTF8);
            return streamReader;
        }
        
        public static string GenerateFilePath(this string directoryPath, string filename)
        {
            var filePath = directoryPath + "/" + Path.GetFileName(filename);

            //判断文件是否存在，存在则在文件名称后加上_和数字
            var directory = new DirectoryInfo(directoryPath);
            var name = Path.GetFileNameWithoutExtension(filename);
            var files = directory.GetFiles(name + "*" + Path.GetExtension(filename), SearchOption.TopDirectoryOnly);
            var regexName = name + "(_\\d+)*" + Path.GetExtension(filename);

            files = files.ToList().Where(t => Regex.IsMatch(t.Name, regexName)).OrderByDescending(t => t.CreationTime).ToArray();

            if (!files.Any()) return filePath;
            var file = files.First();
            var math = Regex.Match(Path.GetFileNameWithoutExtension(file.FullName), @"^\S+(?<num>_\d+)$");
            if (math.Success)
            {
                var num = math.Groups["num"].Value.TrimStart('_').ConvertToInt(100);
                name = name + "_" + (num + 1);
            }
            else
            {
                name += "_1";
            }

            filePath = directoryPath + "/" + name + Path.GetExtension(filename);

            return filePath;
        }

    }
}
