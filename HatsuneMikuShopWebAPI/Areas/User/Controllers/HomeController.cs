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

//資料夾、物件命名規則：
//helper、service、handler都是大家用來分類程式類別的名稱，目的是為了整理程式碼，並賦予它們一定的意圖、範圍和上下文。不同的專案名稱可能不一樣，但很多概念都是相同的。因為這是 C#，所以我們會參考微軟的類別命名方式來組織程式碼。
//xxxHelper - 靜態類別，包含純函式。非常通用，沒有狀態，程式碼很精簡
//xxxUtility/Manager - 包含與某種上下文相關的方法集合。除了注入的類別之外，沒有狀態。通常執行一些不適合放在領域模型中的業務邏輯。方法大小為小型/中型
//xxxRepository - 持久層存取
//xxxMapper - 將一個物件轉換成另一個物件
//xxxService - 執行一些會影響應用程式外部或作為操作流程一部分的行為。通常有狀態。使用前面提到的各種類別來執行任務。可能包含一些複雜的邏輯編排
//xxxHandler - 響應特定請求並執行一些業務邏輯（通過前面提到的某些類別的組合）
//還有很多名稱與設計模式相關，例如 builder、factory、observer 等等。
//我的很多命名方式都參考了微軟 .NET 程式碼、領域驅動設計和 CQRS 的做法。

//API回應時間建議參考：https://ithelp.ithome.com.tw/m/articles/10363630
//會員信箱、手機驗證流程教學：https://www.tsg.com.tw/blog-detail9-232-1-verify.htm