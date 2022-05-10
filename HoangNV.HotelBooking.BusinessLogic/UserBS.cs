using HoangNV.HotelBooking.BusinessLogic.Interface;
using HoangNV.HotelBooking.BusinessLogic.Models;
using HoangNV.HotelBooking.Core.Repository.Interface;
using HoangNV.HotelBooking.Core.Services;
using HoangNV.HotelBooking.Entities.Entities;
using HoangNV.HotelBooking.Repository.Enum;
using HoangNV.HotelBooking.Repository.Interface;
using HoangNV.HotelBooking.Repository.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.BusinessLogic
{
    public class UserBS : BaseBS<Users>, IUserBS
    {
        public IUserRepository _repository;
        public IRoleRepository _roleRepository;
        public UserBS(IUserRepository repository, IRoleRepository roleRepository) : base(repository)
        {
            _repository = repository;
            _roleRepository = roleRepository;
        }

        public UserViewModel GetUsers(string userName, string passWord)
        {
            var (user,role) = _repository.GetUser(userName, passWord);
            if (user != null)
                return new UserViewModel()
                {
                    Username = user.UserName,
                    Role = role,
                    FullName=user.FullName,
                };
            return null;
        }

        public async Task<bool> Insert(UserAddViewModel model)
        {
            var nameRole = System.Enum.GetName(typeof(RoleEnum), model.RoleId);
            PasswordHasher<Users> passwordHasher = new PasswordHasher<Users>();
            var user = new Users()
            {
                Active = 1,
                RoleId =(await _roleRepository.GetRoleByName(nameRole)).RoleId,
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                Id = Guid.NewGuid().ToString(),
            };
            user.PassWord = passwordHasher.HashPassword(user, model.PassWord);
            return await _repository.Insert(user);
        }

        public async Task<IEnumerable<UserResultModel>> Search(UserSearchModel model)
        {
            var result = await _repository.GetList(model.UserName, model.FullName, model.Role);
            return result.Select(x => new UserResultModel()
            {
                UserId=x.Id,
                UserName = x.UserName,
                FullName = x.FullName,
                Email = x.Email,
                Role = x.Roles.RoleName,
            }).ToList();
        }

        public async Task<IEnumerable<UserResultModel>> SearchAll()
        {
            var result = await _repository.GetListAll();
            return result.Select(x => new UserResultModel()
            {
                UserName = x.UserName,
                FullName = x.FullName,
                Email = x.Email,
                Role = x.Roles.RoleName,
            }).ToList();
        }
        public async Task<UserUpdateViewModel> GetUserByName(string userName)
        {
            var result= await _repository.GetUserByName(userName);
            if (result != null)
                return new UserUpdateViewModel()
                {
                    Email= result.Email,
                    FullName=result.FullName,
                    UserName=result.UserName,
                    UserId=result.Id,
                };
            return new UserUpdateViewModel();
        }

        public async Task<PassWordUpdateViewModel> GetUserUpdatePassByName(string userName)
        {
            var result = await _repository.GetUserByName(userName);
            if (result != null)
                return new PassWordUpdateViewModel()
                {
                    UserId = result.Id,
                };
            return new PassWordUpdateViewModel();
        }

        public async Task<bool> UpdateUser(string id)
        {
            var user = await _repository.GetByIdAsync(id);
            if(user!=null)
            {
                user.Active = 2;
                return await _repository.Update(user);
            }
            return false;
        }

        public async Task<bool> UpdateMyUser(UserUpdateViewModel model)
        {
            var user = await _repository.GetByIdAsync(model.UserId);
            if (user != null)
            {
                user.FullName = model.FullName;
                user.Email = model.Email;
                return await _repository.Update(user);
            }
            return false;
        }
        public async Task<bool> UpdateMyPass(PassWordUpdateViewModel model)
        {
            var user = await _repository.GetByIdAsync(model.UserId);
            if (user != null)
            {
                PasswordHasher<Users> passwordHasher = new PasswordHasher<Users>();
                user.PassWord = passwordHasher.HashPassword(user, model.PassWord);
                return await _repository.Update(user);
            }
            return false;
        }
    }
}
