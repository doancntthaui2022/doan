using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.BusinessLogic.Models
{
    public class UserViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public string FullName { get; set; }
    }

    public class UserSearchModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public int Role { get; set; }
    }
    public class UserResultModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
    public class UserValidator : AbstractValidator<UserViewModel>
    {
        public UserValidator(IStringLocalizer<UserViewModel> localizer)
        {

            RuleFor(i => i.Username).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(localizer["E_A002_02"])
                .NotNull()
                .WithMessage(localizer["E_A002_02"])
                .MaximumLength(50)
                .WithMessage(localizer["E_C_008_02"])
            .WithName(localizer["Display_UserName"]);

            RuleFor(i => i.Password).Cascade(CascadeMode.Stop)
               .NotEmpty().WithMessage(localizer["E_A002_02"])
               .NotNull()
               .WithMessage(localizer["E_A002_02"])
               .MaximumLength(50)
               .WithMessage(localizer["E_C_008_02"])
           .WithName(localizer["Display_Password"]);
        }

    }

}
