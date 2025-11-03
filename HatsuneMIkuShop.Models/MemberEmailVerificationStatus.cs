using Common.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifetimeLiveHouse.Models
{
    public class MemberEmailVerificationStatus
    {
        [Key]
        [ForeignKey("Member")]
        public long MemberID { get; set; } 

        public bool IsEmailVerified { get; set; } = false;

        public string? EmailVerificationTokenHash { get; set; } = BCrypt.Net.BCrypt.HashPassword(TokenGeneratorHelper.GeneratePassword(100));

        public DateTime? EmailVerificationTokenExpiry { get; set; } = DateTime.Now.AddHours(24);

        public virtual Member Member { get; set; } = null!;

    }
}
