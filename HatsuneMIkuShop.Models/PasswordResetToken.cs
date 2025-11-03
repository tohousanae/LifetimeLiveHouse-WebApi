
using Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifetimeLiveHouse.Models
{
    // 會員資料表
    public partial class PasswordResetToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }   // 主鍵 P.K

        [ForeignKey("Member")]
        public long MemberID { get; set; }

        [Required]
        public string TokenHash { get; set; } = BCrypt.Net.BCrypt.HashPassword(TokenGeneratorHelper.GeneratePassword(100)); // 重設密碼的 Token 雜湊值

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime ExpiresAt { get; set; } = DateTime.Now.AddHours(1);

        public bool Used { get; set; } = false;

        public DateTime? UsedAt { get; set; }

        public virtual Member Member { get; set; } = null!;
    }
}
