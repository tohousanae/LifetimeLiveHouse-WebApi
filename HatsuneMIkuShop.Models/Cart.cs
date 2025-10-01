using LifetimeLiveHouse.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class Cart
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long CartID { get; set; }   // 主鍵 P.K

    [ForeignKey("Product")]
    public long ProductID { get; set; }

    [ForeignKey("Member")]
    public long MemberID { get; set; }

    [Range(1, int.MaxValue)]
    public int Count { get; set; } = 0;

    public virtual Product Product { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;

}
