using HoangNV.HotelBooking.BusinessLogic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Web.Helper
{

    public static class CsvUtil
        {
           
            public static async Task<MemoryStream> DownloadCsvUTF8WithBomHNV(IEnumerable<BookingConfirmCSVViewModel> csvModels)
            {
                var strBuilder = new StringBuilder();
                strBuilder.AppendLine("Mã đơn đặt,Tên người đặt,Số điện thoại,Ngày nhận phòng,Ngày trả phòng,Tổng tiền");
                foreach (var csvModel in csvModels)
                {
                    strBuilder.AppendLine($"{csvModel.BookingId},{csvModel.CustomerName},{csvModel.PhoneNumber},{csvModel.CheckInTime},{csvModel.CheckOutTime},{csvModel.SumCost}");
                }
                var stream = new MemoryStream();
                Encoding encoding = new UTF8Encoding(true);
                var writer = new StreamWriter(stream, encoding);
                await writer.WriteLineAsync(strBuilder);
                await writer.FlushAsync();
                stream.Seek(0, SeekOrigin.Begin);
                return stream;
            }
        }
    
}
