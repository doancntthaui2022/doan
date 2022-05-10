using HoangNV.HotelBooking.BusinessLogic.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : BaseController
    {
        private readonly IBookingBS _bookingBS;
        public HomeController(IBookingBS bookingBS)
        {
            _bookingBS = bookingBS;
        }
        // GET: HomeController
        public async Task<IActionResult> Index()
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName==null)
                return RedirectToAction("Index", "Login");
            ViewData["CostRoomType"] = await _bookingBS.GetCostRoomTypes(DateTime.Now.Year);
            ViewData["CostRoomTypeByMonth"] = await _bookingBS.GetCostRoomTypesByMonth(DateTime.Now.Month,DateTime.Now.Year);
            ViewBag.Costs = await _bookingBS.StatisticsMonthByYear(DateTime.Now.Year);
            return View();
        }
        public async Task<IActionResult> GetCostYear(int year)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            ViewBag.Year = year;
            ViewData["CostRoomType"] = await _bookingBS.GetCostRoomTypes(year);
            var cost = await _bookingBS.StatisticsMonthByYear(year);
            return View(cost);
        }

        public async Task<IActionResult> GetCostMonth(int month, int year)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            
            if (userName == null)
                return RedirectToAction("Index", "Login");
            ViewBag.Year = month + "/" + year;
            var cost = await _bookingBS.GetCostRoomTypesByMonth(month,year);
            return View(cost);
        }
    }
}
