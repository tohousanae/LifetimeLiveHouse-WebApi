using LifetimeLiveHouse.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class Live
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long LiveID { get; set; }

    [StringLength(40)]
    public string LiveName { get; set; } = null!;

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
    public decimal Admission { get; set; } = 0;

    public string? Discription { get; set; }

    [StringLength(40)]
    public string LiveSong { get; set; } = null!;

    [ForeignKey("BandRole")]
    public string BandRoleID { get; set; } = null!;

    [ForeignKey("Store")]
    public long StoreID { get; set; }

    [ForeignKey("Member")]
    public long MemberID { get; set; }

    [ForeignKey("Event")]
    public string StatusCode { get; set; } = null!;

}