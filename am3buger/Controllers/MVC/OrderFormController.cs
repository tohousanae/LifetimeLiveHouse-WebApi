using Microsoft.AspNetCore.Mvc;

namespace am3burger.Controllers.MVC
{
    public class OrderFormController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
