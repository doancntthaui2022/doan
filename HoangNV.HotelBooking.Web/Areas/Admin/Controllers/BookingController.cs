using HoangNV.HotelBooking.BusinessLogic.Enum;
using HoangNV.HotelBooking.BusinessLogic.Interface;
using HoangNV.HotelBooking.BusinessLogic.Models;
using HoangNV.HotelBooking.BusinessLogic.Struct;
using HoangNV.HotelBooking.Entities.Entities;
using HoangNV.HotelBooking.Repository;
using HoangNV.HotelBooking.Web.Areas.Admin.Models;
using HoangNV.HotelBooking.Web.Helper;
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
    public class BookingController : BaseController
    {
        private readonly BookingContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IRoomBS _roomBS;
        private readonly IRoomTypeBS _roomTypeBS;
        private readonly IBookingBS _bookingBS;
        private readonly IThrottlingEmail _emailBS;
        private readonly IViewRenderService _viewRenderService;
        public BookingController(BookingContext context, IRoomBS roomBS, IRoomTypeBS roomTypeBS, IBookingBS bookingBS,
            IThrottlingEmail emailBS, IViewRenderService viewRenderService,
        IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _roomBS = roomBS;
            _roomTypeBS = roomTypeBS;
            _bookingBS = bookingBS;
            _localizer = localizer;
            _emailBS = emailBS;
            _viewRenderService = viewRenderService;
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

        [HttpPost]
        public async Task<IActionResult> GetList(DataTableAjaxPostModel model)
        {
            var searchCondition = string.IsNullOrEmpty(model.search.value)
                ? new BookingSearchModel()
                : JsonConvert.DeserializeObject<BookingSearchModel>(model.search.value);
            var page = Utility.NormalizePage(model.start, model.length);
            var size = Utility.NormalizeSize(model.length);
            var data = await _bookingBS.Search(searchCondition);
            var totalRecord = data.ToList().Count;
            data = data.Skip(size * (page - 1)).Take(size).ToList();
            var ct = new PagingModel(model.page, totalRecord, data);
            return Json(ct);
        }

        [HttpGet]
        [Obsolete]
        public async Task<IActionResult> BookingDetail()
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");
            ViewBag.RoomTypes = await GetRoomTypeNamesInsert();
            return View();
        }

        [Obsolete]
        private async Task<IEnumerable<SelectListItem>> GetRoomTypeNamesInsert()
        {
            var data = await _bookingBS.Search(new BookingSearchModel());
            var countRoomType = data.GroupBy(x => x.RoomTypeId).ToList();
            var roomTypes = data.Select(x =>  _roomTypeBS.GetById(x.RoomTypeId)).Distinct();
            var result = (roomTypes.Select(x => new SelectListItem() { Text = x.RoomTypeCode +"(Phòng trống: "+ countRoomType.FirstOrDefault(y => y.Key == x.RoomTypeId).Count() + ")", Value = x.RoomTypeId.ToString() + '-' + countRoomType.FirstOrDefault(y => y.Key == x.RoomTypeId).Count() + '-'+x.Cost.ToString() })).OrderBy(x=>x.Value).ToList();
            return result;
        }

        [Obsolete]
        [HttpGet]
        public async Task<IActionResult> GetRoomTypeNamesInsertJson(string startDate, string endDate)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");
            var searchModel = new BookingSearchModel() {
                CheckInTime = DateTime.Parse(startDate),
                CheckOutTime = DateTime.Parse(endDate),
            };
                
            var data = await _bookingBS.Search(searchModel);
            var countRoomType = data.GroupBy(x => x.RoomTypeId).ToList();
            var roomTypes= data.Select(x => _roomTypeBS.GetById(x.RoomTypeId)).Distinct();
            
            var result = (roomTypes.Select(x => new SelectListItem() { Text = x.RoomTypeCode + "(Phòng trống: " + countRoomType.FirstOrDefault(y=>y.Key==x.RoomTypeId).Count()+ ")", Value = x.RoomTypeId.ToString() + '-' + countRoomType.FirstOrDefault(y => y.Key == x.RoomTypeId).Count() + '-' + x.Cost.ToString() })).OrderBy(x => x.Value).ToList();
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Booked(BookingDetailViewModel bookingDetailViewModel)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");
            if (ModelState.IsValid)
            {
                var checkInsert = await _bookingBS.Insert(bookingDetailViewModel);
                if (checkInsert !="false")
                {
                    var bookingCustomer = await _bookingBS.GetBookingWithCustomer(checkInsert);

                    if (!string.IsNullOrEmpty(bookingCustomer.Customer.Email.Trim()))
                    {
                        var subject = string.Format("Thông báo trạng thái đặt phòng khách sạn Del Luna - Đồ án Nguyễn Viết Hoàng");
                        var template = "EmailSuccess";

                        Queue<EmailModel> emails = new Queue<EmailModel>();
                        var text = await _viewRenderService.RenderToStringAsync(template, bookingCustomer);
                        emails.Enqueue(new EmailModel
                        {
                            Email = bookingCustomer.Customer.Email,
                            Content = text,
                            Subject = subject
                        });
                        _emailBS.SendEmailsAsync(emails);
                    }
                    var messageError = SetJsonMsgBox(_localizer["S_C_001"].Value, MessageType.Success);
                    return Json(new ResponseJson().Ok(message: messageError));
                }
                else
                {
                    var messageError = SetJsonMsgBox(_localizer["E_C_001"].Value, MessageType.Error);
                    return Json(new ResponseJson().Error(message: messageError));
                }
            }
            return Json(new ResponseJson
            {
                Status = ResponseStatus.Error,
                Data = GetErrorInModelState()
            });
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
        public async Task<IActionResult> ConfirmBooking()
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");
            var data = await _bookingBS.SearchConfirm();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmBooked()
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");
            var data = await _bookingBS.SearchConfirmBooked();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmChecked()
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");
            var data = await _bookingBS.SearchConfirmChecked();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> CheckingDetail(string id)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();
            var data = await _bookingBS.GetByIdInclude(id);
            return View(data);
        }
        [HttpGet]
        public IActionResult ExitDetail(string id)
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
        public async Task<IActionResult> Checking(string id)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();
            else
            {
                var checkDelete = await _bookingBS.CheckingBooking(id);
                if (checkDelete)
                {
                    var bookingCustomer = await _bookingBS.GetBookingWithCustomer(id);

                    if (!string.IsNullOrEmpty(bookingCustomer.Customer.Email.Trim()))
                    {
                        var subject = string.Format("Thông báo trạng thái đặt phòng khách sạn Del Luna - Đồ án Nguyễn Viết Hoàng");
                        var template = "EmailSuccess";

                        Queue<EmailModel> emails = new Queue<EmailModel>();
                        var text = await _viewRenderService.RenderToStringAsync(template, bookingCustomer);
                        emails.Enqueue(new EmailModel
                        {
                            Email = bookingCustomer.Customer.Email,
                            Content = text,
                            Subject = subject
                        });
                        _emailBS.SendEmailsAsync(emails);
                    }
                    var messageSuccess = SetJsonMsgBox(_localizer["S_C_002_001"].Value, MessageType.Success);
                    return Json(new ResponseJson().Ok(message: messageSuccess));
                }
                var message = SetJsonMsgBox(_localizer["E_C_003"].Value, MessageType.Error);
                return Json(new ResponseJson().Ok(message: message));
            }

        }

        [HttpPost]
        public async Task<IActionResult> BookedConfirm(string id)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();
            else
            {
                var checkDelete = await _bookingBS.CheckingBooked(id);
                if (checkDelete)
                {
                    var bookingCustomer = await _bookingBS.GetBookingWithCustomer(id);

                    if (!string.IsNullOrEmpty(bookingCustomer.Customer.Email.Trim()))
                    {
                        var subject = string.Format("Thông báo trạng thái đặt phòng khách sạn Del Luna - Đồ án Nguyễn Viết Hoàng");
                        var template = "EmailSuccess";

                        Queue<EmailModel> emails = new Queue<EmailModel>();
                        var text = await _viewRenderService.RenderToStringAsync(template, bookingCustomer);
                        emails.Enqueue(new EmailModel
                        {
                            Email = bookingCustomer.Customer.Email,
                            Content = text,
                            Subject = subject
                        });
                        _emailBS.SendEmailsAsync(emails);
                    }
                    var messageSuccess = SetJsonMsgBox(_localizer["S_C_002_002"].Value, MessageType.Success);
                    return Json(new ResponseJson().Ok(message: messageSuccess));
                }
                var message = SetJsonMsgBox(_localizer["E_C_003"].Value, MessageType.Error);
                return Json(new ResponseJson().Ok(message: message));
            }

        }
        [HttpPost]
        public async Task<IActionResult> CheckedConfirm(string id)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();
            else
            {
                var checkDelete = await _bookingBS.ExitBooked(id);
                if (checkDelete)
                {
                    var bookingCustomer = await _bookingBS.GetBookingWithCustomer(id);

                    if (!string.IsNullOrEmpty(bookingCustomer.Customer.Email.Trim()))
                    {
                        var subject = string.Format("Thông báo trạng thái đặt phòng khách sạn Del Luna - Đồ án Nguyễn Viết Hoàng");
                        var template = "EmailSuccess";

                        Queue<EmailModel> emails = new Queue<EmailModel>();
                        var text = await _viewRenderService.RenderToStringAsync(template, bookingCustomer);
                        emails.Enqueue(new EmailModel
                        {
                            Email = bookingCustomer.Customer.Email,
                            Content = text,
                            Subject = subject
                        });
                        _emailBS.SendEmailsAsync(emails);
                    }
                    var messageSuccess = SetJsonMsgBox(_localizer["S_C_002_002"].Value, MessageType.Success);
                    return Json(new ResponseJson().Ok(message: messageSuccess));
                }
                var message = SetJsonMsgBox(_localizer["E_C_003"].Value, MessageType.Error);
                return Json(new ResponseJson().Ok(message: message));
            }

        }


        [HttpPost]
        public async Task<IActionResult> GetListExit
            (DataTableAjaxPostModel model)
        {
            var searchCondition = string.IsNullOrEmpty(model.search.value)
                ? new BookingSearcResulthModel()
                : JsonConvert.DeserializeObject<BookingSearcResulthModel>(model.search.value);
            var page = Utility.NormalizePage(model.start, model.length);
            var size = Utility.NormalizeSize(model.length);
            var data = await _bookingBS.SearchResult(searchCondition);
            var totalRecord = data.ToList().Count;
            data = data.Skip(size * (page - 1)).Take(size).ToList();
            var ct = new PagingModel(model.page, totalRecord, data);
            return Json(ct);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(string id)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");
            var convenient = await _context.Bookings.FindAsync(id);
            if (convenient == null) return NotFound();
            else
            {
                var bookingCustomer = await _bookingBS.GetBookingWithCustomer(id);

                if (!string.IsNullOrEmpty(bookingCustomer.Customer.Email.Trim()))
                {
                    var subject = string.Format("Thông báo hủy đặt phòng khách sạn Del Luna - Đồ án Nguyễn Viết Hoàng");
                    var template = "EmailError";

                    Queue<EmailModel> emails = new Queue<EmailModel>();
                    var text = await _viewRenderService.RenderToStringAsync(template, bookingCustomer);
                    emails.Enqueue(new EmailModel
                    {
                        Email = bookingCustomer.Customer.Email,
                        Content = text,
                        Subject = subject
                    });
                    _emailBS.SendEmailsAsync(emails);
                }
                var checkDelete = await _bookingBS.Delete(id);
                if (checkDelete)
                {
                    
                    var messageSuccess = SetJsonMsgBox(_localizer["S_C_002_001"].Value, MessageType.Success);
                    return Json(new ResponseJson().Ok(message: messageSuccess));
                }
                var message = SetJsonMsgBox(_localizer["E_C_003"].Value, MessageType.Error);
                return Json(new ResponseJson().Ok(message: message));
            }
        }

        [HttpGet]
        public async Task<IActionResult> DowloadCSV(string searchValue)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");
            var searchCondition = string.IsNullOrEmpty(searchValue)
                 ? new BookingSearcResulthModel()
                 : JsonConvert.DeserializeObject<BookingSearcResulthModel>(searchValue);
            var list = await _bookingBS.SearchResultCSV(searchCondition);
            var stream = await CsvUtil.DownloadCsvUTF8WithBomHNV(list);
            return File(stream, "text/csv", "BookingRevenue.csv");
        }
    }
}
