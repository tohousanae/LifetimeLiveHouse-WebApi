using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Caching.Distributed;
using MimeKit;

public class EmailService
{
    private readonly IConfiguration _config;
    private readonly IDistributedCache _cache; // 💡 改為注入 IDistributedCache

    public EmailService(IConfiguration config, IDistributedCache cache)
    {
        _config = config;
        _cache = cache;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
    {
        var authEmail = _config["Mail:AuthEmail"];
        var customSenderEmail = _config["Mail:CustomSenderEmail"];
        var clientId = _config["Mail:ClientId"];
        var clientSecret = _config["Mail:ClientSecret"];
        var refreshToken = _config["Mail:RefreshToken"];
        var server = _config["Mail:Server"] ?? "smtp.gmail.com";
        var port = int.Parse(_config["Mail:Port"] ?? "587");
        var senderName = _config["Mail:SenderName"] ?? "Lifetime LiveHouse";

        var cacheKey = $"GoogleOAuthToken_{authEmail}";

        // 💡 嘗試從 Redis 取得 Token
        var accessToken = await _cache.GetStringAsync(cacheKey);

        if (string.IsNullOrEmpty(accessToken))
        {
            var secrets = new ClientSecrets { ClientId = clientId, ClientSecret = clientSecret };
            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer { ClientSecrets = secrets });
            var credential = new UserCredential(flow, authEmail, new TokenResponse { RefreshToken = refreshToken });

            accessToken = await credential.GetAccessTokenForRequestAsync();

            // 💡 將新的 Token 寫入 Redis，設定 50 分鐘過期

            /* 將快取失效時間設定為剛好 1 小時（60 分鐘）會引發問題，
             * 主要原因在於 Google Access Token 的運作機制與網路環境的不可預測性：
             * Google Token 的生命週期極限：Google 發行的 Access Token 官方有效期限就是嚴格的 60 分鐘，
             * 多一秒都不行。零容錯的緩衝空間：如果你將快取時間也設為 60 分鐘，
             * 當快取用到最後幾秒（例如第 59 分鐘又 50 秒）才被讀出來，
             * 經過網路傳輸的延遲、或是伺服器之間的微小時間差（Clock Drift），
             * 當它送到 Google 伺服器時，Google 計算已經超過 60 分鐘。  
             * SMTP 驗證失敗：Google 的郵件伺服器只要發現 Token 逾時哪怕一瞬間，
             * 就會直接拒絕 AuthenticateAsync 驗證，導致整封註冊驗證信寄不出去。
             * 因此，實務上才會將快取時間刻意縮短（例如設為 50 分鐘），
             * 讓系統在 Token 快過期前提前 10 分鐘向 Google 重新申請一張全新的，
             * 確保每次拿去寄信的 Token 都是絕對安全且未過期的狀態
             */
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(50)
            };
            await _cache.SetStringAsync(cacheKey, accessToken, options);
        }

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(senderName, customSenderEmail));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = htmlMessage };
        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(server, port, SecureSocketOptions.StartTls);

        var oauth2 = new SaslMechanismOAuth2(authEmail, accessToken);
        await client.AuthenticateAsync(oauth2);

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}