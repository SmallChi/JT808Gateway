using JT808.DotNetty.Dashbord.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JT808.DotNetty.Dashbord.Models
{
    [Table("jt808_unification_send")]
    public class JT808_UnificationSend
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("create_time")]
        public long CreateTime { get; set; }
        [Column("unification_send_type")]
        public JT808UnificationSendType UnificationSendType { get; set; }
        [MaxLength(16)]
        [Column("terminal_phone_no")]
        public string TerminalPhoneNo { get; set; }
        [MaxLength(2048)]
        [Column("hex_data")]
        public string HexData { get; set; }
        [MaxLength(32)]
        [Column("remote_address_ip")]
        public string RemoteAddressIP { get; set; }
        [Column("is_success")]
        public bool IsSuccess { get; set; }
        [MaxLength(128)]
        [Column("reason")]
        public string Reason { get; set; }
    }
}
