using HoangNV.HotelBooking.BusinessLogic.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.BusinessLogic.Struct
{
    public struct ResponseJson
    {
        public ResponseStatus Status { get; set; }
        public ResponseMessage Message { get; set; }
        public object Data { get; set; }
        public string Url { get; set; }

        public ResponseJson Ok(ResponseMessage message = new ResponseMessage(), object data = null, string url = null)
        {
            return new ResponseJson()
            {
                Status = ResponseStatus.Ok,
                Message = message,
                Data = data,
                Url = url
            };
        }

        public ResponseJson Error(ResponseMessage message = new ResponseMessage(), object data = null, string url = null)
        {
            return new ResponseJson()
            {
                Status = ResponseStatus.Error,
                Message = message,
                Data = data,
                Url = url
            };
        }
    }
}
