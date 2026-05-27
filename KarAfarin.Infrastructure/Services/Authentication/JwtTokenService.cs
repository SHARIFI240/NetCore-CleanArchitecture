using KarAfarin.Application.Common.Interfaces.Authentication;
using KarAfarin.Application.Common.Interfaces.Services;
using KarAfarin.Application.Common.Interfaces.Services.Authentication;
using KarAfarin.Application.Common.Models.Authentication;
using KarAfarin.Domain.Authentication.Entities;
using KarAfarin.Infrastructure.Persistence.Repositories.Authentication;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace KarAfarin.Infrastructure.Services.Authentication
{
    public class JwtTokenService(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IConfiguration _configuration,
        IHashService hashService,
        IRoleRepository roleRepository
        ) : IJwtTokenService
    {
        // --- تنظیمات کلیدی ---
        private string SecretKey => _configuration["Jwt:Key"]; // در Production از config خوانده شود
        private string Issuer => _configuration["Jwt:Issuer"];
        private string Audience => _configuration["Jwt:Audience"];
        private int AccessTokenLifetime => int.Parse(_configuration["Jwt:AccessTokenExpirationMinutes"]);
        private int RefreshTokenLifetime => 7 * 24 * 60; // 7 روز به دقیقه

        public string GenerateCode()
        {
            return new Random().Next(1000, 9999).ToString();
        }


        public async Task<string> HashCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return string.Empty;



            return hashService.Hash(code);
        }


        // --- تولید Access Token (JWT) ---
        public async Task<TokenResponse> GenerateTokenAsync(Users user, CancellationToken cancellationToken)
        {
            // دریافت نقش‌های کاربر
            var userRoles = await roleRepository.GetRolesByUserIdAsync(user.Id, cancellationToken);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName ?? user.PhoneNumber),
                new(ClaimTypes.Email, user.Email ?? "UserEmail"), // ایمیل خالی اگر وجود ندارد
                new(ClaimTypes.MobilePhone, user.PhoneNumber),
            };

            // اگر کاربر نقش‌های متعددی دارد، بهتر است برای هر نقش یک Claim جداگانه بسازید
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role.Name)));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                expires: DateTime.Now.AddMinutes(AccessTokenLifetime),
                claims: claims,
                signingCredentials: credentials
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            // ساخت ریسپانس نهایی (شامل دسترسی)
            // این توکن باید توسط کلاینت ذخیره و در هدر Authorization ارسال شود
            return new TokenResponse
            {
                AccessToken = accessToken,
                ExpiresIn = AccessTokenLifetime,
                TokenType = "Bearer"
                // RefreshToken فعلا در اینجا نیست، در لایه بالاتر تولید می‌شود
            };
        }


        // --- تولید و ذخیره Refresh Token ---
        public async Task<TokenResponse> GenerateTokenWithRefreshAsync(Users user, CancellationToken cancellationToken)
        {
            // 1. تولید Access Token
            var tokenResponse = await GenerateTokenAsync(user, cancellationToken);

            // 2. تولید Refresh Token (UUID + تاریخ انقضا)
            var refreshToken = GenerateRefreshTokenEntity();

            // 3. ذخیره در دیتابیس
            await refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

            // 4. بازگشت هر دو توکن به کلاینت
            tokenResponse.RefreshToken = refreshToken.Token;
            tokenResponse.RefreshTokenExpiresIn = RefreshTokenLifetime / 60; // به دقیقه
            tokenResponse.RefreshTokenId = refreshToken.Id; // اگر نیاز به ردیابی دارید

            return tokenResponse;
        }

        public RefreshTokens GenerateRefreshTokenEntity()
        {
            var token = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N"); // تولید UUID
            return new RefreshTokens
            {
                Token = token,
                ExpireAt = DateTime.Now.AddMinutes(RefreshTokenLifetime),
                CreateAt = DateTime.Now
                // IpAddress و RevokedAt را می‌توانید در Handler تنظیم کنید
            };
        }

        // ---  اعتبارسنجی و تمدید Access Token ---
        public async Task<TokenResponse> RefreshAccessTokenAsync(string refreshToken, int? oldTokenId, CancellationToken cancellationToken)
        {
            // پیدا کردن ریفرش تان در دیتابیس
            var dbToken = await refreshTokenRepository.GetByTokenAsync(refreshToken, cancellationToken);

            if (dbToken == null || dbToken.RevokedAt != null)
                throw new UnauthorizedAccessException("Refresh Token معتبر نیست یا منقضی شده است.");

            // اگر ایندکس قدیمی داشتیم، آن را باطل کن
            if (oldTokenId != null && oldTokenId != dbToken.Id)
            {
                var oldToken = await refreshTokenRepository.GetByIdAsync((int)oldTokenId, cancellationToken);
                if (oldToken != null)
                {
                    oldToken.Revoke(DateTime.Now); 
                    await refreshTokenRepository.UpdateAsync(oldToken, cancellationToken);
                }
            }

            // تولید توکن جدید
            var user = await userRepository.GetUserByIdAsync(dbToken.UserId, cancellationToken);
            return await GenerateTokenWithRefreshAsync(user, cancellationToken);
        }

        // ---  باطل کردن (Revoke) توکن ---
        public async Task RevokeTokenAsync(int userId, string token, CancellationToken cancellationToken)
        {
            var tokenRecord = await refreshTokenRepository.GetByTokenAsync(token, cancellationToken);
            if (tokenRecord != null && !tokenRecord.RevokedAt.HasValue)
            {
                tokenRecord.Revoke(DateTime.UtcNow);
                await refreshTokenRepository.UpdateAsync(tokenRecord, cancellationToken);
            }
        }

        public async Task<AccessTokenResult> GenerateAccessTokenAsync(Users user, CancellationToken cancellationToken)
        {
            //  خواندن تنظیمات JWT
            var jwtSection = _configuration.GetSection("Jwt");

            var secretKey = jwtSection["Key"]!;
            var issuer = jwtSection["Issuer"];
            var audience = jwtSection["Audience"];
            var expireMinutes = int.Parse(jwtSection["AccessTokenExpirationMinutes"]!);

            // کلید امضا
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey));


            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);


            // Claimهای پایه
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.MobilePhone, user.PhoneNumber),
            };

            var roles = await roleRepository.GetRolesByUserIdAsync(user.Id, cancellationToken);

            foreach (var item in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item.Name));
            }

            // زمان انقضا
            var expiresAt = DateTime.Now.AddMinutes(expireMinutes);

            // ساخت توکن
            var tokenDescriptor = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            var token = new JwtSecurityTokenHandler()
                .WriteToken(tokenDescriptor);

            return new AccessTokenResult
            {
                Token = token,
                ExpiresIn = expireMinutes * 60
            };

        }

    }
}
