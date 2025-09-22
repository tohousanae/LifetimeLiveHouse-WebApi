using LifetimeLiveHouse.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

public class LoginRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long RecordID { get; set; }

    [StringLength(200)]
    public string? Record { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
    [HiddenInput]
    public DateTime LoginDate { get; set; } = DateTime.Now;

    public long ContinuousLoginDate { get; set; } = 0;

    [ForeignKey("Member")]
    public long MemberID { get; set; }
}
