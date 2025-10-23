using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouseWebAPI.DTOs.Users;
using LifetimeLiveHouseWebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NETCore.MailKit.Core;
using Twilio.Rest.Verify.V2.Service;
using Twilio.Types;

public class MemberRegisterServices : IMemberRegisterServices
{
    private readonly LifetimeLiveHouseSysDBContext _context;
    private readonly IEmailService _emailService;
    private readonly string _frontendBaseUrl;
    private readonly TwilioOptions _twilioOpts;

    public MemberRegisterServices(
        LifetimeLiveHouseSysDBContext context,
        IEmailService emailService,
        IConfiguration config,
        IOptions<TwilioOptions> twilioOptions)
    {
        _context = context;
        _emailService = emailService;
        _frontendBaseUrl = config["FrontendBaseUrl"] ?? "https://example.com";
        _twilioOpts = twilioOptions.Value;
    }

    public async Task<ActionResult<string>> MemberRegisterAsync(MemberRegisterDTO dto)
    {
        // 檢查信箱／手機是否已被註冊
        if (await _context.MemberAccount.AnyAsync(u => u.Email == dto.Email))
            return new BadRequestObjectResult("信箱已被註冊");

        if (await _context.Member.AnyAsync(u => u.CellphoneNumber == dto.CellphoneNumber))
            return new BadRequestObjectResult("手機號碼已被註冊");

        // 建立 Member
        var member = new Member
        {
            Name = dto.Name,
            Birthday = dto.Birthday,
            CellphoneNumber = dto.CellphoneNumber
        };
        _context.Member.Add(member);
        await _context.SaveChangesAsync();

        // 建立帳號（未驗證）
        var account = new MemberAccount
        {
            MemberID = member.ID,
            Email = dto.Email,
            PasswordHash = Bycrdto.PasswordHash,
            IsEmailVerified = false,
            EmailVerificationToken = Guid.NewGuid().ToString("N"),
            EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(24),
            IsPhoneVerified = false
        };
        _context.MemberAccounts.Add(account);
        await _context.SaveChangesAsync();

        // 在前端註冊表單內完成手機驗證後送出註冊表單，才進行信箱註冊，若未完成信箱驗證則再登入時再發一次驗證信

        // 發送信箱驗證信
        var emailVerifyLink = $"{_frontendBaseUrl}/verify-email?token={account.EmailVerificationToken}&accountId={account.ID}";
        var emailBody = $@"
            <p>您好 {dto.Name}：</p>
            <p>請點擊以下連結完成信箱驗證：</p>
            <p><a href='{emailVerifyLink}'>點我完成驗證</a></p>
            <p>此連結將在 24 小時後失效。</p>";
        await _emailService.SendAsync(dto.Email, "會員註冊 – 信箱驗證", emailBody, true);

        // 發送手機簡訊驗證（使用 Twilio Verify）
        var serviceSid = _twilioOpts.VerifyServiceSid;
        var toNumber = new PhoneNumber(dto.CellphoneNumber); // 確保為 +8869xxxxxxx 格式
        var verification = await VerificationResource.CreateAsync(
            to: toNumber,
            channel: "sms",
            pathServiceSid: serviceSid
        );

        return new OkObjectResult($"我們已發送驗證信至 {dto.Email} ，請點選信件中的連結完成驗證");
    }

    public async Task<ActionResult<string>> VerifyEmailAsync(long accountId, string token)
    {
        var account = await _context.MemberAccounts
            .FirstOrDefaultAsync(a => a.ID == accountId && a.EmailVerificationToken == token);

        if (account == null)
            return new BadRequestObjectResult("驗證連結無效");

        if (account.EmailVerificationTokenExpiry < DateTime.UtcNow)
            return new BadRequestObjectResult("驗證連結已過期");

        account.IsEmailVerified = true;
        account.EmailVerificationToken = null;
        account.EmailVerificationTokenExpiry = null;

        // 若手機也已驗證，則可視為帳號啟用
        if (account.IsPhoneVerified)
        {
            // 例如設定帳號狀態為啟用（視你的欄位而定）
        }

        await _context.SaveChangesAsync();
        return new OkObjectResult("信箱驗證成功！");
    }

    public async Task<ActionResult<string>> VerifyPhoneAsync(long accountId, string code)
    {
        var account = await _context.MemberAccounts
            .Include(a => a.Member)
            .FirstOrDefaultAsync(a => a.ID == accountId);

        if (account == null)
            return new BadRequestObjectResult("帳號不存在");

        var serviceSid = _twilioOpts.VerifyServiceSid;
        var toNumber = new PhoneNumber(account.Member.CellphoneNumber);

        var verificationCheck = await VerificationCheckResource.CreateAsync(
            to: toNumber,
            code: code,
            pathServiceSid: serviceSid
        );

        if (verificationCheck.Status == "approved")
        {
            account.IsPhoneVerified = true;

            // 若信箱也已驗證，則可視為帳號啟用
            if (account.IsEmailVerified)
            {
                // 例如設定帳號狀態為啟用
            }

            await _context.SaveChangesAsync();
            return new OkObjectResult("手機驗證成功！");
        }
        else
        {
            return new BadRequestObjectResult("驗證碼無效或已過期");
        }
    }
}
