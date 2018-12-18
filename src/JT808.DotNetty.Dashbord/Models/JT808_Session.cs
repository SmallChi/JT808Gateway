using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JT808.DotNetty.Dashbord.Models
{
    [Table("jt808_session")]
    public class JT808_Session
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
        [Column("last_active_time")]
        public long LastActiveTime { get; set; }
        [Column("start_time")]
        public long StartTime { get; set; }
        [MaxLength(32)]
        [Column("remote_address_ip")]
        public string RemoteAddressIP { get; set; }
        /// <summary>
        /// 通道Id对应多个终端号 
        /// true:第三方平台转发
        /// false:设备上报
        /// </summary>
        [Column("is_transmit")]
        public bool IsTransmit { get; set; }
    }
}
