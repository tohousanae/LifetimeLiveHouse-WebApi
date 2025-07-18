<<<<<<<< Updated upstream:HatsuneMikuShopAPI/HatsuneMikuShopAPI/DTO/Users/LoginDTO.cs
﻿
========
﻿using System.ComponentModel.DataAnnotations;

<<<<<<<< HEAD:HatsuneMikuShopWebAPI/DTO/Users/LoginDTO.cs
namespace HatsuneMikuShopWebAPI.DTO.Users
========
>>>>>>>> Stashed changes:HatsuneMikuShopAPI/DTO/Users/LoginDTO.cs
namespace HatsuneMikuShopAPI.DTO.Users
>>>>>>>> abfe7156f0424137ca89cb0a14e9dcaa344ae50c:HatsuneMikuShopAPI/DTO/Users/LoginDTO.cs
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
