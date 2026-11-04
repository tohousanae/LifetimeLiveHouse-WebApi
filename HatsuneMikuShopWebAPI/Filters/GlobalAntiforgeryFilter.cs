using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class GlobalAntiforgeryFilter(IAntiforgery antiforgery) : IAsyncActionFilter
{
    private readonly IAntiforgery _antiforgery = antiforgery;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var httpContext = context.HttpContext;
        var method = httpContext.Request.Method;

        // 1. 安全方法 (GET, HEAD, OPTIONS, TRACE) 不需要防禦，直接放行
        if (HttpMethods.IsGet(method) ||
            HttpMethods.IsHead(method) ||
            HttpMethods.IsOptions(method) ||
            HttpMethods.IsTrace(method))
        {
            await next();
            return;
        }

        // 2. 檢查該 Controller 或 Action 是否有加上 [IgnoreAntiforgeryToken] 標籤
        // 有標註的公開端點 (如登入、忘記密碼) 就能直接略過驗證
        bool hasIgnore = context.ActionDescriptor.EndpointMetadata
            .Any(em => em is IgnoreAntiforgeryTokenAttribute);

        if (hasIgnore)
        {
            await next();
            return;
        }

        try
        {
            // 3. 自動執行內建驗證 (比對 Cookie 中的 XSRF-TOKEN 與 Header 中的 X-XSRF-TOKEN)
            await _antiforgery.ValidateRequestAsync(httpContext);
        }
        catch (AntiforgeryValidationException)
        {
            // 4. 驗證失敗時，優雅地回傳 400 Bad Request，絕不噴 500 錯誤
            context.Result = new BadRequestObjectResult(new { message = "CSRF 驗證失敗：安全性權杖無效或遺失。" });
            return;
        }

        // 5. 驗證通過，執行目標 API 邏輯
        await next();
    }
}