using LifetimeLiveHouse.Models;
using System.Text.Json.Serialization;

namespace LifetimeLiveHouseWebAPI.DTOs.Users
{
    public class LoginDTO
    {
        // 信箱
        public string Email { get; set; } = null!;

        // 密碼
        public string Password { get; set; } = null!;
    }
}
