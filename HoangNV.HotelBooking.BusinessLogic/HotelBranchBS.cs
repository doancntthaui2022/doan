using HoangNV.HotelBooking.BusinessLogic.Enum;
using HoangNV.HotelBooking.BusinessLogic.Interface;
using HoangNV.HotelBooking.BusinessLogic.Models;
using HoangNV.HotelBooking.Core.Services;
using HoangNV.HotelBooking.Entities.Entities;
using HoangNV.HotelBooking.Repository.Enum;
using HoangNV.HotelBooking.Repository.Interface;
using HoangNV.HotelBooking.Repository.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.BusinessLogic
{
    public class HotelBranchBS : BaseBS<HotelBranchs>, IHotelBranchBS
    {
        public IHotelBranchRepository _repository;
        public IImageRepository _imageRepository;
        public HotelBranchBS(IHotelBranchRepository repository,IImageRepository imageRepository) : base(repository)
        {
            _repository = repository;
            _imageRepository = imageRepository;
        }
        //public async Task<HotelBranchViewModel> SearchById(int id)
        //{
        //    var query = await _repository.SearchById(id);
        //    return new HotelBranchViewModel()
        //    {
        //        HotelBranchId=query.HotelBranchId,
        //        HotelBranchCode=query.HotelBranchCode,
        //        HotelBranchName=query.HotelBranchName,
        //        Address=query.Address,
        //        Description=query.Description,
        //        Status=query.Status,
        //        ImageLinks=query.Images.Select(x=>x.ImageLink).ToList(), 
        //    };
        //}
        //public async Task<IEnumerable<HotelBranchQueryModel>> Search(HotelBranchSearchModel branchSearchModel)
        //{
        //    var query= await _repository.Search();
        //    if (branchSearchModel.HotelBranchCode != null)
        //        query = query.Where(x => x.HotelBranchCode.ToLower().Trim().Contains(branchSearchModel.HotelBranchCode.Trim().ToLower())).ToList();
        //    if (branchSearchModel.HotelBranchName != null)
        //        query = query.Where(x => x.HotelBranchName.ToLower().Trim().Contains(branchSearchModel.HotelBranchName.Trim().ToLower())).ToList();
        //    if (branchSearchModel.Address != null)
        //        query = query.Where(x => x.Address.ToLower().Trim().Contains(branchSearchModel.Address.Trim().ToLower())).ToList();
        //    if (branchSearchModel.Status != (int)HotelBranchStatus.All)
        //        query = query.Where(x => x.Status == branchSearchModel.Status).ToList();
        //    return query.Select(p => new HotelBranchQueryModel()
        //    {
        //        HotelBranchId=p.HotelBranchId,
        //        HotelBranchCode=p.HotelBranchCode,
        //        HotelBranchName=p.HotelBranchName,
        //        Address=p.Address,
        //        Status=p.Status,
        //        Description=p.Description,
        //        ImageLinks=p.Images.Select(x=>x.ImageLink).ToList(),
        //    }).ToList();
        //}

        //public async Task<bool> Insert(HotelBranchViewModel hotelVM)
        //{
        //    List<Images> images = new List<Images>();
        //    foreach (var link in hotelVM.ImageLinks)
        //    {
        //        var img = new Images()
        //        {
        //            ImageLink = link
        //        };
        //        var image= await _imageRepository.Insert(img);
        //        if (image != null)
        //            images.Add(image);
        //    }
        //    var hotelBranch = new HotelBranchs()
        //    {
        //        HotelBranchCode = hotelVM.HotelBranchCode,
        //        HotelBranchName = hotelVM.HotelBranchName,
        //        Address = hotelVM.Address,
        //        Description = hotelVM.Description,
        //        Status = (int)HotelBranchStatus.Active,
        //        Images=images,
        //    };
        //    return await _repository.Insert(hotelBranch);
        //}

        //public async Task<bool> Delete(int id)
        //{
        //    var hotel = await _repository.GetByIdAsync(id);
        //    if (hotel.Status == (int)HotelEnum.NonActived)
        //        return await _repository.Delete(id);
        //    else return await _repository.UpdateStatus(id);
        //}

        public async Task<HotelBranchViewModel> GetFristOrDefault()
        {
            var hotel= await _repository.GetFristOrDefault();
            return new HotelBranchViewModel()
            {
                HotelBranchId = hotel.HotelBranchId,
                HotelBranchCode = hotel.HotelBranchCode,
                HotelBranchName = hotel.HotelBranchName,
                Address = hotel.Address,
                Status = hotel.Status,
                Description = hotel.Description,
                Email = hotel.Email,
                PhoneNumber = hotel.PhoneNumber,
                ImageLinks = hotel.Images.Select(x => x.ImageLink).ToList(),
            };
        }

        public async Task<bool> Update(HotelBranchViewModel hotelVM)
        {
            //delete images
            List<Images> imgDeletes = _imageRepository.GetImagesByHotelBranchId(hotelVM.HotelBranchId).ToList();
            foreach (var img in imgDeletes)
            {
                await _imageRepository.Delete(img);
            }
            //add images
            List<Images> images = new List<Images>();
            foreach (var link in hotelVM.ImageLinks)
            {
                var img = new Images()
                {
                    ImageLink = link
                };
                var image = await _imageRepository.Insert(img);
                if (image != null)
                    images.Add(image);
            }
            var hotelBranch = new HotelBranchs()
            {
                HotelBranchId=hotelVM.HotelBranchId,
                HotelBranchCode = hotelVM.HotelBranchCode,
                HotelBranchName = hotelVM.HotelBranchName,
                Address = hotelVM.Address,
                Description = hotelVM.Description,
                Status = (int)HotelBranchStatus.Active,
                Images = images,
                PhoneNumber= hotelVM.PhoneNumber,
                Email=hotelVM.Email,
            };
            return await _repository.UpdateHotelBranch(hotelBranch);
        }
    }
}
