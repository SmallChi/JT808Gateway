using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JT808.DotNetty.Dashbord.Models
{
    [Table("jt808_transmit_log")]
    public class JT808_TransmitLog
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
        [Column("is_success")]
        public bool IsSuccess { get; set; }
        [MaxLength(128)]
        [Column("reason")]
        public string Reason { get; set; }
    }
}
