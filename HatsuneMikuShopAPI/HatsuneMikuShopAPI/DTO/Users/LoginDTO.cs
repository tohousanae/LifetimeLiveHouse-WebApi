<<<<<<<< Updated upstream:HatsuneMikuShopAPI/HatsuneMikuShopAPI/DTO/Users/LoginDTO.cs
﻿
========
﻿using System.ComponentModel.DataAnnotations;

>>>>>>>> Stashed changes:HatsuneMikuShopAPI/DTO/Users/LoginDTO.cs
namespace HatsuneMikuShopAPI.DTO.Users
{
    public class LoginDTO
    {
        // 使用者id
        public int Id { get; set; }

        // 信箱
        public string? Email { get; set; }

        // 密碼
        public string? Password { get; set; }
    }
}
