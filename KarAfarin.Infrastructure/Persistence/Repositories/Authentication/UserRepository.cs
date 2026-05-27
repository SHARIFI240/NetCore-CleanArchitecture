using KarAfarin.Application.Common.Interfaces.Authentication;
using KarAfarin.Application.Common.Models;
using KarAfarin.Application.Features.Authentication.Queries.GetUserById;
using KarAfarin.Application.Features.Blog.Category.Queries.GetCategories;
using KarAfarin.Application.Features.Users.Queries.GetUsersWithPagination;
using KarAfarin.Domain.Authentication.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Infrastructure.Persistence.Repositories.Authentication
{
    public class UserRepository(ApplicationDbContext context) : IUserRepository
    {
        public async Task AddUserAsync(Users user, CancellationToken cancellationToken)
        {
            await context.Users.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> CheckDuplicatedUserNameAsync(string userName, CancellationToken cancellationToken)
        {
            return await context.Users.AnyAsync(g => g.UserName == userName, cancellationToken);
        }

        public async Task<Users?> GetUserByIdAsync(int id, CancellationToken cancellationToken) => await context.Users.FindAsync(id, cancellationToken);

        public async Task<Users?> GetUserByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken) => await context.Users.FirstOrDefaultAsync(c=>c.PhoneNumber == phoneNumber, cancellationToken);

        public async Task UpdateUserAsync(Users user, CancellationToken cancellationToken)
        {
            context.Users.Update(user);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateLoginAttemptsAsync(int userId, int attempts, DateTime? lastLogin, CancellationToken cancellationToken)
        {
            var user = await context.Users.FindAsync(userId);
            if (user is null) return;

            user.FailedLoginAttempts = attempts;
            user.LastLoginDate = lastLogin;
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<PaginatedList<GetUsersWithPaginationDto>> GetUsersWithPaginationAsync(
            int page,
            string? searchParam,
            CancellationToken cancellationToken)
        {
            int take = 20;
            int skip = (page - 1) * take;

            var query = context.Users.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchParam))
            {
                query = query.Where(e =>
                    e.UserName.Contains(searchParam) ||
                    e.FirstName.Contains(searchParam) ||
                    ((e.FirstName + " " + e.LastName).Contains(searchParam)) ||
                    e.LastName.Contains(searchParam));
            }

            var count = await query.CountAsync(cancellationToken);

            var users = await query
                .OrderBy(e => e.Id)
                .Skip(skip)
                .Take(take)
                .Select(u => new
                {
                    u.Id,
                    u.PhoneNumber,
                    u.FirstName,
                    u.LastName,
                    u.RegisterDate,
                    u.LastLoginDate,
                    u.IsActive
                })
                .ToListAsync(cancellationToken);

            int row = skip;

            var result = users.Select(u =>
            {
                row++;

                return new GetUsersWithPaginationDto
                {
                    Id = u.Id,
                    PhoneNumber = u.PhoneNumber,
                    FullName = u.FirstName + " " + u.LastName,
                    Row = row,
                    IsActive = u.IsActive,
                    RegisterDate = u.RegisterDate.ToString("yyyy/MM/dd"),
                    LastLoginDate = u.LastLoginDate != null
                        ? u.LastLoginDate.Value.ToString("yyyy/MM/dd")
                        : ""
                };
            }).ToList();

            return new PaginatedList<GetUsersWithPaginationDto>(result, count, page);
        }

        public async Task<bool> CheckIsCompleteProfileAsync(int userRef, CancellationToken cancellationToken)
        {
            var thisUser = await context.Users.FindAsync(userRef, cancellationToken);

            if (thisUser != null)
            {
                if (thisUser.FirstName == "" || thisUser.LastName == "")
                {
                    return false;
                }
                return true;
            }
            return false;
        }

    }
}
