using LifetimeLiveHouseWebAPI.Modules.User.Interfaces;

namespace LifetimeLiveHouseWebAPI.Services
{
    public class TokenCleanupBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<TokenCleanupBackgroundService> logger) : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ILogger<TokenCleanupBackgroundService> _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Token 清理背景服務已啟動。");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // 因為 BackgroundService 是 Singleton，必須建立 Scope 才能解析 Scoped 服務
                    using var scope = _serviceProvider.CreateScope();
                    var forgetPasswordService = scope.ServiceProvider.GetRequiredService<IForgetPasswordService>();

                    await forgetPasswordService.CleanupExpiredTokensAsync();
                    _logger.LogInformation("過期與已使用的重設密碼 Token 已於 {Time} 成功清除。", DateTime.UtcNow);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "清理 Token 時發生未預期的錯誤。");
                }

                // 設定排程間隔 (例如：每 24 小時執行一次)
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}