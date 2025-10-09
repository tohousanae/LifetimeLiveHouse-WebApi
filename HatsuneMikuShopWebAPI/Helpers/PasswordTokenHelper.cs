using System.Security.Cryptography;
using System.Text;

namespace LifetimeLiveHouseWebAPI.Helpers
{
    // 密碼產生工具類別
    public static class PasswordTokenHelper
    {
        private const string Upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Lower = "abcdefghijklmnopqrstuvwxyz";
        private const string Digits = "0123456789";
        private const string Symbols = "!@#$%^&*()-_=+[]{};:,.<>?/";

        public static string GeneratePassword(int length = 100) // 預設密碼長度 100
        {
            string allChars = Upper + Lower + Digits + Symbols;

            StringBuilder password = new StringBuilder();
            using var rng = RandomNumberGenerator.Create();

            // 確保每種類型都有
            password.Append(GetRandomChar(Upper, rng));
            password.Append(GetRandomChar(Lower, rng));
            password.Append(GetRandomChar(Digits, rng));
            password.Append(GetRandomChar(Symbols, rng));

            for (int i = password.Length; i < length; i++)
            {
                password.Append(GetRandomChar(allChars, rng));
            }

            return ShuffleString(password.ToString(), rng);
        }

        private static char GetRandomChar(string chars, RandomNumberGenerator rng)
        {
            byte[] randomByte = new byte[1];
            rng.GetBytes(randomByte);
            return chars[randomByte[0] % chars.Length];
        }

        private static string ShuffleString(string input, RandomNumberGenerator rng)
        {
            char[] array = input.ToCharArray();
            for (int i = array.Length - 1; i > 0; i--)
            {
                byte[] buffer = new byte[1];
                rng.GetBytes(buffer);
                int j = buffer[0] % (i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
            return new string(array);
        }
    }
}
