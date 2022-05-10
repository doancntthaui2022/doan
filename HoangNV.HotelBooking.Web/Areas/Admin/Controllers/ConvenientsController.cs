using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HoangNV.HotelBooking.Entities.Entities;
using HoangNV.HotelBooking.Repository;
using HoangNV.HotelBooking.Web.Areas.Admin.Models;
using Newtonsoft.Json;
using HoangNV.HotelBooking.Web.Utils;
using Microsoft.Extensions.Localization;
using HoangNV.HotelBooking.Web.Localization;
using HoangNV.HotelBooking.BusinessLogic.Interface;
using HoangNV.HotelBooking.BusinessLogic.Models;
using HoangNV.HotelBooking.BusinessLogic.Enum;
using HoangNV.HotelBooking.BusinessLogic.Struct;
using Microsoft.AspNetCore.Http;

namespace HoangNV.HotelBooking.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ConvenientsController : BaseController
    {
        private readonly BookingContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IConvenientTypeBS _convenientTypeBS;
        private readonly IConvenientBS _convenientBS;

        public ConvenientsController(BookingContext context, IStringLocalizer<SharedResource> localizer,
            IConvenientTypeBS convenientTypeBS, IConvenientBS convenientBS)
        {
            _context = context;
            _localizer = localizer;
            _convenientBS = convenientBS;
            _convenientTypeBS = convenientTypeBS;
        }

        // GET: Admin/Convenients
        public async Task<IActionResult> Index()
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");
            ViewBag.convenientTypes = await GetConvenientTypeNames();
            return View();
        }

        private async Task<List<SelectListItem>> GetConvenientTypeNames()
        {
            var convenientTypes = (await _convenientTypeBS.Search(string.Empty)).Select(x => new SelectListItem() { Text = x.ConvenientTypeName, Value = x.ConvenientTypeId.ToString() }).ToList();
            convenientTypes.Insert(0, new SelectListItem { Text = _localizer["All"], Value = "0", Selected = true });
            return convenientTypes;
        }

        [HttpPost]
        public async Task<IActionResult> GetList(DataTableAjaxPostModel model)
        {
            var searchCondition = string.IsNullOrEmpty(model.search.value)
                ? new ConvenientSearchModel()
                : JsonConvert.DeserializeObject<ConvenientSearchModel>(model.search.value);
            var page = Utility.NormalizePage(model.start, model.length);
            var size = Utility.NormalizeSize(model.length);
            var data = await _convenientBS.Search(searchCondition);
            var totalRecord = data.ToList().Count;
            data = data.Skip(size * (page - 1)).Take(size).ToList();
            var ct = new PagingModel(model.page, totalRecord, data);
            return Json(ct);
        }

        [HttpPost]
        public async Task<IActionResult> InsertConvenient(ConvenientViewModel convenient)
        {

            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");

            if (ModelState.IsValid)
            {
                
                var checkInsert = await _convenientBS.Insert(convenient);
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
        public async Task<IActionResult> DeleteConvenient(int id)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");
            var convenient= await _context.Convenients.FindAsync(id);
            if (convenient == null) return NotFound();
            else
            {
                var checkDelete = await _convenientBS.Delete(id);
                if (checkDelete)
                {
                    var messageSuccess = SetJsonMsgBox(_localizer["S_C_002"].Value, MessageType.Success);
                    return Json(new ResponseJson().Ok(message: messageSuccess));
                }
                var message = SetJsonMsgBox(_localizer["E_C_002"].Value, MessageType.Success);
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

        [HttpPost]
        public async Task<IActionResult> UpdateConvenient(ConvenientViewModel convenient)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");
            if (ModelState.IsValid)
            {

                var checkInsert = await _convenientBS.Update(convenient);
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
                    Data = GetErrorInModelState()
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetConvenientUpdate(int id)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");
            var convenient = await _context.Convenients.FindAsync(id);
            if(convenient!=null)
                return Json(new ResponseJson().Ok(data:convenient));
            var messageError = SetJsonMsgBox(_localizer["W_C_006_01"].Value, MessageType.Error);
            return Json(new ResponseJson().Error(message: messageError));
        }
    }
}
