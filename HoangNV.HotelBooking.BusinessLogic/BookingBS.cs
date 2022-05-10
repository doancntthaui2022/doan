using HoangNV.HotelBooking.BusinessLogic.Interface;
using HoangNV.HotelBooking.BusinessLogic.Models;
using HoangNV.HotelBooking.Core.Services;
using HoangNV.HotelBooking.Entities.Entities;
using HoangNV.HotelBooking.Repository.Enum;
using HoangNV.HotelBooking.Repository.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.BusinessLogic
{
    public class BookingBS : BaseBS<Booking>, IBookingBS
    {
        public IBookingRepository _repository;
        public IRoomRepository _roomRepository;
        public IRoomTypeRepository _roomTypeRepository;
        public ICustomerRepository _customerRepository;
        public IBookingDetailRepository _bookingDetailRepository;
        public IImageRepository _imageRepository;
        public ICustomerBS _customerTypeBS;
        public IUserRepository _userRepository;
        public BookingBS(IBookingRepository repository, IRoomRepository roomRepository, IImageRepository imageRepository,
            IRoomTypeRepository roomTypeRepository, ICustomerRepository customerRepository, IBookingDetailRepository bookingDetailRepository
            , ICustomerBS customerTypeBS, IUserRepository userRepository) : base(repository)
        {
            _repository = repository;
            _roomRepository = roomRepository;
            _roomTypeRepository = roomTypeRepository;
            _customerRepository = customerRepository;
            _bookingDetailRepository = bookingDetailRepository;
            _imageRepository = imageRepository;
            _customerTypeBS = customerTypeBS;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<RoomBookViewModel>> Search(BookingSearchModel searchModel)
        {
            var rooms = await _roomRepository.SearchInclue(string.Empty,(int)searchModel.RoomTypeId, (int)RoomEnum.Free );
            var roomResult = rooms;
            if (searchModel.Area != null)
                roomResult = roomResult.Where(x => x.RoomType.Area >= searchModel.Area);
            if (searchModel.SumCost != null)
                roomResult = roomResult.Where(x => x.RoomType.Cost <= searchModel.SumCost);
            if (searchModel.NumOfPer != null)
                roomResult = roomResult.Where(x => x.RoomType.NumOfPer >= searchModel.NumOfPer);

            var bookings = await _repository.Search();
            var result = new List<Rooms>();
            foreach (var item in roomResult)
            {
                if (!bookings.Any(x => x.BookingDetails.FirstOrDefault(y=>y.RoomId== item.RoomId)!=null && ((x.CheckInTime>=searchModel.CheckInTime && x.CheckInTime<=searchModel.CheckOutTime)
                    || (x.CheckOutTime>=searchModel.CheckInTime && x.CheckOutTime<=searchModel.CheckOutTime) ||(searchModel.CheckInTime>=x.CheckInTime && x.CheckOutTime>=searchModel.CheckOutTime)
                ) && (x.BookingStatus == (int)StatusBookingRoomEnum.Booked || x.BookingStatus == (int)StatusBookingRoomEnum.Booking|| x.BookingStatus == (int)StatusBookingRoomEnum.Book)))
                {
                    result.Add(item);
                }
                    
            }
            return result.Select(x => new RoomBookViewModel()
            {
                RoomId=x.RoomId,
                RoomCode= x.RoomCode,
                Area= x.RoomType.Area,
                Cost=x.RoomType.Cost,
                NumOfPer= x.RoomType.NumOfPer,
                RoomTypeId=x.RoomTypeId,
            });
        }
        public async Task<IEnumerable<RoomBookClientViewModel>> SearchClient(BookingSearchModel searchModel)
        {
            var rooms = await _roomRepository.SearchInclue(string.Empty, (int)searchModel.RoomTypeId, (int)RoomEnum.Free);
            var roomResult = rooms;
            if (searchModel.Area != null)
                roomResult = roomResult.Where(x => x.RoomType.Area >= searchModel.Area);
            if (searchModel.SumCost != null)
                roomResult = roomResult.Where(x => x.RoomType.Cost <= searchModel.SumCost);
            if (searchModel.NumOfPer != null)
                roomResult = roomResult.Where(x => x.RoomType.NumOfPer >= searchModel.NumOfPer);

            var bookings = await _repository.Search();
            var result = new List<Rooms>();
            foreach (var item in roomResult)
            {
                if (!bookings.Any(x => x.BookingDetails.FirstOrDefault(y => y.RoomId == item.RoomId) != null && ((x.CheckInTime >= searchModel.CheckInTime && x.CheckInTime <= searchModel.CheckOutTime)
                    || (x.CheckOutTime >= searchModel.CheckInTime && x.CheckOutTime <= searchModel.CheckOutTime) || (searchModel.CheckInTime >= x.CheckInTime && x.CheckOutTime >= searchModel.CheckOutTime)
                ) && (x.BookingStatus == (int)StatusBookingRoomEnum.Booked || x.BookingStatus == (int)StatusBookingRoomEnum.Booking || x.BookingStatus == (int)StatusBookingRoomEnum.Book)))
                {
                    result.Add(item);
                }

            }
            
            var countRoomType = result.GroupBy(x => x.RoomTypeId).ToList();
            return result.Select(x => new RoomBookClientViewModel()
            {

                RoomId = x.RoomId,
                RoomCode = x.RoomCode,
                Area = x.RoomType.Area,
                Cost = x.RoomType.Cost,
                NumOfPer = x.RoomType.NumOfPer,
                RoomTypeId = x.RoomTypeId,
                NumOfRoom= countRoomType.FirstOrDefault(y => y.Key == x.RoomTypeId).Count(),
                RoomTypeName=x.RoomType.RoomTypeName,
                ImageLinks=(_imageRepository.GetImagesByRoomTypeId(x.RoomTypeId).Result).Select(y=>y.ImageLink).ToList(),
            });
        }
        public async Task<string> Insert(BookingDetailViewModel bookingDetailViewModel)
        {
            var search = await Search(new BookingSearchModel() { 
                CheckInTime=bookingDetailViewModel.CheckInTime,
                CheckOutTime=bookingDetailViewModel.CheckOutTime,
            });
            var customer = new Customers()
            {
                CustomerName = bookingDetailViewModel.CustomerName,
                Email = bookingDetailViewModel.Email,
                CheckInPersonCode = bookingDetailViewModel.CheckInPersonCode,
                CheckInPersonName = bookingDetailViewModel.CheckInPersonName,
                PhoneNumber = bookingDetailViewModel.PhoneNumber,
            };
            var boolCustomer =await _customerRepository.Insert(customer);
            if (!boolCustomer) return boolCustomer.ToString();
            var booking = new Booking()
            {
                BookingId= Guid.NewGuid().ToString(),
                CheckInTime = bookingDetailViewModel.CheckInTime,
                CheckOutTime = bookingDetailViewModel.CheckOutTime,
                SumCost = (decimal)bookingDetailViewModel.CostSum,
                BookingStatus = (int)StatusBookingRoomEnum.Booked,
                Customer= customer,
            };
            var boolBooking =await _repository.Insert(booking);
            if (!boolBooking) return boolBooking.ToString();
            for (int i = 0; i < bookingDetailViewModel.RoomTypeId.Count; i++)
            {
                var roomType = await _roomTypeRepository.GetByIdInclude(bookingDetailViewModel.RoomTypeId[i]);
                var numOfRoom = bookingDetailViewModel.NumOfRoom[i];
                var rooms2 = search.Where(x => x.RoomTypeId == bookingDetailViewModel.RoomTypeId[i]).ToList();
                for (int j = 0; j < numOfRoom; j++)
                {
                    var bookingDetail = new BookingDetails()
                    {
                        BookingId = booking.BookingId,
                        BookingDetailId = Guid.NewGuid().ToString(),
                        RoomId = rooms2[j].RoomId,
                        CostNow = roomType.Cost,
                        Booking= booking,
                    };
                    var boolBookingDetail = await _bookingDetailRepository.Insert(bookingDetail);
                    if (!boolBookingDetail) return boolBooking.ToString();
                }
            }
            return booking.BookingId;
        }


        public async Task<IEnumerable<BookingConfirmViewModel>> SearchConfirm()
        {
            var booking=  await _repository.SearchConfirm();
            return booking.Select(x => new BookingConfirmViewModel() { 
                BookingId=x.BookingId,
                CheckInTime=x.CheckInTime,
                CheckOutTime=x.CheckOutTime,
                CustomerName=x.Customer.CustomerName,
                PhoneNumber=x.Customer.PhoneNumber,
            });
        }

        public async Task<BookingConfirmViewModel> GetByIdInclude(string id)
        {
            var result= await _repository.GetByIdInclude(id);
            var roomIds = result.BookingDetails.Select(x => x.RoomId).ToList();
            var roomsCode = new List<string>();
            foreach (var item in roomIds)
            {
                roomsCode.Add((await _roomRepository.GetByIdAsync(item)).RoomCode);
            }
            return new BookingConfirmViewModel()
            {
                BookingId = result.BookingId,
                RoomCode =roomsCode.OrderBy(x=>x.Trim()).ToList(),
                CheckInTime = result.CheckInTime,
                CheckOutTime = result.CheckOutTime,
                CheckInPersonCode = result.Customer.CheckInPersonCode,
                CheckInPersonName = result.Customer.CheckInPersonName,
                CustomerName = result.Customer.CustomerName,
                Email = result.Customer.Email,
                PhoneNumber = result.Customer.PhoneNumber,
                SumCost =(float)result.SumCost,
                StatusBooking=result.BookingStatus,
            };
        }

        public async Task<bool> CheckingBooking(string id)
        {
            var result = await _repository.GetByIdInclude(id);
            result.BookingStatus = (int)StatusBookingRoomEnum.Booked;
            return await _repository.UpdateStatusBooking(result);
        }

        public async Task<Booking> GetBookingWithCustomer(string id)
        {
            return await _repository.GetByIdInclude(id);
        }
        public async Task<IEnumerable<BookingConfirmViewModel>> SearchConfirmBooked()
        {
            var booking = await _repository.SearchConfirmBooked();
            return booking.Select(x => new BookingConfirmViewModel()
            {
                BookingId = x.BookingId,
                CheckInTime = x.CheckInTime,
                CheckOutTime = x.CheckOutTime,
                CustomerName = x.Customer.CustomerName,
                PhoneNumber = x.Customer.PhoneNumber,
            });
        }

        public async Task<bool> CheckingBooked(string id)
        {
            var result = await _repository.GetByIdInclude(id);
            result.BookingStatus = (int)StatusBookingRoomEnum.Book;
            return await _repository.UpdateStatusBooking(result);
        }

        public async Task<IEnumerable<BookingConfirmViewModel>> SearchConfirmChecked()
        {
            var booking = await _repository.SearchConfirmChecked();
            return booking.Select(x => new BookingConfirmViewModel()
            {
                BookingId = x.BookingId,
                CheckInTime = x.CheckInTime,
                CheckOutTime = x.CheckOutTime,
                CustomerName = x.Customer.CustomerName,
                PhoneNumber = x.Customer.PhoneNumber,
            });
        }

        public async Task<bool> ExitBooked(string id)
        {
            var result = await _repository.GetByIdInclude(id);
            result.BookingStatus = (int)StatusBookingRoomEnum.Exit;
            return await _repository.UpdateStatusBooking(result);
        }

        public async Task<IEnumerable<BookingConfirmViewModel>> SearchResult(BookingSearcResulthModel searchCondition)
        {
            var result = await _repository.SearchExit(searchCondition.CheckInTime, searchCondition.CheckOutTime, searchCondition.CustomerName,searchCondition.PhoneNumber);
            return result.Select(x => new BookingConfirmViewModel() {
                BookingId = x.BookingId,
                CheckInTime = x.CheckInTime,
                CheckOutTime = x.CheckOutTime,
                CustomerName = x.Customer.CustomerName,
                PhoneNumber = x.Customer.PhoneNumber,
            }).OrderByDescending(x=>x.CheckInTime).ToList();
        }

        public async Task<IEnumerable<BookingConfirmCSVViewModel>> SearchResultCSV(BookingSearcResulthModel searchCondition)
        {
            var result = await _repository.SearchExit(searchCondition.CheckInTime, searchCondition.CheckOutTime, searchCondition.CustomerName, searchCondition.PhoneNumber);
            return result.Select(x => new BookingConfirmCSVViewModel()
            {
                BookingId = x.BookingId,
                CustomerName = x.Customer.CustomerName,
                PhoneNumber = x.Customer.PhoneNumber,
                CheckInTime = x.CheckInTime.ToString("dd/MM/yyyy"),
                CheckOutTime = x.CheckOutTime.ToString("dd/MM/yyyy"),
                SumCost=x.SumCost.ToString(),
            }).OrderByDescending(x => x.CheckInTime).ToList();
        }

        public async Task<bool> Delete(string id)
        {
            var result = await _repository.GetByIdInclude(id);
            return await _repository.Delete(result);

        }

        public async Task<string> InsertClient(BookingDetailViewModel bookingDetailViewModel, string userName)
        {
            var search = await Search(new BookingSearchModel()
            {
                CheckInTime = bookingDetailViewModel.CheckInTime,
                CheckOutTime = bookingDetailViewModel.CheckOutTime,
            });
            var boolCustomer = true;
            Customers customerByUser = null;
            var user = await _userRepository.GetUserByName(userName);
            if (user==null)
            {
                customerByUser = new Customers()
                {
                    CustomerName = bookingDetailViewModel.CustomerName,
                    Email = bookingDetailViewModel.Email,
                    CheckInPersonCode = bookingDetailViewModel.CheckInPersonCode,
                    CheckInPersonName = bookingDetailViewModel.CheckInPersonName,
                    PhoneNumber = bookingDetailViewModel.PhoneNumber,
                };
                boolCustomer = await _customerRepository.Insert(customerByUser);
            }
            else
            {
                customerByUser = await _customerRepository.GetCustomerByUserName(userName);
                if (customerByUser != null)
                {
                    customerByUser.CustomerName = bookingDetailViewModel.CustomerName;
                    customerByUser.Email = bookingDetailViewModel.Email;
                    customerByUser.CheckInPersonCode = bookingDetailViewModel.CheckInPersonCode;
                    customerByUser.CheckInPersonName = bookingDetailViewModel.CheckInPersonName;
                    customerByUser.PhoneNumber = bookingDetailViewModel.PhoneNumber;
                    boolCustomer = await _customerRepository.Update(customerByUser);
                }
                else
                {
                    customerByUser = new Customers()
                    {
                        CustomerName = bookingDetailViewModel.CustomerName,
                        Email = bookingDetailViewModel.Email,
                        CheckInPersonCode = bookingDetailViewModel.CheckInPersonCode,
                        CheckInPersonName = bookingDetailViewModel.CheckInPersonName,
                        PhoneNumber = bookingDetailViewModel.PhoneNumber,
                    };
                    boolCustomer = await _customerRepository.Insert(customerByUser);
                }
                user.Customer = customerByUser;
                await _userRepository.Update(user);
            }
            
            if (!boolCustomer) return boolCustomer.ToString();
            var booking = new Booking()
            {
                BookingId = Guid.NewGuid().ToString(),
                CheckInTime = bookingDetailViewModel.CheckInTime,
                CheckOutTime = bookingDetailViewModel.CheckOutTime,
                SumCost = (decimal)bookingDetailViewModel.CostSum,
                BookingStatus = (int)StatusBookingRoomEnum.Booking,
                Customer = customerByUser,
            };
            var boolBooking = await _repository.Insert(booking);
            if (!boolBooking) return boolBooking.ToString();
            for (int i = 0; i < bookingDetailViewModel.RoomTypeId.Count; i++)
            {
                var roomType = await _roomTypeRepository.GetByIdInclude(bookingDetailViewModel.RoomTypeId[i]);
                var numOfRoom = bookingDetailViewModel.NumOfRoom[i];
                var rooms2 = search.Where(x => x.RoomTypeId == bookingDetailViewModel.RoomTypeId[i]).ToList();
                for (int j = 0; j < numOfRoom; j++)
                {
                    var bookingDetail = new BookingDetails()
                    {
                        BookingId = booking.BookingId,
                        BookingDetailId = Guid.NewGuid().ToString(),
                        RoomId = rooms2[j].RoomId,
                        CostNow = roomType.Cost,
                        Booking = booking,
                    };
                    var boolBookingDetail = await _bookingDetailRepository.Insert(bookingDetail);
                    if (!boolBookingDetail) return boolBooking.ToString();
                }
            }
            return booking.BookingId;
        }
        public async Task<IEnumerable<BookingViewModel>> SearchAccount(BookingSearchModelAccount searchModel, string userName)
        {
            IEnumerable<BookingViewModel> customers = new List<BookingViewModel>();
            var customer = await _customerRepository.GetCustomerByUserName(userName);
            if (customer == null) return customers;
            return customer.Bookings.Where(x=>x.CheckInTime>=searchModel.CheckInTime && x.CheckOutTime<=searchModel.CheckOutTime).Select(x => new BookingViewModel()
            {
               BookingId= x.BookingId,
               BookingStatus=x.BookingStatus,
               CheckInTime=x.CheckInTime,
               CheckOutTime=x.CheckOutTime,
               SumCost=x.SumCost,
            });
        }

        public async Task<BookingConfirmClientViewModel> GetByIdIncludeClient(string id)
        {
            var result = await _repository.GetByIdInclude(id);
            var roomIds = result.BookingDetails.Select(x => x.RoomId).ToList();
            var roomsCode = new List<string>();
            var rooms = new List<Rooms>();
            var numOfOrder = new List<int>();
            var Cost = new List<decimal>();
            var roomTypeName = new List<string>();
            foreach (var item in roomIds)
            {
                rooms.Add(await _roomRepository.GetByIdAsync(item));
                roomsCode.Add((await _roomRepository.GetByIdAsync(item)).RoomCode);
            }
            var roomsType = rooms.GroupBy(x => x.RoomTypeId).ToList();
            foreach (var item in roomsType)
            {
                numOfOrder.Add(item.Count());
                Cost.Add(item.FirstOrDefault().RoomType.Cost);
                roomTypeName.Add(item.FirstOrDefault().RoomType.RoomTypeName);
            }
            return new BookingConfirmClientViewModel()
            {
                BookingId = result.BookingId,
                RoomCode = roomsCode.OrderBy(x => x.Trim()).ToList(),
                CheckInTime = result.CheckInTime,
                CheckOutTime = result.CheckOutTime,
                CheckInPersonCode = result.Customer.CheckInPersonCode,
                CheckInPersonName = result.Customer.CheckInPersonName,
                CustomerName = result.Customer.CustomerName,
                Email = result.Customer.Email,
                PhoneNumber = result.Customer.PhoneNumber,
                SumCost = (float)result.SumCost,
                StatusBooking = result.BookingStatus,
                NumOfOrder=numOfOrder,
                Cost=Cost,
                RoomTypeName=roomTypeName
                
            };
        }
        
        public async Task<List<decimal>> StatisticsMonthByYear(int Year)
        {
            List<decimal> sumCost = new List<decimal>();
            for (int i = 1; i <=12; i++)
            {
                sumCost.Add(await _repository.StatisticsMonthByYear(i, Year));
            }
            return sumCost;
        }

        public async Task<List<CostRoomType>> GetCostRoomTypes(int year)
        {
            var listRoomType =(await _roomTypeRepository.Search(string.Empty,string.Empty)).OrderBy(x=>x.RoomTypeCode).ToList();
            List<CostRoomType> listCost = new List<CostRoomType>();
            foreach (var item in listRoomType)
            {
                listCost.Add(new CostRoomType()
                {
                    RoomTypeCode = item.RoomTypeCode,
                    Cost=await _repository.GetCostRoomTypes(item.RoomTypeId,year)
                });
            }
            return listCost;
        }
        public async Task<List<CostRoomType>> GetCostRoomTypesByMonth(int month, int year)
        {
            var listRoomType = (await _roomTypeRepository.Search(string.Empty, string.Empty)).OrderBy(x => x.RoomTypeCode).ToList();
            List<CostRoomType> listCost = new List<CostRoomType>();
            foreach (var item in listRoomType)
            {
                listCost.Add(new CostRoomType()
                {
                    RoomTypeCode = item.RoomTypeCode,
                    Cost = await _repository.GetCostRoomTypesByMonth(item.RoomTypeId,month, year)
                });
            }
            return listCost;
        }
    }
}
