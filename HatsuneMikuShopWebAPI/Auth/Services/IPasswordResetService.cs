namespace LifetimeLiveHouseWebAPI.Auth.Services
{
    // 用介面定義屬性、欄位、方法
    public interface IPasswordResetService
    {
        /// <summary>
        /// 請求重設密碼流程：給定 email，若有該使用者則產生 token 並寄信
        /// </summary>
        Task RequestResetAsync(string email);

        /// <summary>
        /// 以 token 重設密碼：驗證 token + 密碼，若合法則更新使用者密碼
        /// </summary>
        /// <param name="token">原始 token</param>
        /// <param name="newPassword">新的密碼</param>
        /// <returns>若成功，回傳成功；若失敗，回傳錯誤訊息</returns>
        Task<ResetResult> ResetPasswordAsync(string token, string newPassword);
    }
}
