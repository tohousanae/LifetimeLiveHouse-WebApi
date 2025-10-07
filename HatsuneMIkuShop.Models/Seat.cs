using LifetimeLiveHouse.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class Seat
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long SeatID { get; set; }   // 主鍵 P.K

    [ForeignKey("Member")]
    public long MemberID { get; set; }

    [ForeignKey("Store")]
    public long StoreID { get; set; }

    public virtual Member? Member { get; set; }

    public virtual Store Store { get; set; } = null!;

}
