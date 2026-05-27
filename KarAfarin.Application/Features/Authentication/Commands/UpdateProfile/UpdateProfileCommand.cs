using KarAfarin.Application.Common.Models.ResultOperation;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KarAfarin.Application.Features.Authentication.Commands.UpdateProfile
{
    public class UpdateProfileCommand : IRequest<ResultOperation>
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "وارد کردن نام الزامی می باشد")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "وارد کردن نام خانوادگی الزامی می باشد")]
        public string LastName { get; set; }

        public string? Email { get; set; }
    }
}
