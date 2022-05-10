using HoangNV.HotelBooking.BusinessLogic.Interface;
using HoangNV.HotelBooking.BusinessLogic.Models;
using HoangNV.HotelBooking.Core.Services;
using HoangNV.HotelBooking.Entities.Entities;
using HoangNV.HotelBooking.Repository.Enum;
using HoangNV.HotelBooking.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.BusinessLogic
{
    public class RoomBS : BaseBS<Rooms>, IRoomBS
    {
        public IRoomRepository _repository;
        public IBookingDetailRepository _bookingDetailRepository;
        public IBookingRepository _bookingRepository;
        public RoomBS(IRoomRepository repository, IBookingDetailRepository bookingDetailRepository,
            IBookingRepository bookingRepository) : base(repository)
        {
            _repository = repository;
            _bookingDetailRepository = bookingDetailRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<IEnumerable<RoomViewModel>> Search(RoomViewModel searchCondition)
        {
            var room = await _repository.Search(searchCondition.RoomCode, searchCondition.RoomTypeId, searchCondition.Status);
            return room.Select(x => new RoomViewModel()
            {
                RoomCode=x.RoomCode,
                RoomId=x.RoomId,
                RoomTypeId=x.RoomTypeId,
                Status=x.Status,
            }).OrderBy(x=>x.Status).ToList();

        }

        public async Task<string> UpdateStatus(int id, int status)
        {
            var room = await _repository.GetByIdAsync(id);
            var bookingDetails = await _bookingDetailRepository.GetBookingDetailsByRoomId(id);
            if(bookingDetails.Count()!=0)
            {
                foreach (var item in bookingDetails)
                {
                    if (item.Booking.BookingStatus == (int)StatusBookingRoomEnum.Booked ||
                        item.Booking.BookingStatus == (int)StatusBookingRoomEnum.Booking ||
                        item.Booking.BookingStatus == (int)StatusBookingRoomEnum.Book) return "false";
                }
            }    
            if (room == null) return null;
            if (room.Status == (int)RoomEnum.Free || room.Status == (int)RoomEnum.Nonactive)
            {
                room.Status = status;
                await _repository.Update(room);
                return "true";
            }    
            return "false";
        }

        public async Task<bool> Delete(int id)
        {
            var room = await _repository.GetByIdAsync(id);
            if(room.Status == (int)RoomEnum.Nonactive)
                return await _repository.Delete(room);
            return false;
        }

        public async Task<int> GetRoomNameMax(int roomTypeId)
        {
            var listRoomDel = (await _repository.GetRoomsByRoomTypeId(roomTypeId)).ToList();
            Rooms roomDel = listRoomDel.ToList().FirstOrDefault();
            int demRoom = int.Parse(roomDel.RoomCode.Substring(roomDel.RoomCode.LastIndexOf('R') + 1));
            return demRoom;
        }
        public async Task<bool> Insert(RoomAddViewModel roomAddViewModel)
        {
            var room = new Rooms()
            {
                RoomCode = roomAddViewModel.RoomCode,
                RoomTypeId = roomAddViewModel.RoomTypeId,
                Status = (int)RoomEnum.Free,
            };
           return await _repository.Insert(room);
        }
    }
}
