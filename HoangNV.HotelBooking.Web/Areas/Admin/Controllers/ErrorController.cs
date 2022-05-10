using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HoangNV.HotelBooking.Web.Areas.Admin.Controllers
{
    [AllowAnonymous]
    [Area("Admin")]
    public class ErrorController : BaseController
    {
        public IActionResult Index()
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            return View();
        }
    }
}
