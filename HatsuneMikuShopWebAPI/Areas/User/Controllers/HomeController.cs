using Microsoft.AspNetCore.Mvc;

namespace LifetimeLiveHouseWebAPI.Areas.User.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
//參考資料放置：


//1. 資料夾、物件命名規則：
//helper、service、handler都是大家用來分類程式類別的名稱，目的是為了整理程式碼，並賦予它們一定的意圖、範圍和上下文。不同的專案名稱可能不一樣，但很多概念都是相同的。因為這是 C#，所以我們會參考微軟的類別命名方式來組織程式碼。
//xxxHelper - 靜態類別，包含純函式。非常通用，沒有狀態，程式碼很精簡
//xxxUtility/Manager - 包含與某種上下文相關的方法集合。除了注入的類別之外，沒有狀態。通常執行一些不適合放在領域模型中的業務邏輯。方法大小為小型/中型
//xxxRepository - 持久層存取
//xxxMapper - 將一個物件轉換成另一個物件
//xxxService - 執行一些會影響應用程式外部或作為操作流程一部分的行為。通常有狀態。使用前面提到的各種類別來執行任務。可能包含一些複雜的邏輯編排
//xxxHandler - 響應特定請求並執行一些業務邏輯（通過前面提到的某些類別的組合）
//還有很多名稱與設計模式相關，例如 builder、factory、observer 等等。
//我的很多命名方式都參考了微軟 .NET 程式碼、領域驅動設計和 CQRS 的做法。


// 2. API回應時間建議參考：https://ithelp.ithome.com.tw/articles/10363630


//3. 會員信箱、手機驗證流程教學：https://www.tsg.com.tw/blog-detail9-232-1-verify.htm


//4. 提升可維護性的方法：
//我打開了微軟官方的.NET 依賴注入與服務設計指南
//。根據該文件與專業軟體設計實務，你的情況確實應該考慮將函數拆成多個服務類別與介面，理由如下：

//✅ 1. 職責單一原則（Single Responsibility Principle）
//目前 MemberRegisterServices 同時處理：
//帳號建立（InsertMember, InsertMemberAccount）
//驗證狀態管理（InsertMemberVerificationStatus）
//電子郵件驗證（VerifyEmailAsync）
//手機驗證（VerifyPhoneAsync）
//驗證發送（Twilio、MailKit）

//這代表類別有多重職責。依據 SOLID 原則，應拆成數個小型服務，例如：
//IMemberRegistrationService       // 負責建立會員與帳號
//IEmailVerificationService        // 負責信箱驗證邏輯
//IPhoneVerificationService        // 負責手機驗證邏輯
//IMemberVerificationStatusService // 負責狀態儲存與查詢

//✅ 2. 減少類別長度、提升可維護性
//當服務文件超過一頁（通常 >300 行），維護會變得困難：
//Debug 時要上下捲動找函數。
//版本控制中，每次修改會影響過多邏輯。
//單元測試難以隔離測試單一功能。
//拆分後，每個服務文件約 100 行以下，閱讀性、測試性、重用性都會明顯提升。
//✅ 3. 提升可測試性

//每個服務可以獨立進行 Mock 測試，例如：
//_mockEmailService.Verify(x => x.SendAsync(...), Times.Once);

//而不必同時啟動整個註冊流程的資料庫、Twilio、MailKit 等依賴。

//✅ 4. 方便未來擴充與替換
//例如未來改用 AWS SNS 取代 Twilio，你只需替換 IPhoneVerificationService 的實作，而不需修改 MemberRegisterServices 的程式。

//📦 推薦的檔案結構範例
//Services/
//│
//├── Member/
//│   ├── MemberRegistrationService.cs
//│   ├── MemberAccountService.cs
//│   ├── MemberVerificationStatusService.cs
//│
//├── Verification/
//│   ├── EmailVerificationService.cs
//│   ├── PhoneVerificationService.cs
//│
//Interfaces/
//│   ├── IMemberRegistrationService.cs
//│   ├── IEmailVerificationService.cs
//│   ├── IPhoneVerificationService.cs
//│   ├── IMemberVerificationStatusService.cs

//✅ 結論
//當一個服務類別的功能橫跨多個領域（註冊、驗證、資料處理等），或函數超過一頁（>5 個以上的主要公用方法）時，就應該拆成獨立服務與介面。

// [C#]AsNoTracking()方法-不使用追蹤以增進查詢效能：https://ithelp.ithome.com.tw/articles/10310644

//C# — 封裝性 Public vs Private https://medium.com/jason%E7%9A%84%E5%89%8D%E7%AB%AF%E4%B9%8B%E8%B7%AF/c-%E5%B0%81%E8%A3%9D%E6%80%A7-public-vs-private-76549e407a6d

// 流程圖製作教學 https://www.thingsaboutweb.dev/zh-TW/posts/flowchart-introduction

// readme.md 文件撰寫範例參考：
// 1. https://github.com/hsiangfeng/README-Example-Template
// 2. https://github.com/joechen0730/Resume?tab=readme-ov-file