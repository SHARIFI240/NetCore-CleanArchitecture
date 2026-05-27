using KarAfarin.Domain.Authentication.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Common.Interfaces.Authentication
{
    public interface IRoleRepository
    {
        Task<Roles?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task<List<Roles>> GetRolesByUserIdAsync(int userId, CancellationToken cancellationToken);
        Task AddRolesToUserAsync(string[] rolesSelected, Users user, CancellationToken cancellationToken);
    }
}