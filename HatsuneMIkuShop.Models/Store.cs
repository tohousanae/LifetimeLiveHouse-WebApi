using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifetimeLiveHouse.Models;

public partial class Store
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long StoreID { get; set; }

    [Required]
    [MaxLength(40)]
    public string StoreName { get; set; } = null!;

    [Required]
    [Column(TypeName = "money")]
    [Range(0, double.MaxValue)]
    public decimal RentFeePerHour { get; set; }

    [Required]
    [MaxLength(20)]
    public string sTel { get; set; } = null!; // 電話

    [Required]
    [MaxLength(50)]
    public string Address { get; set; } = null!; // 地址

    public string? Discription { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
    [HiddenInput]
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    [Required]
    public string Region { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();

    public virtual ICollection<RehearsalStudio> RehearsalStudios { get; set; } = new List<RehearsalStudio>();

    public virtual ICollection<Live> Lives { get; set; } = new List<Live>();

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Instrument> Instruments { get; set; } = new List<Instrument>();
}
