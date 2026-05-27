using KarAfarin.Application.Common.Models.Authentication;
using KarAfarin.Application.Common.Models.ResultOperation;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KarAfarin.Application.Features.Authentication.Commands.VerifyCode
{
    public class VerifyCodeCommand : IRequest<TokenResponse>
    {
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "وارد کردن کد الزامی می باشد")]
        public string VerifyCode { get; set; }

        public string IpAddress { get; set; } = "";
    }
}
