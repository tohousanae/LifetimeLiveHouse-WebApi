using LifetimeLiveHouse.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class Event
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long EventID { get; set; }

    [StringLength(40)]
    [Required]
    public string EventName { get; set; } = null!;

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
    public DateTime StartTime { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
    public DateTime EndTime { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndTime < StartTime)
        {
            yield return new ValidationResult(
                "OutRentTime 不得早於 RentTime",
                new[] { nameof(EndTime) }
            );
        }
    }

    [Column(TypeName = "money")]
    [Range(0, double.MaxValue)]
    public decimal RegistrationFee { get; set; } = 0;

    public string? Discription { get; set; }

    [StringLength(50)]
    public string EventPicture { get; set; } = null!;

    [ForeignKey("Store")]
    public long StoreID { get; set; }

    [ForeignKey("Member")]
    public long MemberID { get; set; }

    [ForeignKey("EventStatus")]
    [Required]
    public string StatusCode { get; set; } = null!;

    public virtual ICollection<RegisteredEvent> RegisteredEvents { get; set; } = new List<RegisteredEvent>();

    public virtual Member Member { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;

    public virtual MemberStatus MemberStatus { get; set;} = null!;
}