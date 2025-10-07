
using LifetimeLiveHouse.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long OrderID { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
    public DateTime OrderDate { get; set; } = DateTime.Now; //訂單日期

    [StringLength(20)]
    public string oTel { get; set; } = null!;

    [StringLength(200)]
    public string? Note { get; set; }

    [ForeignKey("Member")]
    public long MemberID { get; set; }

    [ForeignKey("Employee")]
    public long? EmployeeID { get; set; }

    [ForeignKey("PayType")]
    [StringLength(2)]
    public string? PayCode { get; set; }

    [ForeignKey("OrderStatus")]
    [StringLength(1)]
    public string StatusCode { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();

    public virtual OrderStatus OrderStatus { get; set; } = null!;

    public virtual PayType PayType { get; set; } = null!;
}