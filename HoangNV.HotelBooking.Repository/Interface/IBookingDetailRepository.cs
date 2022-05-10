using HoangNV.HotelBooking.Core.Repository.Interface;
using HoangNV.HotelBooking.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Repository.Interface
{
    public interface IBookingDetailRepository : IRepository<BookingDetails>
    {
        Task<bool> Insert(BookingDetails bookingDetails);

        Task<IEnumerable<BookingDetails>> GetBookingDetailsByRoomId(int id);
    }
}
