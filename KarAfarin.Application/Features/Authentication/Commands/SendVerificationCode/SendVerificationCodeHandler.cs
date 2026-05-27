using KarAfarin.Application.Common.Interfaces.Authentication;
using KarAfarin.Application.Common.Interfaces.Services.Authentication;
using KarAfarin.Application.Common.Models.ResultOperation;
using KarAfarin.Domain.Authentication.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Features.Authentication.Commands.SendVerificationCode
{
    public class SendVerificationCodeHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService, IRoleRepository roleRepository) : IRequestHandler<SendVerificationCodeCommand, ResultOperation>
    {
        public async Task<ResultOperation> Handle(SendVerificationCodeCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetUserByPhoneNumberAsync(request.PhoneNumber, cancellationToken);

            var code = jwtTokenService.GenerateCode();

            var hashCode = await jwtTokenService.HashCode(code);

            if (user == null)
            {

                Domain.Authentication.Entities.Users newUser = new Domain.Authentication.Entities.Users()
                { 
                    PhoneNumber = request.PhoneNumber,
                    FirstName = "",
                    LastName = "",
                    IsActive = true,
                    RegisterDate = DateTime.Now,
                    UserName = request.PhoneNumber,
                    LoginCodeHash = hashCode,
                };

                await userRepository.AddUserAsync(newUser, cancellationToken);
                await roleRepository.AddRolesToUserAsync(["NormalUser"], newUser, cancellationToken);
            }
            else { 
                    
                user.LastLoginDate = DateTime.Now;
                user.LoginCodeHash = hashCode;
                await userRepository.UpdateUserAsync(user, cancellationToken);
            }


            return ResultOperation.Ok(request.PhoneNumber, "کد تایید با موفقیت برای کاربر ارسال شد");
        }
    }
}
