using HoangNV.HotelBooking.BusinessLogic.Models;
using HoangNV.HotelBooking.Core.Services;
using HoangNV.HotelBooking.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.BusinessLogic.Interface
{
    public interface IBookingBS : IBaseBS<Booking>
    {
        Task<IEnumerable<RoomBookViewModel>> Search(BookingSearchModel searchModel);
        Task<IEnumerable<RoomBookClientViewModel>> SearchClient(BookingSearchModel searchModel);
        Task<IEnumerable<BookingViewModel>> SearchAccount(BookingSearchModelAccount searchModel, string userName);
        Task<IEnumerable<BookingConfirmViewModel>> SearchConfirm();
        Task<IEnumerable<BookingConfirmViewModel>> SearchConfirmBooked();
        Task<IEnumerable<BookingConfirmViewModel>> SearchConfirmChecked();
        Task<string> Insert(BookingDetailViewModel bookingDetailViewModel);
        Task<string> InsertClient(BookingDetailViewModel bookingDetailViewModel,string userName);
        Task<BookingConfirmViewModel> GetByIdInclude(string id);
        Task<BookingConfirmClientViewModel> GetByIdIncludeClient(string id);
        Task<Booking> GetBookingWithCustomer(string id);
        Task<bool> CheckingBooking(string id);
        Task<bool> CheckingBooked(string id);
        Task<bool> ExitBooked(string id);
        Task<IEnumerable<BookingConfirmViewModel>> SearchResult(BookingSearcResulthModel searchCondition);
        Task<IEnumerable<BookingConfirmCSVViewModel>> SearchResultCSV(BookingSearcResulthModel searchCondition);
        Task<bool> Delete(string id);

        Task<List<decimal>> StatisticsMonthByYear(int Year);

        Task<List<CostRoomType>> GetCostRoomTypes(int year);
        Task<List<CostRoomType>> GetCostRoomTypesByMonth(int month,int year);

    }
}
