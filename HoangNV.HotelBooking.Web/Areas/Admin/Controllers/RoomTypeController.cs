using HoangNV.HotelBooking.BusinessLogic.Enum;
using HoangNV.HotelBooking.BusinessLogic.Interface;
using HoangNV.HotelBooking.BusinessLogic.Models;
using HoangNV.HotelBooking.BusinessLogic.Struct;
using HoangNV.HotelBooking.Entities.Entities;
using HoangNV.HotelBooking.Repository;
using HoangNV.HotelBooking.Repository.Models;
using HoangNV.HotelBooking.Web.Areas.Admin.Models;
using HoangNV.HotelBooking.Web.Localization;
using HoangNV.HotelBooking.Web.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomTypeController : BaseController
    {

        private readonly BookingContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IRoomTypeBS _roomTypeBS;
        private readonly IConvenientTypeBS _convenientTypeBS;
        private readonly IConvenientBS _convenientBS;
        private readonly IBedBS _bedBS;
        public RoomTypeController(BookingContext context, IRoomTypeBS roomTypeBS, IConvenientTypeBS convenientTypeBS,
            IConvenientBS convenientBS, IBedBS bedBS,
            IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _roomTypeBS = roomTypeBS;
            _convenientTypeBS = convenientTypeBS;
            _convenientBS = convenientBS;
            _bedBS = bedBS;
            _localizer = localizer;
        }
        public IActionResult Index()
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
                ? new RoomTypeSearchModel()
                : JsonConvert.DeserializeObject<RoomTypeSearchModel> (model.search.value);
            var page = Utility.NormalizePage(model.start, model.length);
            var size = Utility.NormalizeSize(model.length);
            var data = await _roomTypeBS.Search(searchCondition);
            var totalRecord = data.ToList().Count;
            data = data.Skip(size * (page - 1)).Take(size).ToList();
            var ct = new PagingModel(model.page, totalRecord, data);
            return Json(ct);
        }

        private async Task<IEnumerable<ConvenientTypes>> GetConvenientTypes()
        {
            var convenientTypes = (await _convenientTypeBS.SearchWithConvenient(string.Empty)).Where(x=>x.Convenients.Count()>0).ToList();
            return convenientTypes;
        }
        private async Task<IEnumerable<ConvenientQueryModel>> GetConvenients()
        {
            var convenients = (await _convenientBS.Search(new ConvenientSearchModel() { ConvenientName=String.Empty, ConvenientTypeId=0})).ToList();
            return convenients;
        }
        private async Task<IEnumerable<BedQueryModel>> GetBeds()
        {
            var beds = (await _bedBS.Search(string.Empty)).ToList();
            return beds;
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

            ViewBag.ConvenientTypes = await GetConvenientTypes();
            ViewBag.Convenients = await GetConvenients();
            ViewBag.Beds = await GetBeds();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InsertRoomTypes(RoomTypeViewModel roomTypeViewModel, List<IFormFile> ListImages)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");

            if (ModelState.IsValid)
            {
                if (ListImages != null && ListImages.Count > 0)
                {
                    foreach (var image in ListImages)
                    {
                        if (image.Length > 0 && image != null)
                        {
                            var fileName = Path.GetFileName(image.FileName);
                            var uniqueFileName = GetUniqueFileName(fileName);
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", uniqueFileName);
                            using (var stream = System.IO.File.Create(filePath))
                            {
                                await image.CopyToAsync(stream);
                            }
                            roomTypeViewModel.ImageLinks.Add(uniqueFileName);
                        }
                    }
                }
                var checkInsert = await _roomTypeBS.Insert(roomTypeViewModel);
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

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString()
                      + Path.GetExtension(fileName);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRoomType(int id)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");

            var roomType = await _context.RoomTypes.FindAsync(id);
            if (roomType == null) return NotFound();
            else
            {
                var checkDelete = await _roomTypeBS.Delete(id);
                if (checkDelete)
                {
                    var messageSuccess = SetJsonMsgBox(_localizer["S_C_002"].Value, MessageType.Success);
                    return Json(new ResponseJson().Ok(message: messageSuccess));
                }
                var message = SetJsonMsgBox(_localizer["E_C_003"].Value, MessageType.Error);
                return Json(new ResponseJson().Ok(message: message));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");
            var roomType = await _roomTypeBS.GetByIdInclude(id);
            if (roomType == null) return NotFound();
            
            ViewBag.ConvenientTypes = await GetConvenientTypes();
            ViewBag.Convenients = await GetConvenients();
            ViewBag.Beds = await GetBeds();
            return View(roomType);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRoomType(RoomTypeUpdteVM roomTypeUpdteVM, List<IFormFile> ListImages)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");
            if (ModelState.IsValid)
            {
                if (ListImages != null && ListImages.Count > 0)
                {
                    foreach (var image in ListImages)
                    {
                        if (image.Length > 0 && image != null)
                        {
                            var fileName = Path.GetFileName(image.FileName);
                            var uniqueFileName = GetUniqueFileName(fileName);
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", uniqueFileName);
                            using (var stream = System.IO.File.Create(filePath))
                            {
                                await image.CopyToAsync(stream);
                            }
                            roomTypeUpdteVM.ImageLinks.Add(uniqueFileName);
                        }
                    }
                }
                var checkInsert = await _roomTypeBS.Update(roomTypeUpdteVM);
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
