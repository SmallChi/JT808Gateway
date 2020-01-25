using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JT808.Gateway.Client
{
    public class JT808ReportOptions
    {
        public string FileName { get; set; } = $"JT808Report.{DateTime.Now.ToString("yyyyMMddHHssmm")}.txt";
        public string FilePath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
        public string FileFullPath { get { return Path.Combine(FilePath, FileName); } }
        public int Interval { get; set; } = 3;
        public void FileExistsAndCreate()
        {
            if(!File.Exists(FileFullPath))
            {
                File.Create(FileFullPath).Close();
            }
        }
    }
}
