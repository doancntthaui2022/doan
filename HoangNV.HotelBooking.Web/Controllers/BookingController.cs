using HoangNV.HotelBooking.BusinessLogic.Interface;
using HoangNV.HotelBooking.BusinessLogic.Models;
using HoangNV.HotelBooking.Entities.Entities;
using HoangNV.HotelBooking.Repository;
using HoangNV.HotelBooking.Repository.Models;
using HoangNV.HotelBooking.Web.Areas.Admin.Controllers;
using HoangNV.HotelBooking.Web.Helper;
using HoangNV.HotelBooking.Web.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Web.Controllers 
{
    public class BookingController : BaseController
    {
        private readonly BookingContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IHotelBranchBS _hotelBranchBS;
        private readonly IConvenientTypeBS _convenientTypeBS;
        private readonly IConvenientBS _convenientBS;
        private readonly IBookingBS _bookingBS;
        private readonly IBedBS _bedBS;
        private readonly IRoomTypeBS _roomTypeBS;
        private readonly ICustomerBS _customerTypeBS;
        private readonly IThrottlingEmail _emailBS;
        private readonly IViewRenderService _viewRenderService;

        public BookingController(BookingContext context,
            IStringLocalizer<SharedResource> localizer, IHotelBranchBS hotelBranchBS, IConvenientTypeBS convenientTypeBS, IBookingBS bookingBS
            , IThrottlingEmail emailBS, IViewRenderService viewRenderService, IRoomTypeBS roomTypeBS, IBedBS bedBS, IConvenientBS convenientBS, ICustomerBS customerTypeBS)
        {
            _context = context;
            _localizer = localizer;
            _hotelBranchBS = hotelBranchBS;
            _convenientTypeBS = convenientTypeBS;
            _convenientBS = convenientBS;
            _bookingBS = bookingBS;
            _bedBS = bedBS;
            _roomTypeBS = roomTypeBS;
            _customerTypeBS = customerTypeBS;
            _emailBS = emailBS;
            _viewRenderService = viewRenderService;
        }
        public async Task<IActionResult> Index(string startDate, string endDate, int numOfPer)
        {
            var hotelBranchViewModel = await _hotelBranchBS.GetFristOrDefault();
            if (hotelBranchViewModel == null)
                return NotFound();
            ViewData["ConvenientTypes"] = await GetConvenientTypes();
            ViewData["Convenients"] = await GetConvenients();

            ViewData["HotelBranch"] = hotelBranchViewModel;
            var bookingsearch = new List<RoomBookClientViewModel>();
            var modelSearch = new BookingSearchModel()
            {
                CheckInTime = !string.IsNullOrEmpty(startDate)? DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture): DateTime.Today,
                CheckOutTime = !string.IsNullOrEmpty(endDate) ? DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) :DateTime.Today,
                NumOfPer = numOfPer,
            };
            if (endDate == null && modelSearch.CheckInTime > DateTime.Today)
                modelSearch.CheckOutTime = modelSearch.CheckInTime;
            if (startDate == null && modelSearch.CheckOutTime < DateTime.Today)
                modelSearch.CheckInTime = modelSearch.CheckOutTime;
            bookingsearch =( await _bookingBS.SearchClient(modelSearch)).ToList();
            var listBooking = new List<RoomBookClientViewModel>();
            foreach (var item in bookingsearch)
            {
                var a = listBooking.FirstOrDefault(x => x.RoomTypeId == item.RoomTypeId);
                if (a == null)
                    listBooking.Add(item);
            }
            ViewBag.StartDate = modelSearch.CheckInTime;
            ViewBag.EndDate = modelSearch.CheckOutTime;
            ViewBag.NumOfPer = modelSearch.NumOfPer;
            var countRoomType = bookingsearch.GroupBy(x => x.RoomTypeId).ToList();
            ViewBag.Count = countRoomType.Count;
            ViewData["RoomType"] = await _roomTypeBS.SearchClient(new RoomTypeSearchModel());
            return View(listBooking);
        }
        private async Task<IEnumerable<ConvenientTypes>> GetConvenientTypes()
        {
            var convenientTypes = (await _convenientTypeBS.SearchWithConvenient(string.Empty)).Where(x => x.Convenients.Count() > 0).OrderByDescending(x => x.Convenients.Count).ToList();
            return convenientTypes;
        }
        private async Task<IEnumerable<ConvenientQueryModel>> GetConvenients()
        {
            var convenients = (await _convenientBS.Search(new ConvenientSearchModel() { ConvenientName = String.Empty, ConvenientTypeId = 0 })).ToList();
            return convenients;
        }
        private async Task<IEnumerable<BedQueryModel>> GetBeds()
        {
            var beds = (await _bedBS.Search(string.Empty)).ToList();
            return beds;
        }

        [HttpGet]
        public async Task<IActionResult> DetailBooking(BookingDetailViewModel bookingDetailViewModel)
        {
            var userName = HttpContext.Session.GetString("user_name_client");
            if (userName != null)
            {
               var customer = await _customerTypeBS.GetCustomerByUserName(userName);
                if (customer != null)
                {
                    ViewData["Customer"] = customer;
                }
            }

            var hotelBranchViewModel = await _hotelBranchBS.GetFristOrDefault();
            if (hotelBranchViewModel == null)
                return NotFound();

            ViewData["HotelBranch"] = hotelBranchViewModel;
            bookingDetailViewModel.CostSum = 0;
            for (int i = 0; i < bookingDetailViewModel.CostTotal.Count; i++)
            {
                if(bookingDetailViewModel.NumOfRoom[i]==0)
                {
                    bookingDetailViewModel.RoomTypeId.RemoveAt(i);
                    bookingDetailViewModel.NumOfRoom.RemoveAt(i);
                    bookingDetailViewModel.CostTotal.RemoveAt(i);
                    i--;
                }    
                else bookingDetailViewModel.CostSum += bookingDetailViewModel.CostTotal[i] * bookingDetailViewModel.NumOfRoom[i]* ((int)(bookingDetailViewModel.CheckOutTime - bookingDetailViewModel.CheckInTime).TotalDays +1);
            }
            if (bookingDetailViewModel.CostSum==0)
            {
                return RedirectToAction("Index", "Booking");
            }
            ViewData["RoomType"] = await _roomTypeBS.SearchClient(new RoomTypeSearchModel());
            return View(bookingDetailViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Book(BookingDetailViewModel bookingDetailViewModel)
        {
            var hotelBranchViewModel = await _hotelBranchBS.GetFristOrDefault();
            if (hotelBranchViewModel == null)
                return NotFound();
            var userName = HttpContext.Session.GetString("user_name_client");
            ViewData["HotelBranch"] = hotelBranchViewModel;
            ViewData["RoomType"] = await _roomTypeBS.SearchClient(new RoomTypeSearchModel());
            if (ModelState.IsValid)
            {
                var checkInsert = await _bookingBS.InsertClient(bookingDetailViewModel,userName);
                if (checkInsert!="false")
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
                    return RedirectToAction("Success", "Booking");
                }
                else
                {
                    ViewBag.Error = "Lỗi hệ thống!";
                    return View(bookingDetailViewModel);
                }
            }
            else return View(bookingDetailViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Success()
        {
            var hotelBranchViewModel = await _hotelBranchBS.GetFristOrDefault();
            if (hotelBranchViewModel == null)
                return NotFound();

            ViewData["HotelBranch"] = hotelBranchViewModel;
            return View();
        }
    }
}
