using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LifetimeLiveHouseWebAPI.Areas.User.Controllers
{
    [Area("User")]
    [Route("api/User/[controller]")]
    [ApiController]
    public class ForgotPasswordController : ControllerBase
    {
    }
}
