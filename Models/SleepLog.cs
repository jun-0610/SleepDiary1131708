using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SleepDiary1131708.Models
{
    [Table("TableSleepRecords1131708")]
    public class SleepLog
    {
        [Key]
        [Column("RecordID")]
        public int RecordID { get; set; }

        [Required]
        [Column("UserID")]
        public int UserID { get; set; }

        [Required]
        [Column("SleepDate")]
        [DataType(DataType.Date)]
        public DateTime SleepDate { get; set; }

        [Required]
        [Column("SleepHours", TypeName = "decimal(4,1)")]
        public decimal SleepHours { get; set; }

        [Required]
        [Column("QualityScore")]
        public int QualityScore { get; set; }

        [Column("Reason")]
        [StringLength(200)]
        public string? Reason { get; set; }

        [Column("Remark")]
        [StringLength(200)]
        public string? Remark { get; set; }

        public virtual User? User { get; set; }
    }
}
