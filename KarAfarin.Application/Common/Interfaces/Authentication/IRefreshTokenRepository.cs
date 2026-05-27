using KarAfarin.Domain.Authentication.Entities;

namespace KarAfarin.Application.Common.Interfaces.Authentication
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshTokens?> GetByTokenAsync(string token, CancellationToken cancellationToken);
        Task<RefreshTokens?> GetByUserIdAndActiveAsync(int userId, CancellationToken cancellationToken);
        Task AddAsync(RefreshTokens token, CancellationToken cancellationToken);
        Task UpdateAsync(RefreshTokens token, CancellationToken cancellationToken);
        Task RemoveAsync(RefreshTokens token, CancellationToken cancellationToken);
        Task<RefreshTokens> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task DeactivateTokensByUserIdAsync(int userId, CancellationToken cancellationToken);
        Task RevokedLastToken(int userId, string newToken, CancellationToken cancellationToken);
    }
}