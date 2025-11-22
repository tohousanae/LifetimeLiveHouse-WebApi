using Humanizer;
using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouse.Models;
using LifetimeLiveHouse.Services.Member.Interfaces;
using LifetimeLiveHouseWebAPI.DTOs.Users;
using LifetimeLiveHouseWebAPI.Helpers;
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
        //if (await _context.MemberAccount.AnyAsync(u => u.Email == dto.Email))
        //    return new BadRequestObjectResult("信箱已被註冊");

        //if (await _context.Member.AnyAsync(u => u.CellphoneNumber == dto.CellphoneNumber))
        //    return new BadRequestObjectResult("手機號碼已被註冊");
        var checkResult = await CheckEmailOrCellphoneAlreadyRegisteredAsync(dto.Email, dto.CellphoneNumber);
        if (checkResult != null)
        {
            return checkResult;
        }

        //// 建立 Member
        //var member = new Member
        //{
        //    Name = dto.Name,
        //    Birthday = dto.Birthday,
        //    CellphoneNumber = dto.CellphoneNumber
        //};
        //_context.Members.Add(member);
        //await _context.SaveChangesAsync();

        //// 建立帳號（未驗證）
        //var account = new MemberAccount
        //{
        //    MemberID = member.ID,
        //    Email = dto.Email,
        //    PasswordHash = dto.PasswordHash,
        //    IsEmailVerified = false,
        //    EmailVerificationToken = Guid.NewGuid().ToString("N"),
        //    EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(24),
        //    IsPhoneVerified = false
        //};
        //_context.MemberAccounts.Add(account);
        //await _context.SaveChangesAsync();

        //var verificationStatus = new MemberVerificationStatus
        //{
        //    MemberID = member.MemberID,
        //    IsEmailVerified = false,
        //    EmailVerificationToken = Guid.NewGuid().ToString("N"),
        //    EmailVerificationTokenExpiry = DateTime.Now.AddHours(24),
        //    IsPhoneVerified = false
        //};
        //_context.MemberVerificationStatus.Add(verificationStatus);
        //await _context.SaveChangesAsync();

        // 發送信箱驗證信
        var emailVerifyLink = $"{_frontendBaseUrl}/verify-email?token={Uri.EscapeDataString(account.EmailVerificationToken)}&accountId={account.ID}";
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
        return new OkObjectResult($"我們已發送驗證信至您的信箱({dto.Email})，請點選信件中的連結完成驗證。");
    }
    private async Task<ActionResult<string>?> CheckEmailOrCellphoneAlreadyRegisteredAsync(string email, string cellphoneNumber)
    {
        if (await _context.MemberAccount.AnyAsync(u => u.Email == email))
        {
            return new BadRequestObjectResult("信箱已被註冊");
        }

        if (await _context.Member.AnyAsync(u => u.CellphoneNumber == cellphoneNumber))
        {
            return new BadRequestObjectResult("手機號碼已被註冊");
        }

        return null;  // 表示都沒問題
    }
    public async Task<Member> InsertMember(MemberRegisterDTO dto)
    {
        var member = new Member
        {
            Name = dto.Name,
            Birthday = dto.Birthday,
            CellphoneNumber = dto.CellphoneNumber
        };

        _context.Member.Add(member);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            throw;
        }

        // 新增帳號並回傳 Member（或回傳帳號，看你的需求）
        await InsertMemberAccount(member.MemberID, dto.Email, dto.Password);
        await InsertMemberEmailVerificationStatus(member.MemberID);
        return member;
    }

    public async Task<MemberAccount> InsertMemberAccount(long memberId, string email, string password)
    {
        var account = new MemberAccount
        {
            MemberID = memberId,
            Email = email,
            Password = BCrypt.Net.BCrypt.HashPassword(password),
            // 你看是否還有其他欄位要設定，例如 IsVerified = false
        };

        _context.MemberAccount.Add(account);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            throw;
        }

        return account;
    }
    public async Task<MemberEmailVerificationStatus> InsertMemberEmailVerificationStatus(long memberId)
    {
        var verificationStatus = new MemberEmailVerificationStatus
        {
            MemberID = memberId,
            IsEmailVerified = false,
            EmailVerificationTokenHash = PasswordTokenHelper.GeneratePassword(100),
            //EmailVerificationTokenExpiry = DateTime.Now.AddHours(24), 已在模型設定
            IsPhoneVerified = false
        };
        _context.MemberVerificationStatus.Add(verificationStatus);
        await _context.SaveChangesAsync();

        _context.MemberAccount.Add(account);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            throw;
        }
        return verificationStatus;
    }
    public async Task<ActionResult<string>> VerifyEmailAsync(long accountId, string token)
    {
        var account = await _context.MemberAccount
            .FirstOrDefaultAsync(a => a.MemberID == accountId && a.EmailVerificationToken == token);

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
        var account = await _context.MemberAccount
            .Include(a => a.Member)
            .FirstOrDefaultAsync(a => a.MemberID == accountId);

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

