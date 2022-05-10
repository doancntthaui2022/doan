using HoangNV.HotelBooking.BusinessLogic.Enum;
using HoangNV.HotelBooking.BusinessLogic.Interface;
using HoangNV.HotelBooking.BusinessLogic.Models;
using HoangNV.HotelBooking.BusinessLogic.Struct;
using HoangNV.HotelBooking.Entities.Entities;
using HoangNV.HotelBooking.Repository;
using HoangNV.HotelBooking.Web.Areas.Admin.Controllers;
using HoangNV.HotelBooking.Web.Areas.Admin.Models;
using HoangNV.HotelBooking.Web.Localization;
using HoangNV.HotelBooking.Web.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly BookingContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IHotelBranchBS _hotelBranchBS;
        private readonly IUserBS _userBS;
        private readonly IRoleBS _roleBS;
        private readonly IRoomBS _roomBS;
        private readonly IRoomTypeBS _roomTypeBS;
        private readonly IBookingBS _bookingBS;

        public AccountController(BookingContext context, IUserBS userBS, IRoleBS roleBS,
            IStringLocalizer<SharedResource> localizer, IHotelBranchBS hotelBranchBS, IRoomBS roomBS, IRoomTypeBS roomTypeBS, IBookingBS bookingBS)
        {
            _context = context;
            _localizer = localizer;
            _userBS = userBS;
            _roleBS = roleBS;
            _hotelBranchBS = hotelBranchBS;
            _roomBS = roomBS;
            _roomTypeBS = roomTypeBS;
            _bookingBS = bookingBS;
        }
        public async Task<IActionResult> Register()
        {
            var userName = HttpContext.Session.GetString("user_name_client");
            if (userName != null)
            {
                HttpContext.Session.Remove("user_name_client");
            }
            var hotelBranchViewModel = await _hotelBranchBS.GetFristOrDefault();
            if (hotelBranchViewModel == null)
                return NotFound();
            ViewData["HotelBranch"] = hotelBranchViewModel;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserAddViewModel model)
        {
            var hotelBranchViewModel = await _hotelBranchBS.GetFristOrDefault();
            if (hotelBranchViewModel == null)
                return NotFound();
            ViewData["HotelBranch"] = hotelBranchViewModel;
            model.RoleId = (int)AccountRole.Client;
            var userName = HttpContext.Session.GetString("user_name_client");
            if (userName != null)
            {
                HttpContext.Session.Remove("user_name_client");
                return RedirectToAction("Register", "Account");
            }
            ModelState.Remove("RoleId");
            if (ModelState.IsValid)
            {

                var checkInsert = await _userBS.Insert(model);
                if (checkInsert)
                {
                    return Redirect("/Account/Login");
                }
                else
                {
                    return View(model);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Login()
        {
            var userName = HttpContext.Session.GetString("user_name_client");
            if (userName != null)
            {
                return RedirectToAction("Index", "Home");
            }
            var hotelBranchViewModel = await _hotelBranchBS.GetFristOrDefault();
            if (hotelBranchViewModel == null)
                return NotFound();
            ViewData["HotelBranch"] = hotelBranchViewModel;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel model)
        {
            var hotelBranchViewModel = await _hotelBranchBS.GetFristOrDefault();
            if (hotelBranchViewModel == null)
                return NotFound();
            ViewData["HotelBranch"] = hotelBranchViewModel;
            if (ModelState.IsValid)
            {
                UserViewModel user = _userBS.GetUsers(model.Username, model.Password);
                if (user != null)
                {
                    if (user.Role == "Client")
                    {
                        HttpContext.Session.SetString("user_name_client", user.Username);
                        HttpContext.Session.SetString("user_role_client", user.Role);
                        HttpContext.Session.SetString("user_full_name_client", user.FullName);
                        return Redirect("/Home");
                    }
                }
                ModelState.AddModelError("Password", "Sai tài khoản hoặc mật khẩu!!!");
                return View(model);
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var hotelBranchViewModel = await _hotelBranchBS.GetFristOrDefault();
            if (hotelBranchViewModel == null)
                return NotFound();
            ViewData["HotelBranch"] = hotelBranchViewModel;
            var userName = HttpContext.Session.GetString("user_name_client");
            if (userName != null)
            {
                HttpContext.Session.Remove("user_name_client");
            }
            return RedirectToAction("Login", "Account");
        }


        [HttpGet]
        public async Task<IActionResult> Update()
        {
            var hotelBranchViewModel = await _hotelBranchBS.GetFristOrDefault();
            if (hotelBranchViewModel == null)
                return NotFound();
            ViewData["HotelBranch"] = hotelBranchViewModel;
            var userName = HttpContext.Session.GetString("user_name_client");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Login", "Account");
            var model = await _userBS.GetUserByName(userName);
            if (model != null)
                return View(model);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateViewModel model)
        {
            var hotelBranchViewModel = await _hotelBranchBS.GetFristOrDefault();
            if (hotelBranchViewModel == null)
                return NotFound();
            ViewData["HotelBranch"] = hotelBranchViewModel;
            var userName = HttpContext.Session.GetString("user_name_client");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Login", "Account");
            if (ModelState.IsValid)
            {

                var updateCheck = await _userBS.UpdateMyUser(model);
                if (updateCheck)
                {
                    ViewData["success"] = string.Format("Cập nhật thành công");
                    HttpContext.Session.SetString("user_full_name_client", model.FullName);
                    return View(model);
                }
                else
                {
                    ViewData["error"] = string.Format("Lỗi hệ thống");
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePassword()
        {
            var hotelBranchViewModel = await _hotelBranchBS.GetFristOrDefault();
            if (hotelBranchViewModel == null)
                return NotFound();
            ViewData["HotelBranch"] = hotelBranchViewModel;
            var userName = HttpContext.Session.GetString("user_name_client");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Login", "Account");
            var model = await _userBS.GetUserUpdatePassByName(userName);
            if (model != null)
                return View(model);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(PassWordUpdateViewModel model)
        {
            var hotelBranchViewModel = await _hotelBranchBS.GetFristOrDefault();
            if (hotelBranchViewModel == null)
                return NotFound();
            ViewData["HotelBranch"] = hotelBranchViewModel;
            var userName = HttpContext.Session.GetString("user_name_client");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Login", "Account");
            var user = await _userBS.GetUserByName(userName);
            var userCheck = await _context.Users.FindAsync(user.UserId);
            if (ModelState.IsValid)
            {
                PasswordHasher<Users> passwordHasher = new PasswordHasher<Users>();
                PasswordVerificationResult result = passwordHasher.VerifyHashedPassword(userCheck, userCheck.PassWord, model.OldPassWord);
                if (result == PasswordVerificationResult.Failed)
                {
                    ModelState.AddModelError("OldPassWord", "Mật khẩu cũ không chính xác.");
                }
            }
            if (ModelState.IsValid)
            {

                var updateCheck = await _userBS.UpdateMyPass(model);
                if (updateCheck)
                {
                    ViewData["success"] = string.Format("Cập nhật thành công");
                    HttpContext.Session.SetString("user_full_name_client", user.FullName);
                    return View(model);
                }
                else
                {
                    ViewData["error"] = string.Format("Lỗi hệ thống");
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListBooking()
        {
            var hotelBranchViewModel = await _hotelBranchBS.GetFristOrDefault();
            if (hotelBranchViewModel == null)
                return NotFound();
            ViewData["HotelBranch"] = hotelBranchViewModel;
            var userName = HttpContext.Session.GetString("user_name_client");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Login", "Account");
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GetList(DataTableAjaxPostModel model)
        {
            var userName = HttpContext.Session.GetString("user_name_client");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Login", "Account");
            var searchCondition = string.IsNullOrEmpty(model.search.value)
                ? new BookingSearchModelAccount()
                : JsonConvert.DeserializeObject<BookingSearchModelAccount>(model.search.value);
            var page = Utility.NormalizePage(model.start, model.length);
            var size = Utility.NormalizeSize(model.length);
            var data = await _bookingBS.SearchAccount(searchCondition,userName);
            var totalRecord = data.ToList().Count;
            data = data.Skip(size * (page - 1)).Take(size).ToList();
            var ct = new PagingModel(model.page, totalRecord, data);
            return Json(ct);
        }

        [HttpGet]
        public async Task<IActionResult> BookingDetail(string id)
        {
            var hotelBranchViewModel = await _hotelBranchBS.GetFristOrDefault();
            if (hotelBranchViewModel == null)
                return NotFound();
            ViewData["HotelBranch"] = hotelBranchViewModel;
            var userName = HttpContext.Session.GetString("user_name_client");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Login", "Account");
            ViewData["RoomType"] = await _roomTypeBS.SearchClient(new RoomTypeSearchModel());
            var bookingDetail = await _bookingBS.GetByIdIncludeClient(id);
            if(bookingDetail==null) return NotFound();
            return View(bookingDetail);
        }
    }
}
