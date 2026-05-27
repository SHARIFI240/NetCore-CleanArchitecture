using KarAfarin.Application.Common.Interfaces.Authentication;
using KarAfarin.Application.Common.Interfaces.Services;
using KarAfarin.Application.Common.Interfaces.Services.Authentication;
using KarAfarin.Application.Common.Models.Authentication;
using KarAfarin.Application.Common.Models.ResultOperation;
using KarAfarin.Domain.Authentication.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text;

namespace KarAfarin.Application.Features.Authentication.Commands.VerifyCode
{
    public class VerifyCodeHandler(
        IUserRepository userRepository,
        IHashService hashService, 
        IJwtTokenService jwtTokenService,
        IRefreshTokenRepository refreshTokenRepository,
        IRoleRepository roleRepository
        ) : IRequestHandler<VerifyCodeCommand, TokenResponse>
    {
        public async Task<TokenResponse> Handle(VerifyCodeCommand request, CancellationToken cancellationToken)
        {
            var thisUser = await userRepository.GetUserByPhoneNumberAsync(request.PhoneNumber, cancellationToken);
            if (thisUser != null)
            {
                var roles = await roleRepository.GetRolesByUserIdAsync(thisUser.Id, cancellationToken);


                if (!thisUser.IsActive)
                {                   
                    return new TokenResponse()
                    {
                        ErrorMessage = "حساب کاربری شما غیر فعال می باشد"
                    };
                }

                if (thisUser.FailedLoginAttempts >= 3)
                {
                    return new TokenResponse()
                    {
                        ErrorMessage = "حساب کاربری شما به دلیل تلاش های ناموفق برای ورود موقتا غیر فعال شده است"
                    };
                }

                if (!hashService.Verify(request.VerifyCode, thisUser.LoginCodeHash))
                {

                    thisUser.FailedLoginAttempts++;
                    await userRepository.UpdateUserAsync(thisUser, cancellationToken);

                    return new TokenResponse()
                    {
                        ErrorMessage = "کد وارد شده نامعتبر می باشد"
                    };
                }

                thisUser.FailedLoginAttempts = 0;
                thisUser.LoginCodeHash = null;

                await userRepository.UpdateUserAsync(thisUser,cancellationToken);

                var accessToken = await jwtTokenService.GenerateAccessTokenAsync(thisUser, cancellationToken);

                await refreshTokenRepository.RevokedLastToken(thisUser.Id, accessToken.Token, cancellationToken);

                RefreshTokens refreshToken = new RefreshTokens()
                { 
                    CreateAt = DateTime.Now,
                    User = thisUser,
                    Token = accessToken.Token,
                    IpAddress = request.IpAddress,
                    ExpireAt = DateTime.Now.AddDays(30),
                };

                await refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

                return new TokenResponse
                {
                    AccessToken = accessToken.Token,
                    ExpiresIn = accessToken.ExpiresIn,
                    TokenType = "Bearer",
                    RefreshToken = refreshToken.Token,
                    RefreshTokenExpiresIn = (int)(refreshToken.ExpireAt - DateTime.Now).TotalSeconds,
                    ReturnToAddress = roles.Any(c => c.Name == "Admin") ? "/Admin/Dashboard/Index" : "/Home/Index"

                };
            }

            return new TokenResponse()
            {
                ErrorMessage = "تایید شماره تماس با خطا مواجه شد"
            };

        }

    }
}
