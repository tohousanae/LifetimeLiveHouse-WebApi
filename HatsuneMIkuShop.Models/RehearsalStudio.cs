using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class RehearsalStudio
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long RehearsalStudioID { get; set; }

    [StringLength(40)]
    public string RehearsalStudioName { get; set; } = null!;

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
    public DateTime StartRentTime { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
    public DateTime OutRentTime { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (OutRentTime < StartRentTime)
        {
            yield return new ValidationResult(
                "OutRentTime 不得早於 RentTime",
                new[] { nameof(OutRentTime) }
            );
        }
    }

    [Column(TypeName = "money")]
    [Range(0, double.MaxValue)]
    public decimal RentFeePerHour { get; set; } = 0;

    public string? Discription { get; set; }

    [StringLength(50)]
    public string RehearsalStudioPhoto { get; set; } = null!;

    [ForeignKey("Store")]
    public long StoreID { get; set; }

    [ForeignKey("Member")]
    public long? MemberID { get; set; }

}