using HoangNV.HotelBooking.BusinessLogic.Interface;
using HoangNV.HotelBooking.BusinessLogic.Models;
using HoangNV.HotelBooking.Entities.Entities;
using HoangNV.HotelBooking.Repository;
using HoangNV.HotelBooking.Repository.Models;
using HoangNV.HotelBooking.Web.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookingContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IHotelBranchBS _hotelBranchBS;
        private readonly IConvenientTypeBS _convenientTypeBS;
        private readonly IConvenientBS _convenientBS;

        public HomeController(BookingContext context,
            IStringLocalizer<SharedResource> localizer, IHotelBranchBS hotelBranchBS ,IConvenientTypeBS convenientTypeBS,
            IConvenientBS convenientBS)
        {
            _context = context;
            _localizer = localizer;
            _hotelBranchBS = hotelBranchBS;
            _convenientTypeBS = convenientTypeBS;
            _convenientBS = convenientBS;
        }
        // GET: HomeController
        public async Task<ActionResult> Index()
        {
            var hotelBranchViewModel = await _hotelBranchBS.GetFristOrDefault();
            if (hotelBranchViewModel == null)
                return NotFound();
            ViewData["ConvenientTypes"] = await GetConvenientTypes();
            ViewData["Convenients"] = await GetConvenients();
            
            ViewData["HotelBranch"] = hotelBranchViewModel;
            return View();
        }

        private async Task<IEnumerable<ConvenientTypes>> GetConvenientTypes()
        {
            var convenientTypes = (await _convenientTypeBS.SearchWithConvenient(string.Empty)).Where(x => x.Convenients.Count() > 0).OrderByDescending(x=>x.Convenients.Count).ToList();
            return convenientTypes;
        }
        private async Task<IEnumerable<ConvenientQueryModel>> GetConvenients()
        {
            var convenients = (await _convenientBS.Search(new ConvenientSearchModel() { ConvenientName = String.Empty, ConvenientTypeId = 0 })).ToList();
            return convenients;
        }
    }
}
