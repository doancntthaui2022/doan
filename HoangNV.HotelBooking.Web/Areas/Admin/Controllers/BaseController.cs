using HoangNV.HotelBooking.BusinessLogic.Enum;
using HoangNV.HotelBooking.BusinessLogic.Struct;
using Microsoft.AspNetCore.Mvc;

namespace HoangNV.HotelBooking.Web.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {

        }
        protected void SetMessageBox(string message, MessageType messageType)
        {
            ViewBag.ResponseInfo = SetJsonMsgBox(message, messageType);
        }

        protected void SetConfirmBox(string message, MessageType messageType)
        {
            ViewBag.ResponseInfo = SetJsonConfirmBox(message, messageType);
        }

        protected ResponseMessage SetJsonMsgBox(string message, MessageType messageType)
        {
            return new ResponseMessage().CreateMessageBox(message, messageType);
        }

        protected ResponseMessage SetJsonConfirmBox(string message, MessageType messageType)
        {
            return new ResponseMessage().CreateConfirmBox(message, messageType);
        }
    }
}
