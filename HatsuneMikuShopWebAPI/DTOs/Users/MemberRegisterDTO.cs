
using System.ComponentModel.DataAnnotations;

namespace LifetimeLiveHouseWebAPI.DTOs.Users
{
    public class MemberRegisterDTO
    {
        // Member
        [StringLength(40)]
        public string Name { get; set; } = null!;    // 暱稱

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
        public DateTime Birthday { get; set; }   // 生日


        // MemberAccount
        [StringLength(30)]
        [EmailAddress]
        [Required]
        public string Email { get; set; } = null!;

        [StringLength(200)]
        public string Password { get; set; } = null!;

    }
}
