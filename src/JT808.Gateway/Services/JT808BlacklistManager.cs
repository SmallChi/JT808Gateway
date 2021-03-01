using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using System.IO;
using Microsoft.Extensions.Options;
using System.Linq;

namespace JT808.Gateway.Services
{
    /// <summary>
    /// SIM黑名单管理
    /// </summary>
    public class JT808BlacklistManager
    {
        private ConcurrentDictionary<string, byte> Blacklist;

        private const string BlacklistFileName = "blacklist.ini";

        private FileSystemWatcher fileSystemWatcher;

        private string FullPath;

        /// <summary>
        /// 
        /// </summary>
        public JT808BlacklistManager()
        {
            Blacklist = new ConcurrentDictionary<string, byte>();
            FullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, BlacklistFileName);
            if (!File.Exists(FullPath))
            {
                File.Create(FullPath).Close();
            }
            else
            {
                Init(FullPath);
            }
            fileSystemWatcher = new FileSystemWatcher(AppDomain.CurrentDomain.BaseDirectory, BlacklistFileName);
            fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
            fileSystemWatcher.EnableRaisingEvents = true;
            fileSystemWatcher.Changed += (sender, e) => 
            {
                Init(e.FullPath);
            };
        }

        private void Init(string fullPath)
        {
            var values = File.ReadAllLines(fullPath);
            foreach (var item in values)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    Blacklist.TryAdd(item, 0);
                }
            }
        }

        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="sim"></param>
        /// <returns></returns>
        public bool Contains(string sim)
        {
            return Blacklist.ContainsKey(sim);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sim"></param>
        public void Add(string sim)
        {
            if(Blacklist.TryAdd(sim, 0))
            {
                File.AppendAllLines(FullPath, new List<string> { sim });
            }
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="sim"></param>
        public void Remove(string sim)
        {
            Blacklist.TryRemove(sim, out _);
            File.WriteAllLines(FullPath, Blacklist.Select(s => s.Key).OrderBy(o=>o).ToList());
        }

        /// <summary>
        /// 查询所有黑名单
        /// </summary>
        /// <returns></returns>
        public List<string> GetAll()
        {
            return Blacklist.Select(s => s.Key).ToList();
        }
    }
}
