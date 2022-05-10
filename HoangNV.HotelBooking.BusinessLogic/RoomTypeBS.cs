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
    public class RoomTypeBS : BaseBS<RoomTypes>, IRoomTypeBS
    {
        public IRoomTypeRepository _repository;
        public IImageRepository _imageRepository;
        public IRoomRepository _roomRepository;
        public IRoomBedRepository _roomBedRepository;
        public IHotelBranchRepository _hotelBrachRepository;
        public IRoomConvenientRepository _roomCovenientRepository;
        public RoomTypeBS(IRoomTypeRepository repository, IImageRepository imageRepository, IRoomRepository roomRepository,
            IRoomBedRepository roomBedRepository, IHotelBranchRepository hotelBranchRepository
            ,IRoomConvenientRepository roomConvenientRepository) : base(repository)
        {
            _repository = repository;
            _imageRepository = imageRepository;
            _roomRepository = roomRepository;
            _hotelBrachRepository = hotelBranchRepository;
            _roomBedRepository = roomBedRepository;
            _roomCovenientRepository = roomConvenientRepository;
        }

        public async Task<IEnumerable<RoomTypeViewModel>> Search(RoomTypeSearchModel roomTypeViewModel)
        {
            var query = await _repository.Search(roomTypeViewModel.RoomTypeCode,roomTypeViewModel.RoomTypeName);
            return query.Select(x => new RoomTypeViewModel()
            {
                Area=x.Area,
                Cost=x.Cost,
                Description=x.Description,
                NumOfPer=x.NumOfPer,
                RoomTypeCode=x.RoomTypeCode,
                RoomTypeId=x.RoomTypeId,
                RoomTypeName=x.RoomTypeName,
                NumOfRoom=x.Rooms.Count,

            }).ToList();
        }

        public async Task<IEnumerable<RoomTypeViewModel>> SearchClient(RoomTypeSearchModel roomTypeViewModel)
        {
            var query = await _repository.Search(roomTypeViewModel.RoomTypeCode, roomTypeViewModel.RoomTypeName);
            return query.Select(x => new RoomTypeViewModel()
            {
                Area = x.Area,
                Cost = x.Cost,
                ConvenientName= _roomCovenientRepository.GetRoomConvenientsByRoomTypeId(x.RoomTypeId).Result.Select(x=>x.Convenient.ConvenientName).ToList(),
                Description = x.Description,
                NumOfPer = x.NumOfPer,
                RoomTypeCode = x.RoomTypeCode,
                RoomTypeId = x.RoomTypeId,
                RoomTypeName = x.RoomTypeName,
                NumOfRoom = x.Rooms.Count,
                ImageLinks = x.Images.Select(x => x.ImageLink).ToList(),
                BedNumber= _roomBedRepository.GetRoomBedsByRoomTypeId(x.RoomTypeId).Result.Select(y=>y.NumOfBed +" "+ y.Bed.BedType).ToList(),
               
            }).ToList();
        }
        public async Task<RoomTypeUpdteVM> GetByIdInclude(int id)
        {
            var roomType = await _repository.GetByIdInclude(id);
            return new RoomTypeUpdteVM()
            {
                Area = roomType.Area,
                Cost = roomType.Cost,
                Description = roomType.Description,
                NumOfPer = roomType.NumOfPer,
                RoomTypeCode = roomType.RoomTypeCode,
                RoomTypeId = roomType.RoomTypeId,
                RoomTypeName = roomType.RoomTypeName,
                NumOfRoom = roomType.Rooms.Count(),
                ConvenientId = roomType.RoomConvenients.Select(x => x.ConvenientId).ToList(),
                BedNumber = roomType.RoomBeds.Select(x => x.BedId + "-" + x.NumOfBed).ToList(),
                HotelBranchId = roomType.HotelBranchId,
                ImageLinks = roomType.Images.Select(x => x.ImageLink).ToList(),
            };
        }
        public async Task<bool> Delete(int id)
        {
            var roomtType = await _repository.GetByIdInclude(id);
            return await _repository.Delete(roomtType);
        }
        public async Task<bool> Insert(RoomTypeViewModel roomTypeVM)
        {
            List<Images> images = new List<Images>();
            foreach (var link in roomTypeVM.ImageLinks)
            {
                var img = new Images()
                {
                    ImageLink = link
                };
                var image = await _imageRepository.Insert(img);
                if (image != null)
                    images.Add(image);
            }

            var roomType = new RoomTypes()
            {
                RoomTypeCode = roomTypeVM.RoomTypeCode,
                RoomTypeName = roomTypeVM.RoomTypeName,
                NumOfPer = (int)roomTypeVM.NumOfPer,
                Area = (float)roomTypeVM.Area,
                Cost = (decimal)roomTypeVM.Cost,
                HotelBranchId =(await _hotelBrachRepository.GetFristOrDefault()).HotelBranchId,
                Description = roomTypeVM.Description,
                Images = images,
            };
            var checkRoomtype= await _repository.Insert(roomType);
            if(checkRoomtype)
            {
                var roomTypeSelect = await _repository.GetByCode(roomTypeVM.RoomTypeCode);
                //create rooms
                List<Rooms> rooms = new List<Rooms>();
                for (int i = 0; i < roomTypeVM.NumOfRoom; i++)
                {
                    var room = new Rooms()
                    {
                        RoomCode = roomTypeVM.RoomTypeCode + " - R" + i,
                        RoomTypeId=roomTypeSelect.RoomTypeId,
                        Status = (int)RoomEnum.Free,
                    };
                     await _roomRepository.Insert(room);
                    rooms.Add(room);
                }
                //create bed of room
                var listRoomBeds = new List<RoomBeds>();
                foreach (var bed in roomTypeVM.BedNumber)
                {
                    string[] idBeds = bed.Trim().Split('-');
                    if(int.Parse(idBeds[1])>0)
                    {
                        RoomBeds beds = new RoomBeds()
                        {
                            BedId = int.Parse(idBeds[0]),
                            RoomTypeId= roomTypeSelect.RoomTypeId,
                            NumOfBed = int.Parse(idBeds[1]),
                        };
                        await _roomBedRepository.Insert(beds);
                        listRoomBeds.Add(beds);
                    }    
                    
                }
                //create convenient of room
                var convenientRooms = new List<RoomConvenients>();
                foreach (var convenient in roomTypeVM.ConvenientId)
                {
                    RoomConvenients roomConvenients = new RoomConvenients()
                    {
                        ConvenientId=convenient,
                        RoomTypeId=roomTypeSelect.RoomTypeId,
                    };
                    await _roomCovenientRepository.Insert(roomConvenients);
                    convenientRooms.Add(roomConvenients);
                }

            }
            return checkRoomtype;
        }

        public async Task<bool> Update(RoomTypeUpdteVM roomTypeUpdateViewModel)
        {
            //delete images
            List<Images> imgDeletes = (await _imageRepository.GetImagesByRoomTypeId((int)roomTypeUpdateViewModel.RoomTypeId)).ToList();
            foreach (var img in imgDeletes)
            {
                await _imageRepository.Delete(img);
            }
            //add images
            List<Images> images = new List<Images>();
            foreach (var link in roomTypeUpdateViewModel.ImageLinks)
            {
                var img = new Images()
                {
                    RoomTypeId = roomTypeUpdateViewModel.RoomTypeId,
                    ImageLink = link
                };
                var image = await _imageRepository.Insert(img);
                if (image != null)
                    images.Add(image);
            }

            //create rooms
            List<Rooms> rooms = new List<Rooms>();
            var listRoomDel = (await _roomRepository.GetRoomsByRoomTypeId(roomTypeUpdateViewModel.RoomTypeId)).ToList();
            foreach (var item in listRoomDel)
            {
                item.RoomCode = roomTypeUpdateViewModel.RoomTypeCode + " - R" + item.RoomCode.Substring((item.RoomCode.LastIndexOf('R') + 1));
                await _roomRepository.Update(item);
                rooms.Add(item);
            }
            Rooms roomDel = listRoomDel.ToList().FirstOrDefault();
            int demRoom = int.Parse(roomDel.RoomCode.Substring(roomDel.RoomCode.LastIndexOf('R') + 1));
            for (int i = 0; i < roomTypeUpdateViewModel.NumOfRoom-listRoomDel.Count; i++)
            {
                var room = new Rooms()
                {
                    RoomCode = roomTypeUpdateViewModel.RoomTypeCode + " - R" + (demRoom+1),
                    RoomTypeId = roomTypeUpdateViewModel.RoomTypeId,
                    Status = (int)RoomEnum.Free,
                };
                await _roomRepository.Insert(room);
                rooms.Add(room);
                demRoom++;
            }
            //remove bed of room 
            List<RoomBeds> bedDelete = (await _roomBedRepository.GetRoomBedsByRoomTypeId(roomTypeUpdateViewModel.RoomTypeId)).ToList();
            foreach (var bed in bedDelete)
            {
                await _roomBedRepository.Delete(bed);
            }
            //create bed of room
            var listRoomBeds = new List<RoomBeds>();
            foreach (var bed in roomTypeUpdateViewModel.BedNumber)
            {
                string[] idBeds = bed.Trim().Split('-');
                if (int.Parse(idBeds[1]) > 0)
                {
                    RoomBeds beds = new RoomBeds()
                    {
                        BedId = int.Parse(idBeds[0]),
                        RoomTypeId = roomTypeUpdateViewModel.RoomTypeId,
                        NumOfBed = int.Parse(idBeds[1]),
                    };
                    await _roomBedRepository.Insert(beds);
                    listRoomBeds.Add(beds);
                }

            }
            //remove convenient of room
            List<RoomConvenients> convenientDelete = (await _roomCovenientRepository.GetRoomConvenientsByRoomTypeId(roomTypeUpdateViewModel.RoomTypeId)).ToList();
            foreach (var covenient in convenientDelete)
            {
                await _roomCovenientRepository.Delete(covenient);
            }
            //create convenient of room
            var convenientRooms = new List<RoomConvenients>();
            foreach (var convenient in roomTypeUpdateViewModel.ConvenientId)
            {
                RoomConvenients roomConvenients = new RoomConvenients()
                {
                    ConvenientId = convenient,
                    RoomTypeId = roomTypeUpdateViewModel.RoomTypeId,
                };
                await _roomCovenientRepository.Insert(roomConvenients);
                convenientRooms.Add(roomConvenients);
            }

            var roomType = new RoomTypes()
            {
                RoomTypeCode = roomTypeUpdateViewModel.RoomTypeCode,
                RoomTypeName = roomTypeUpdateViewModel.RoomTypeName,
                NumOfPer = (int)roomTypeUpdateViewModel.NumOfPer,
                Area = (float)roomTypeUpdateViewModel.Area,
                Cost = (decimal)roomTypeUpdateViewModel.Cost,
                HotelBranchId = (await _hotelBrachRepository.GetFristOrDefault()).HotelBranchId,
                Description = roomTypeUpdateViewModel.Description,
                Images = images,
                RoomBeds=listRoomBeds,
                RoomConvenients=convenientRooms,
                Rooms=rooms,
                RoomTypeId=roomTypeUpdateViewModel.RoomTypeId,
            };
            return await _repository.Update(roomType);
        }
    }
}
