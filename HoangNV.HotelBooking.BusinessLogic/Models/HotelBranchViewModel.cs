using FluentValidation;
using HoangNV.HotelBooking.BusinessLogic.Interface;
using HoangNV.HotelBooking.Repository.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.BusinessLogic.Models
{
    public class HotelBranchViewModel
    {
        public int HotelBranchId { get; set; }
        public string HotelBranchName { get; set; }
        public string HotelBranchCode { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int? Status { get; set; }
        public string Description { get; set; }
        public IList<string> ImageLinks { get; set; } = new List<string>();
    }

    public class HotelBranchValidator : AbstractValidator<HotelBranchViewModel>
    {
        public HotelBranchValidator(IStringLocalizer<HotelBranchViewModel> localizer)
        {
            RuleFor(x=>x.HotelBranchCode).Cascade(CascadeMode.Stop)
                 .NotEmpty().WithMessage(localizer["E_A002_02"])
                .NotNull()
                .WithMessage(localizer["E_A002_02"])
                .MaximumLength(50)
                .WithMessage(localizer["E_C_008_02"])
            .WithName(localizer["Display_HotelBranchCode"]);

            RuleFor(x => x.HotelBranchName).Cascade(CascadeMode.Stop)
                 .NotEmpty().WithMessage(localizer["E_A002_02"])
                .NotNull()
                .WithMessage(localizer["E_A002_02"])
                .MaximumLength(50)
                .WithMessage(localizer["E_C_008_02"])
                
            .WithName(localizer["Display_HotelBranchName"]);

            RuleFor(x => x.Address).Cascade(CascadeMode.Stop)
                 .NotEmpty().WithMessage(localizer["E_A002_02"])
                .NotNull()
                .WithMessage(localizer["E_A002_02"])
                .MaximumLength(50)
                .WithMessage(localizer["E_C_008_02"])
                
            .WithName(localizer["Display_HotelBranchAddress"]);

            RuleFor(i => i.Email).Cascade(CascadeMode.Stop)
              .NotEmpty().WithMessage(localizer["E_A002_02"])
              .NotNull()
              .WithMessage(localizer["E_A002_02"])
              .MaximumLength(50)
              .WithMessage(localizer["E_C_008_02"])
              .EmailAddress()
              .WithMessage(localizer["E_C_001_09"])
          .WithName(localizer["Display_Email"]);

            RuleFor(i => i.PhoneNumber).Cascade(CascadeMode.Stop)
              .NotEmpty().WithMessage(localizer["E_A002_02"])
              .NotNull()
              .WithMessage(localizer["E_A002_02"])
              .MaximumLength(50)
              .WithMessage(localizer["E_C_008_02"])
              .WithMessage(localizer["E_C_001_09"])
          .WithName(localizer["Display_PhoneNumber"]);
        }

    }
}
