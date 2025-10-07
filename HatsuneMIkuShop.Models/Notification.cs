using LifetimeLiveHouse.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class Notification
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long NotificationID { get; set; }

    [StringLength(200)]
    public string? Description { get; set; }

    public bool Readed { get; set; }

    [ForeignKey("Member")]
    public long MemberID { get; set; }

    public virtual Member Member { get; set; } = null!;
}
