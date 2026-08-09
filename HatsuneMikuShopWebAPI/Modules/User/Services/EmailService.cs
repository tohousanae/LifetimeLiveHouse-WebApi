using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
    {
        // 1. 從組態動態讀取所有設定，徹底消滅寫死的機密與設定
        var authEmail = _config["Mail:AuthEmail"];                 // 負責向 Google 驗證身分的 Gmail 帳號
        var customSenderEmail = _config["Mail:CustomSenderEmail"]; // 實際顯示給使用者的自訂網域信箱

        var clientId = _config["Mail:ClientId"];
        var clientSecret = _config["Mail:ClientSecret"];
        var refreshToken = _config["Mail:RefreshToken"];

        var server = _config["Mail:Server"] ?? "smtp.gmail.com";
        var port = int.Parse(_config["Mail:Port"] ?? "587");
        var senderName = _config["Mail:SenderName"] ?? "Lifetime LiveHouse";

        // 利用 Refresh Token 向 Google 換取即時的 Access Token
        var secrets = new ClientSecrets
        {
            ClientId = clientId,
            ClientSecret = clientSecret
        };

        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = secrets
        });

        // ⚠️ 注意：這裡必須使用 authEmail (Gmail 帳號) 來取得授權 Token
        var credential = new UserCredential(flow, authEmail, new TokenResponse
        {
            RefreshToken = refreshToken
        });

        // 取得效期只有一小時的臨時 Access Token
        var token = await credential.GetAccessTokenForRequestAsync();

        // 2. 組合郵件內容
        var message = new MimeMessage();

        // ⚠️ 注意：信件表頭使用 customSenderEmail 作為顯示的寄件者
        message.From.Add(new MailboxAddress(senderName, customSenderEmail));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = htmlMessage };
        message.Body = bodyBuilder.ToMessageBody();

        // 3. 透過 MailKit 使用 OAuth2 機制寄信
        using var client = new SmtpClient();
        await client.ConnectAsync(server, port, SecureSocketOptions.StartTls);

        // ⚠️ 注意：這裡依然必須使用 authEmail 進行 SMTP 伺服器的 OAuth2 登入驗證
        var oauth2 = new SaslMechanismOAuth2(authEmail, token);
        await client.AuthenticateAsync(oauth2);

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}