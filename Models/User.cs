using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SleepDiary1131708.Models
{
    [Table("TableUsers1131708")]
    public class User
    {
        [Key]
        [Column("UserID")]
        public int UserID { get; set; }

        [Required]
        [Column("Account")]
        [StringLength(50)]
        public string Account { get; set; } = string.Empty;

        [Required]
        [Column("Password")]
        [StringLength(50)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Column("Name")]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<SleepLog> SleepRecords { get; set; } = new List<SleepLog>();
    }
}
