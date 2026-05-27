using KarAfarin.Application.Common.Interfaces.Authentication;
using KarAfarin.Domain.Authentication.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KarAfarin.Infrastructure.Persistence.Repositories.Authentication
{
    public class RoleRepository(ApplicationDbContext context) : IRoleRepository
    {
        public async Task<Roles?> GetByNameAsync(string name, CancellationToken cancellationToken) =>
            await context.Roles.FirstOrDefaultAsync(r => r.Name == name, cancellationToken);


        public async Task<List<Roles>> GetRolesByUserIdAsync(int userId, CancellationToken cancellationToken) =>
       (await context.UserRoles
           .Where(ur => ur.UserId == userId)
           .Include(ur => ur.Role)
           .ToListAsync(cancellationToken)).Select(ur => ur.Role).ToList();

        public async Task AddRolesToUserAsync(string[] rolesSelected, Users user, CancellationToken cancellationToken)
        {
            var roles = await context.Roles.Where(g => rolesSelected.Contains(g.Name))
                .ToListAsync();

            foreach (var item in roles)
            {
                UserRoles userRole = new UserRoles()
                { 
                    Role = item,
                    User = user
                };

                context.UserRoles.Add(userRole);
            }

            await context.SaveChangesAsync(cancellationToken);

        }

    }
}