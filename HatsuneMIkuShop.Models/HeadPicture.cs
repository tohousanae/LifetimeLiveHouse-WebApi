using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HatsuneMIkuShop.Models
{
    [PrimaryKey(nameof(UserId), nameof(headPictureFileName))]
    public class HeadPicture
    {
        [Display(Name = "會員Id")]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Display(Name = "頭像圖片檔名")]
        [StringLength(4000)]
        // 姓名
        public string headPictureFileName { get; set; } = null!;
    }
}
