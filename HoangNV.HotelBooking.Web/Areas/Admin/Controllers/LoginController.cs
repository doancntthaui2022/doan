using HoangNV.HotelBooking.BusinessLogic.Interface;
using HoangNV.HotelBooking.BusinessLogic.Models;
using HoangNV.HotelBooking.Entities.Entities;
using HoangNV.HotelBooking.Repository;
using HoangNV.HotelBooking.Repository.Models;
using HoangNV.HotelBooking.Web.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : BaseController
    {
        private readonly BookingContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IUserBS _userBS;
        public LoginController(BookingContext context,
            IStringLocalizer<SharedResource> localizer, IUserBS userBS)
        {
            _context = context;
            _localizer = localizer;
            _userBS = userBS;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserViewModel user = _userBS.GetUsers(model.Username, model.Password);
                    if (user != null)
                    {
                        if(user.Role !="Client")
                        {
                            HttpContext.Session.SetString("user_name", user.Username);
                            HttpContext.Session.SetString("user_role", user.Role);
                            HttpContext.Session.SetString("user_full_name", user.FullName);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                ModelState.AddModelError("user_pasword", "Sai tài khoản hoặc mật khẩu!!!");
                return View(model);

            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Login");
        }
    }
}
