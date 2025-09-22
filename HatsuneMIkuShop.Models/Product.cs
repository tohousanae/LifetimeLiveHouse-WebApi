using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

public class Product
{
    [Key]
    public long ProductID { get; set; }

    [StringLength(1)]
    [ForeignKey("ProductStatus")]
    [Column(TypeName = "nchar")]
    public string StatusCode { get; set; } = null!; // FK

    [StringLength(40)]
    public string ProductName { get; set; } = null!;

    public long ProductNum { get; set; } = 0;   // default 0

    [Column(TypeName = "money")]
    [Range(0, double.MaxValue)]
    public decimal Pricing { get; set; }

    [Column(TypeName = "money")]
    [Range(0, double.MaxValue)]
    public decimal RetailPrice { get; set; }

    [StringLength(200)]
    public string? Description { get; set; }

    [StringLength(50)]
    public string Photo { get; set; } = null!;

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
    [HiddenInput]
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    [StringLength(5)]
    [Column(TypeName = "nchar")]
    [ForeignKey("Category")]
    public string CateID { get; set; } = null!;
}
