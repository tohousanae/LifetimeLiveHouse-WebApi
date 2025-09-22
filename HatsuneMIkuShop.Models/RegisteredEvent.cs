using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifetimeLiveHouse.Models
{
    public class RegisteredEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RecordID { get; set; }

        [ForeignKey("Event")]
        public long EventID { get; set; }

        [ForeignKey("Member")]
        public long MemberID { get; set; }
    }
}