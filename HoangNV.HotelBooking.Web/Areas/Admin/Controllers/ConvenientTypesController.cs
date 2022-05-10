using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HoangNV.HotelBooking.Web.Areas.Admin.Models;
using HoangNV.HotelBooking.Web.Utils;
using HoangNV.HotelBooking.Repository;
using HoangNV.HotelBooking.BusinessLogic.Interface;
using Newtonsoft.Json;
using HoangNV.HotelBooking.BusinessLogic.Struct;
using HoangNV.HotelBooking.BusinessLogic.Enum;
using HoangNV.HotelBooking.Web.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Http;

namespace HoangNV.HotelBooking.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ConvenientTypesController : BaseController
    {
        private readonly BookingContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IConvenientTypeBS _convenientTypeBS;

        public ConvenientTypesController(BookingContext context, IConvenientTypeBS convenientTypeBS,
            IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _convenientTypeBS = convenientTypeBS;
            _localizer = localizer;
        }

        // GET: Admin/ConvenientTypes
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
                ? string.Empty
                : JsonConvert.DeserializeObject<string>(model.search.value);
            var page = Utility.NormalizePage(model.start, model.length);
            var size = Utility.NormalizeSize(model.length);
            var data = await _convenientTypeBS.Search(searchCondition);
            var totalRecord = data.ToList().Count;
            data = data.Skip(size * (page - 1)).Take(size).ToList();
            var ct = new PagingModel(model.page, totalRecord, data);
            return Json(ct);
        }

        [HttpPost]
        public async Task<IActionResult> InsertConvenientType(string convenientTypeName)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");
            var checkData = await _convenientTypeBS.SearchByName(convenientTypeName);
            if (checkData!=null)
            {
                var messageError = SetJsonMsgBox(_localizer["E_C_002"].Value, MessageType.Error);
                return Json(new ResponseJson().Error(message: messageError));
            }
            var checkInsert = await _convenientTypeBS.Insert(convenientTypeName);
            if(checkInsert)
            {
                var messageSuccess = SetJsonMsgBox(_localizer["S_C_001"].Value, MessageType.Success);
                return Json(new ResponseJson().Ok(message: messageSuccess));

            }
            else
            {
                var messageError = SetJsonMsgBox(_localizer["E_C_001"].Value, MessageType.Error);
                return Json(new ResponseJson().Error(message: messageError));
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateConvenientType(int id, string newName)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");

            var checkData = await _convenientTypeBS.SearchByName(newName);
            if (checkData != null)
            {
                var messageError = SetJsonMsgBox(_localizer["E_C_002"].Value, MessageType.Error);
                return Json(new ResponseJson().Error(message: messageError));
            }
            var convenientType = await _context.ConvenientTypes.FindAsync(id);
            if (convenientType == null) return NotFound();
            else
            {
                var checkDelete = await _convenientTypeBS.Update(id, newName);
                if (checkDelete)
                {
                    var messageSuccess = SetJsonMsgBox(_localizer["S_C_003"].Value, MessageType.Success);
                    return Json(new ResponseJson().Ok(message: messageSuccess));
                }
                var message = SetJsonMsgBox(_localizer["E_C_004"].Value, MessageType.Success);
                return Json(new ResponseJson().Ok(message: message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConvenientType(int id)
        {
            var userName = HttpContext.Session.GetString("user_name");
            var userRole = HttpContext.Session.GetString("user_role");
            if (userName == null)
                return RedirectToAction("Index", "Login");
            if (userRole != "Employee")
                return RedirectToAction("Index", "Error");

            var convenientType = await _context.ConvenientTypes.FindAsync(id);
            if (convenientType == null) return NotFound();
            else
            {
                var checkDelete = await _convenientTypeBS.Delete(id);
                if(checkDelete)
                {
                    var messageSuccess = SetJsonMsgBox(_localizer["S_C_002"].Value, MessageType.Success);
                    return Json(new ResponseJson().Ok(message: messageSuccess));
                }
                var message = SetJsonMsgBox(_localizer["E_C_002"].Value, MessageType.Error);
                return Json(new ResponseJson().Ok(message: message));
            }
        }
    }
}
