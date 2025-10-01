using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace LifetimeLiveHouse.Models
{
    public class News
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NewsID { get; set; }

        [StringLength(40)]
        public string NewsTitle { get; set; } = null!;

        [StringLength(10)]
        public string Author { get; set; } = null!;

        public string? Description { get; set; }

        public string Picture { get; set; } = null!;  //照片的檔名規則:GUID+.jpg

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
        [HiddenInput]
        public DateTime PostDate { get; set; } = DateTime.Now;
    }
}
