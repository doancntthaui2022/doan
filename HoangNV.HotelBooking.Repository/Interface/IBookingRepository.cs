using HoangNV.HotelBooking.Core.Repository.Interface;
using HoangNV.HotelBooking.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Repository.Interface
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<IEnumerable<Booking>> Search();

        Task<bool> Insert(Booking booking);

        Task<IEnumerable<Booking>> SearchConfirm();
        Task<IEnumerable<Booking>> SearchConfirmBooked();
        Task<IEnumerable<Booking>> SearchConfirmChecked();

        Task<IEnumerable<Booking>> SearchExit(DateTime checkIn, DateTime checkOut, string CustomerName, string PhoneNumber);

        Task<Booking> GetByIdInclude(string id);

        Task<bool> UpdateStatusBooking(Booking booking);

        Task<bool> Delete(Booking bookingDetails);

        Task<decimal> StatisticsMonthByYear(int Month, int Year);
        Task<decimal> GetCostRoomTypes(int roomTypeId, int Year);
        Task<decimal> GetCostRoomTypesByMonth(int roomTypeId, int Month, int Year);
    }
}
