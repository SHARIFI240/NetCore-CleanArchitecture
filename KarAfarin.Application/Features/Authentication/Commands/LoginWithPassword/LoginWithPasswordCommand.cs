using KarAfarin.Application.Common.Models.Authentication;
using KarAfarin.Application.Common.Models.ResultOperation;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KarAfarin.Application.Features.Authentication.Commands.LoginWithPassword
{
    public class LoginWithPasswordCommand : IRequest<TokenResponse>
    {
       public string PhoneNumber { get; set; }

        public string Password { get; set; }

        public string IpAddress { get; set; } = "";
    }
}