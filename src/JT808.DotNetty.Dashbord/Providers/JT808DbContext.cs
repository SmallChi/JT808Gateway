using JT808.DotNetty.Dashbord.Enums;
using JT808.DotNetty.Dashbord.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JT808.DotNetty.Dashbord.Providers
{
    /// <summary>
    /// 
    /// Add-Migration Init
    /// 首次更新：创建对应文件夹
    /// Update-Database
    /// System.NotSupportedException: SQLite does not support this migration operation ('DropColumnOperation').
    /// SQLite： 不支持删除列
    /// 解决方式：
    /// https://elanderson.net/2017/04/entity-framework-core-with-sqlite-migration-limitations/
    /// https://sqlite.org/lang_altertable.html#otheralter
    /// </summary>
    public class JT808DbContext : DbContext
    {
        public DbSet<JT808_AtomicCounter> JT808_AtomicCounters { get; set; }
        public DbSet<JT808_UnificationSend> JT808_UnificationSends { get; set; }
        public DbSet<JT808_TransmitLog> JT808_TransmitLogs { get; set; }
        public DbSet<JT808_Transmit> JT808_Transmits { get; set; }
        public DbSet<JT808_SourcePackageLog> JT808_SourcePackageLogs { get; set; }
        public DbSet<JT808_SourcePackage> JT808_SourcePackages { get; set; }
        public DbSet<JT808_SessionLog> JT808_SessionLogs { get; set; }
        public DbSet<JT808_Session> JT808_Sessions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=../JT808.DotNetty.Dashbord/data/jt808.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<JT808_UnificationSend>()
                .Property(e => e.UnificationSendType)
                .HasConversion(
                    v => v.ToString(),
                    v => (JT808UnificationSendType)Enum.Parse(typeof(JT808UnificationSendType), v));
        }
    }
}
