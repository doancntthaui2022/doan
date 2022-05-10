using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.BusinessLogic.Models
{


    public class BookingSearchModel
    {
        public DateTime? CheckInTime { get; set; } = DateTime.Today;
        public DateTime? CheckOutTime { get; set; } = DateTime.Today;
        public decimal? SumCost { get; set; }
        public int? NumOfPer { get; set; }
        public int? RoomTypeId { get; set; } = 0;
        public float? Area { get; set; }
    }
    public class BookingViewModel
    {
        public string BookingId { get; set; }
        public int BookingStatus { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public decimal SumCost { get; set; }
        public int CustomerId { get; set; }
        public int RoomId { get; set; }
    }

    public class RoomBookViewModel
    {
        public int RoomId { get; set; }
        public string RoomCode { get; set; }
        public int RoomTypeId { get; set; }
        public float? Area { get; set; }
        public decimal? Cost { get; set; }
        public int? NumOfPer { get; set; }
    }


    public class RoomBookClientViewModel
    {
        public int RoomId { get; set; }
        public string RoomCode { get; set; }
        public string RoomTypeName { get; set; }
        public int RoomTypeId { get; set; }
        public float? Area { get; set; }
        public decimal? Cost { get; set; }
        public int? NumOfPer { get; set; }

        public List<string> BedNumber { get; set; } = new List<string>();
        public int? NumOfRoom { get; set; }
        public List<string> ImageLinks { get; set; } = new List<string>();

    }
    public class BookingConfirmViewModel
    {
        public string BookingId { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public List<string> RoomCode { get; set; }
        public float SumCost { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CheckInPersonCode { get; set; }
        public string CheckInPersonName { get; set; }

        public int StatusBooking { get; set; }
    }
    public class BookingSearcResulthModel
    {
        public DateTime CheckInTime { get; set; } = DateTime.MinValue;
        public DateTime CheckOutTime { get; set; } = DateTime.MaxValue;
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class BookingSearchModelAccount
    {
        public DateTime? CheckInTime { get; set; } = DateTime.MinValue;
        public DateTime? CheckOutTime { get; set; } = DateTime.MaxValue;
    }

    public class BookingViewModelClientDetails
    {
        public string BookingId { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public List<string> RoomCode { get; set; }
        public float SumCost { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CheckInPersonCode { get; set; }
        public string CheckInPersonName { get; set; }

        public int StatusBooking { get; set; }
    }

    public class BookingConfirmClientViewModel
    {
        public string BookingId { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public List<string> RoomCode { get; set; }
        public List<string> RoomTypeName { get; set; }
        public List<decimal> Cost { get; set; }
        public List<int> NumOfOrder { get; set; }
        public float SumCost { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CheckInPersonCode { get; set; }
        public string CheckInPersonName { get; set; }

        public int StatusBooking { get; set; }
    }

    public class CostRoomType
    {
        public string RoomTypeCode { get; set; }
        public decimal Cost { get; set; }
    }

    public class BookingConfirmCSVViewModel
    {
        public string BookingId { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public string SumCost { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
