using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JT808.DotNetty.Dashbord.Models
{
    [Table("jt808_session_log")]
    public class JT808_SessionLog
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("create_time")]
        public long CreateTime { get; set; }
        [MaxLength(32)]
        [Column("channel_id")]
        public string ChannelId { get; set; }
        [MaxLength(16)]
        [Column("terminal_phone_no")]
        public string TerminalPhoneNo { get; set; }
        [MaxLength(128)]
        [Column("reason")]
        public string Reason { get; set; }
        [Column("is_success")]
        public bool IsSuccess { get; set; }
    }
}
