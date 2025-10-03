using LifetimeLiveHouse.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class EventStatus
{
    [Key]
    [StringLength(1)]
    [Column(TypeName = "nchar")]
    [Required]
    public string StatusCode { get; set; } = null!;

    [StringLength(10)]
    [Required]
    public string Status { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}