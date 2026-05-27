using KarAfarin.Application.Common.Models;
using KarAfarin.Application.Features.Users.Queries.GetUsersWithPagination;
using KarAfarin.Domain.Authentication.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Common.Interfaces.Authentication
{
    public interface IUserRepository
    {
        Task AddUserAsync(Users user, CancellationToken cancellationToken);
        Task<bool> CheckDuplicatedUserNameAsync(string userName, CancellationToken cancellationToken);
        Task<Users?> GetUserByIdAsync(int id, CancellationToken cancellationToken);
        Task UpdateUserAsync(Users user, CancellationToken cancellationToken);
        Task UpdateLoginAttemptsAsync(int userId, int attempts, DateTime? lastLogin, CancellationToken cancellationToken);
        Task<Users?> GetUserByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken);
        Task<PaginatedList<GetUsersWithPaginationDto>> GetUsersWithPaginationAsync(int page, string? searchParam, CancellationToken cancellationToken);
        Task<bool> CheckIsCompleteProfileAsync(int userRef, CancellationToken cancellationToken);
    }
}
