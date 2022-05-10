using HoangNV.HotelBooking.BusinessLogic;
using HoangNV.HotelBooking.BusinessLogic.Enum;
using HoangNV.HotelBooking.BusinessLogic.Interface;
using HoangNV.HotelBooking.BusinessLogic.Models;
using HoangNV.HotelBooking.BusinessLogic.Struct;
using HoangNV.HotelBooking.Entities.Entities;
using HoangNV.HotelBooking.Repository;
using HoangNV.HotelBooking.Web.Areas.Admin.Models;
using HoangNV.HotelBooking.Web.Localization;
using HoangNV.HotelBooking.Web.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : BaseController
    {
        private readonly BookingContext _context;
        private readonly IUserBS _userBS;
        private readonly IRoleBS _roleBS;
        private readonly IStringLocalizer<SharedResource> _localizer;
        public AccountController(BookingContext context, IUserBS userBS, IRoleBS roleBS,
        IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _userBS = userBS;
            _roleBS = roleBS;
            _localizer = localizer;
        }
        public async Task<IActionResult> Index()
        {

            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Admin")
                return RedirectToAction("Index", "Error");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetList(DataTableAjaxPostModel model)
        {
            var searchCondition = string.IsNullOrEmpty(model.search.value)
                ? new UserSearchModel()
                : JsonConvert.DeserializeObject<UserSearchModel>(model.search.value);
            var page = Utility.NormalizePage(model.start, model.length);
            var size = Utility.NormalizeSize(model.length);
            var data = await _userBS.Search(searchCondition);
            var totalRecord = data.ToList().Count;
            data = data.Skip(size * (page - 1)).Take(size).ToList();
            var ct = new PagingModel(model.page, totalRecord, data);
            return Json(ct);
        }
        [HttpGet]
        public async Task<IActionResult> Insert()
        {

            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Admin")
                return RedirectToAction("Index", "Error");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Insert(UserAddViewModel model)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Admin")
                return RedirectToAction("Index", "Error");
            if (ModelState.IsValid)
            {

                var checkInsert = await _userBS.Insert(model);
                if (checkInsert)
                {
                    var messageError = SetJsonMsgBox(_localizer["S_C_001"].Value, MessageType.Success);
                    return Json(new ResponseJson().Ok(message: messageError));
                }
                else
                {
                    var messageError = SetJsonMsgBox(_localizer["E_C_001"].Value, MessageType.Error);
                    return Json(new ResponseJson().Error(message: messageError));
                }
            }
            else
            {
                return Json(new ResponseJson
                {
                    Status = ResponseStatus.Error,
                    Data = GetErrorInModelState()
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Admin")
                return RedirectToAction("Index", "Error");
            var account = await _context.Users.FindAsync(id);
            if (account == null) return NotFound();
            else
            {
                var checkDelete = await _userBS.UpdateUser(id);
                if (checkDelete)
                {
                    var messageSuccess = SetJsonMsgBox(_localizer["S_C_002"].Value, MessageType.Success);
                    return Json(new ResponseJson().Ok(message: messageSuccess));
                }
                var message = SetJsonMsgBox(_localizer["E_C_002"].Value, MessageType.Error);
                return Json(new ResponseJson().Ok(message: message));
            }
        }
        private object GetErrorInModelState()
        {
            return ModelState.Where(ms => ms.Value.Errors.Count > 0)
                             .Select(ms =>
                             {
                                 var key = ms.Key;
                                 return new
                                 {
                                     key,
                                     errors = ms.Value.Errors.Select(er => er.ErrorMessage).ToList()
                                 };
                             }).ToList();
        }

        [HttpGet]
        public async Task<IActionResult> Detail()
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            var model = await _userBS.GetUserByName(userName);
            if(model!=null)
                return View(model);
            return RedirectToAction("Index", "Error");
        }
        [HttpGet]
        public async Task<IActionResult> UpdatePassWord()
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            var model = await _userBS.GetUserUpdatePassByName(userName);
            if (model != null)
                return View(model);
            return RedirectToAction("Index", "Error");
        }
        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateViewModel model)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (ModelState.IsValid)
            {

                var updateCheck = await _userBS.UpdateMyUser(model);
                if (updateCheck)
                {
                    HttpContext.Session.SetString("user_full_name", model.FullName);
                    var messageError = SetJsonMsgBox(_localizer["S_C_003"].Value, MessageType.Success);
                    return Json(new ResponseJson().Ok(message: messageError));
                }
                else
                {
                    var messageError = SetJsonMsgBox(_localizer["E_C_004"].Value, MessageType.Error);
                    return Json(new ResponseJson().Error(message: messageError));
                }
            }
            else
            {
                return Json(new ResponseJson
                {
                    Status = ResponseStatus.Error,
                    Data = GetErrorInModelState()
                });
            }

        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(PassWordUpdateViewModel model)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            var user = await _userBS.GetUserByName(userName);
            var userCheck = await _context.Users.FindAsync(user.UserId);
            if(ModelState.IsValid)
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
                    var messageError = SetJsonMsgBox(_localizer["S_C_003"].Value, MessageType.Success);
                    return Json(new ResponseJson().Ok(message: messageError));
                }
                else
                {
                    var messageError = SetJsonMsgBox(_localizer["E_C_004"].Value, MessageType.Error);
                    return Json(new ResponseJson().Error(message: messageError));
                }
            }
            else
            {
                return Json(new ResponseJson
                {
                    Status = ResponseStatus.Error,
                    Data = GetErrorInModelState()
                });
            }

        }
    }
}
