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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomController : BaseController
    {
        private readonly BookingContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IRoomBS _roomBS;
        private readonly IRoomTypeBS _roomTypeBS;
        public RoomController(BookingContext context, IRoomBS roomBS, IRoomTypeBS roomTypeBS,
        IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _roomBS = roomBS;
            _roomTypeBS = roomTypeBS;
            _localizer = localizer;
        }
        public async Task<IActionResult> Index()
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");

            ViewBag.RoomTypes = await GetRoomTypeNames();
            return View();
        }

        private async Task<IEnumerable<SelectListItem>> GetRoomTypeNames()
        {
            var roomTypes = (await _roomTypeBS.Search(new BusinessLogic.Models.RoomTypeSearchModel())).Select(x => new SelectListItem() { Text = x.RoomTypeCode, Value = x.RoomTypeId.ToString() }).ToList();
            roomTypes.Insert(0, new SelectListItem { Text = _localizer["All"], Value = "0", Selected = true });
            return roomTypes;
        }

        public async Task<IActionResult> GetList(DataTableAjaxPostModel model)
        {
            var searchCondition = string.IsNullOrEmpty(model.search.value)
                ? new RoomViewModel()
                : JsonConvert.DeserializeObject<RoomViewModel>(model.search.value);
            var page = Utility.NormalizePage(model.start, model.length);
            var size = Utility.NormalizeSize(model.length);
            var data = await _roomBS.Search(searchCondition);
            var totalRecord = data.ToList().Count;
            data = data.Skip(size * (page - 1)).Take(size).ToList();
            var ct = new PagingModel(model.page, totalRecord, data);
            return Json(ct);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateStatus(int id, int status)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");

            var result = await _roomBS.UpdateStatus(id,status);

            if(result==null)
            {
                var messageError = SetJsonMsgBox(_localizer["W_R_001"].Value, MessageType.Warning);
                return Json(new ResponseJson().Error(message: messageError));
            }    
            if(result=="false")
            {
                var messageError = SetJsonMsgBox(_localizer["W_R_002"].Value, MessageType.Error);
                return Json(new ResponseJson().Error(message: messageError));
            }
            var message = SetJsonMsgBox(_localizer["S_R_001"].Value, MessageType.Success);
            return Json(new ResponseJson().Error(message: message));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");

            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound();
            else
            {
                var checkDelete = await _roomBS.Delete(id);
                if (checkDelete)
                {
                    var messageSuccess = SetJsonMsgBox(_localizer["S_C_002"].Value, MessageType.Success);
                    return Json(new ResponseJson().Ok(message: messageSuccess));
                }
                var message = SetJsonMsgBox(_localizer["E_C_002_01"].Value, MessageType.Error);
                return Json(new ResponseJson().Ok(message: message));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Insert()
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");

            ViewBag.RoomTypes = await GetRoomTypeNames();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetName(int roomTypeId)
        {
            var dem = await _roomBS.GetRoomNameMax(roomTypeId);
            return Json(dem);
        }

        public async Task<IActionResult> InsertRoom(RoomAddViewModel roomAddViewModel)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");
            if (ModelState.IsValid)
            {
                var checkInsert = await _roomBS.Insert(roomAddViewModel);
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
    }
}
