using KarAfarin.Application.Common.Interfaces.Authentication;
using KarAfarin.Application.Common.Interfaces.Services.Authentication;
using KarAfarin.Application.Common.Models.ResultOperation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Features.Authentication.Commands.GeerateNewVerifyCode
{
    public record GenerateNewVerifyCodeCommand(string phoneNumber) : IRequest<ResultOperation>;

    public class GenerateNewVerifyCodeHandler(IJwtTokenService jwtTokenService, IUserRepository userRepository) : IRequestHandler<GenerateNewVerifyCodeCommand, ResultOperation>
    {
        public async Task<ResultOperation> Handle(GenerateNewVerifyCodeCommand request, CancellationToken cancellationToken)
        {
            var thisUser = await userRepository.GetUserByPhoneNumberAsync(request.phoneNumber, cancellationToken);
            if (thisUser != null)
            {
                var code = jwtTokenService.GenerateCode();

                var hashCode = await jwtTokenService.HashCode(code);

                thisUser.LoginCodeHash = hashCode;

                await userRepository.UpdateUserAsync(thisUser, cancellationToken);

                return ResultOperation.Ok(null, "کد جدید با موفقیت ارسال شد");
            }


            return ResultOperation.Fail("کاربری با این مشخصات پیدا نشد");
        }
    }
}
