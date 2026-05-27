using KarAfarin.Application.Common.Models.ResultOperation;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KarAfarin.Application.Features.Authentication.Commands.SendVerificationCode
{
    public class SendVerificationCodeCommand : IRequest<ResultOperation>
    {
        public string PhoneNumber { get; set; }
    }
}
