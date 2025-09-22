using LifetimeLiveHouse.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class Event
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long EventID { get; set; }

    [StringLength(40)]
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

    [ForeignKey("Store")]
    public long StoreID { get; set; }

    [ForeignKey("Member")]
    public long MemberID { get; set; }

    [ForeignKey("Event")]
    public string StatusCode { get; set; } = null!;

}