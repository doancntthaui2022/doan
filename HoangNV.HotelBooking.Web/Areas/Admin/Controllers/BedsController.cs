using HoangNV.HotelBooking.BusinessLogic.Enum;
using HoangNV.HotelBooking.BusinessLogic.Interface;
using HoangNV.HotelBooking.BusinessLogic.Models;
using HoangNV.HotelBooking.BusinessLogic.Struct;
using HoangNV.HotelBooking.Repository;
using HoangNV.HotelBooking.Web.Areas.Admin.Models;
using HoangNV.HotelBooking.Web.Localization;
using HoangNV.HotelBooking.Web.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BedsController : BaseController
    {
        private readonly BookingContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IBedBS _bedBS;
        public BedsController(BookingContext context, IBedBS bedBS,
            IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _bedBS = bedBS;
            _localizer = localizer;
        }


        public ActionResult Index()
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetList(DataTableAjaxPostModel model)
        {
            var searchCondition = string.IsNullOrEmpty(model.search.value)
                ? string.Empty
                : JsonConvert.DeserializeObject<string>(model.search.value);
            var page = Utility.NormalizePage(model.start, model.length);
            var size = Utility.NormalizeSize(model.length);
            var data = await _bedBS.Search(searchCondition);
            var totalRecord = data.ToList().Count;
            data = data.Skip(size * (page - 1)).Take(size).ToList();
            var ct = new PagingModel(model.page, totalRecord, data);
            return Json(ct);
        }

        [HttpPost]
        public async Task<IActionResult> InsertBeds(BedViewModel bedVM)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");
            if (ModelState.IsValid)
            {
                
                var checkInsert = await _bedBS.Insert(bedVM.BedType);
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
                    Data = ModelState.Where(ms => ms.Value.Errors.Count > 0)
                             .Select(ms =>
                             {
                                 var key = ms.Key;
                                 return new
                                 {
                                     key,
                                     errors = ms.Value.Errors.Select(er => er.ErrorMessage).ToList()
                                 };
                             }).ToList()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBed(int id)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");
            var convenient = await _context.Beds.FindAsync(id);
            if (convenient == null) return NotFound();
            else
            {
                var checkDelete = await _bedBS.Delete(id);
                if (checkDelete)
                {
                    var messageSuccess = SetJsonMsgBox(_localizer["S_C_002_003"].Value, MessageType.Success);
                    return Json(new ResponseJson().Ok(message: messageSuccess));
                }
                var message = SetJsonMsgBox(_localizer["E_C_003"].Value, MessageType.Error);
                return Json(new ResponseJson().Ok(message: message));
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetBedUpdate(int id)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");
            var beds = await _context.Beds.FindAsync(id);
            if (beds != null)
                return Json(new ResponseJson().Ok(data: beds));
            var messageError = SetJsonMsgBox(_localizer["W_C_006_01"].Value, MessageType.Error);
            return Json(new ResponseJson().Error(message: messageError));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBeds(BedViewModel bedVM)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");
            if (ModelState.IsValid)
            {
                
                var checkInsert = await _bedBS.Update(bedVM);
                if (checkInsert)
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
                    Data = ModelState.Where(ms => ms.Value.Errors.Count > 0)
                             .Select(ms =>
                             {
                                 var key = ms.Key;
                                 return new
                                 {
                                     key,
                                     errors = ms.Value.Errors.Select(er => er.ErrorMessage).ToList()
                                 };
                             }).ToList()
                });
            }
        }
    }
}
