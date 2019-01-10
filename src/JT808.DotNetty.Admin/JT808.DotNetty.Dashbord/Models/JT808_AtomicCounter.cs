using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JT808.DotNetty.Dashbord.Models
{
    [Table("jt808_atomic_counter")]
    public class JT808_AtomicCounter
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("create_time")]
        public long CreateTime { get; set; }

        [Column("success_count")]
        public long SuccessCount { get; set; }

        [Column("fail_count")]
        public long FailCount { get; set; }
    }
}
