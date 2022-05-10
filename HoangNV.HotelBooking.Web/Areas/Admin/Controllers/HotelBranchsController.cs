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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HotelBranchsController : BaseController
    {
        private readonly BookingContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IHotelBranchBS _hotelBranchBS;

        public HotelBranchsController(BookingContext context,
            IStringLocalizer<SharedResource> localizer, IHotelBranchBS hotelBranchBS)
        {
            _context = context;
            _localizer = localizer;
            _hotelBranchBS = hotelBranchBS;
        }
        // GET: HotelBranchsController
        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> GetList(DataTableAjaxPostModel model)
        //{
        //    var searchCondition = string.IsNullOrEmpty(model.search.value)
        //        ? new HotelBranchSearchModel()
        //        : JsonConvert.DeserializeObject<HotelBranchSearchModel>(model.search.value);
        //    var page = Utility.NormalizePage(model.start, model.length);
        //    var size = Utility.NormalizeSize(model.length);
        //    var data = await _hotelBranchBS.Search(searchCondition);
        //    var totalRecord = data.ToList().Count;
        //    data = data.Skip(size * (page - 1)).Take(size).ToList();
        //    var ct = new PagingModel(model.page, totalRecord, data);
        //    return Json(ct);
        //}

        //[HttpGet]
        //public IActionResult GetInsert()
        //{
        //    return View();
        //}
        
        //[HttpPost]
        //public async Task<IActionResult> InsertHotelBranchs(HotelBranchViewModel hotelBranchVM,List<IFormFile> ListImages)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (ListImages != null && ListImages.Count > 0)
        //        {
        //            foreach (var image in ListImages)
        //            {
        //                if(image.Length>0 && image!=null)
        //                {
        //                    var fileName = Path.GetFileName(image.FileName);
        //                    var uniqueFileName = GetUniqueFileName(fileName);
        //                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", uniqueFileName);
        //                    using (var stream = System.IO.File.Create(filePath))
        //                    {
        //                        await image.CopyToAsync(stream);
        //                    }
        //                    hotelBranchVM.ImageLinks.Add(uniqueFileName);
        //                }    
        //            }
        //        }
        //        var checkInsert = await _hotelBranchBS.Insert(hotelBranchVM);
        //        if (checkInsert)
        //        {
        //            var messageError = SetJsonMsgBox(_localizer["S_C_001"].Value, MessageType.Success);
        //            return Json(new ResponseJson().Ok(message: messageError));
        //        }
        //        else
        //        {
        //            var messageError = SetJsonMsgBox(_localizer["E_C_001"].Value, MessageType.Error);
        //            return Json(new ResponseJson().Error(message: messageError));
        //        }
        //    }
        //    else
        //    {
        //        return Json(new ResponseJson
        //        {
        //            Status = ResponseStatus.Error,
        //            Data = ModelState.Where(ms => ms.Value.Errors.Count > 0)
        //                     .Select(ms =>
        //                     {
        //                         var key = ms.Key;
        //                         return new
        //                         {
        //                             key,
        //                             errors = ms.Value.Errors.Select(er => er.ErrorMessage).ToList()
        //                         };
        //                     }).ToList()
        //        });
        //    }
        //}

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString()
                      + Path.GetExtension(fileName);
        }

        //[HttpPost]
        //public async Task<IActionResult> DeleteHotelBranch(int id)
        //{
        //    var hotel = await _context.HotelBranchs.FindAsync(id);
        //    if (hotel == null) return NotFound();
        //    else
        //    {
        //        var checkDelete = await _hotelBranchBS.Delete(id);
        //        if (checkDelete)
        //        {
        //            var messageSuccess = SetJsonMsgBox(_localizer["S_C_004"].Value, MessageType.Success);
        //            return Json(new ResponseJson().Ok(message: messageSuccess));
        //        }
        //        var message = SetJsonMsgBox(_localizer["E_C_002"].Value, MessageType.Success);
        //        return Json(new ResponseJson().Ok(message: message));
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> GetUpdate()
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");
            var hotelBranchViewModel = await _hotelBranchBS.GetFristOrDefault();
            if (hotelBranchViewModel == null)
                return NotFound();
            return View(hotelBranchViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateHotelBranch(HotelBranchViewModel hotelBranchVM, List<IFormFile> ListImages)
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
                            hotelBranchVM.ImageLinks.Add(uniqueFileName);
                        }
                    }
                }
                var checkInsert = await _hotelBranchBS.Update(hotelBranchVM);
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
