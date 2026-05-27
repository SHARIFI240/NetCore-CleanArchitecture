using KarAfarin.Application.Common.Models.Authentication;
using KarAfarin.Domain.Authentication.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Common.Interfaces.Services.Authentication
{
    public interface IJwtTokenService
    {
        string GenerateCode();
        Task<string> HashCode(string code);
        Task<TokenResponse> GenerateTokenAsync(Users user, CancellationToken cancellationToken);
        Task<TokenResponse> GenerateTokenWithRefreshAsync(Users user, CancellationToken cancellationToken);
        RefreshTokens GenerateRefreshTokenEntity();
        Task<TokenResponse> RefreshAccessTokenAsync(string refreshToken, int? oldTokenId, CancellationToken cancellationToken);
        Task RevokeTokenAsync(int userId, string token, CancellationToken cancellationToken);
        Task<AccessTokenResult> GenerateAccessTokenAsync(Users user, CancellationToken cancellationToken);
    }
}
