using KarAfarin.Application.Common.Interfaces.Authentication;
using KarAfarin.Application.Common.Interfaces.Services;
using KarAfarin.Application.Common.Interfaces.Services.Authentication;
using KarAfarin.Application.Common.Models.Authentication;
using KarAfarin.Application.Common.Models.ResultOperation;
using KarAfarin.Domain.Authentication.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace KarAfarin.Application.Features.Authentication.Commands.LoginWithPassword
{
    public class LoginWithPasswordHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService, IHashService hashService, IRefreshTokenRepository refreshTokenRepository, IRoleRepository roleRepository) : IRequestHandler<LoginWithPasswordCommand, TokenResponse>
    {
        public async Task<TokenResponse> Handle(LoginWithPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetUserByPhoneNumberAsync(request.PhoneNumber, cancellationToken);
            if (user != null)
            {
                var roles = await roleRepository.GetRolesByUserIdAsync(user.Id, cancellationToken);

                if (!user.IsActive)
                {
                    return new TokenResponse
                    {
                        ErrorMessage = "حساب کاربری شما غیر فعال می باشد"
                    };
                }

                if (user.FailedLoginAttempts >= 3)
                {
                    return new TokenResponse
                    {
                        ErrorMessage = "حساب کاربری شما به دلیل تلاش های ناموفق برای ورود موقتا غیر فعال شده است"
                    };
                }

                if (!hashService.Verify(request.Password, user.PasswordHash))
                {
                    user.FailedLoginAttempts++;
                    await userRepository.UpdateUserAsync(user, cancellationToken);

                    return new TokenResponse
                    {
                        ErrorMessage = "نام کاربری یا رمزعبور اشتباه می باشد"
                    };

                }

                user.FailedLoginAttempts = 0;

                await userRepository.UpdateUserAsync(user, cancellationToken);

                var accessToken = await jwtTokenService.GenerateAccessTokenAsync(user, cancellationToken);

                await refreshTokenRepository.RevokedLastToken(user.Id, accessToken.Token, cancellationToken);
                
                

                RefreshTokens refreshToken = new RefreshTokens()
                {
                    CreateAt = DateTime.Now,
                    User = user,
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
                    ReturnToAddress = roles.Any(c=>c.Name == "Admin") ? "/Admin/Dashboard/Index" : "/Home/Index"
                };



            }

            return new TokenResponse
            {
                ErrorMessage = "هیچ کاربری با این مشخصات پیدا نشد"
            };

        }
    }
}
