using HoangNV.HotelBooking.BusinessLogic.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.BusinessLogic.Struct
{
    public struct ResponseMessage
    {
        public MessageDisplayType DisplayType { get; set; }
        public MessageType MessageType { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }

        public ResponseMessage CreateMessageBox(string message, MessageType messageType)
        {
            return new ResponseMessage
            {
                DisplayType = MessageDisplayType.MessageBox,
                MessageType = messageType,
                Message = message
            };
        }

        public ResponseMessage CreateConfirmBox(string message, MessageType messageType)
        {
            return new ResponseMessage
            {
                DisplayType = MessageDisplayType.ConfirmBox,
                MessageType = messageType,
                Message = message
            };
        }
    }
}
