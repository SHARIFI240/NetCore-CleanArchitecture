using KarAfarin.Application.Common.Interfaces.Authentication;
using KarAfarin.Domain.Authentication.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace KarAfarin.Infrastructure.Persistence.Repositories.Authentication
{
    public class RefreshTokenRepository(ApplicationDbContext context) : IRefreshTokenRepository
    {
        public async Task<RefreshTokens?> GetByTokenAsync(string token, CancellationToken cancellationToken) =>
        await context.RefreshTokens.FirstOrDefaultAsync(r => r.Token == token, cancellationToken);

        public async Task<RefreshTokens?> GetByUserIdAndActiveAsync(int userId, CancellationToken cancellationToken) =>
        await context.RefreshTokens
        .Where(r => r.UserId == userId && !r.IsExpired(DateTime.Now))
        .FirstOrDefaultAsync(cancellationToken);

        public async Task AddAsync(RefreshTokens token, CancellationToken cancellationToken)
        {
            context.RefreshTokens.Add(token);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(RefreshTokens token, CancellationToken cancellationToken)
        {
            context.RefreshTokens.Update(token);    
            await context.SaveChangesAsync(cancellationToken);  
        }

        public async Task<RefreshTokens> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await context.RefreshTokens.FindAsync(id,cancellationToken);
        }

        public async Task RemoveAsync(RefreshTokens token, CancellationToken cancellationToken)
        {
            context.RefreshTokens.Remove(token);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeactivateTokensByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            var tokens = await context.RefreshTokens
                .Where(x => x.UserId == userId && x.RevokedAt == null && x.ExpireAt > DateTime.Now)
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.Revoke(DateTime.Now);
            }

            await context.SaveChangesAsync();

        }

        public async Task RevokedLastToken(int userId, string newToken, CancellationToken cancellationToken)
        {
            var lastToken = await context.RefreshTokens.Where(g=>g.UserId == userId)
                .OrderByDescending(g=>g.CreateAt).FirstOrDefaultAsync(cancellationToken);

            if (lastToken != null)
            {
                lastToken.ReplacedByToken = newToken;
                lastToken.RevokedAt = DateTime.Now;

                context.RefreshTokens.Update(lastToken);

                await context.SaveChangesAsync(cancellationToken);
            }

        }

    }
}