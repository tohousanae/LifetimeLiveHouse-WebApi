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

            // 💡 將新的 Token 寫入 Redis，設定 60 分鐘過期
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
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