using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JT808.DotNetty.Dashbord.Models
{
    [Table("jt808_source_package")]
    public class JT808_SourcePackage
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("create_time")]
        public long CreateTime { get; set; }
        [MaxLength(32)]
        [Column("ip")]
        public string IP { get; set; }
        [Column("port")]
        public int Port { get; set; }
        /// <summary>
        /// 是否系统配置
        /// true:网关配置文件
        /// false:平台配置
        /// </summary>
        [Column("is_system_config")]
        public bool IsSystemConfig { get; set; }
    }
}
