using KarAfarin.Domain.Authentication.Entities;
using KarAfarin.Domain.Blog.Entities;
using KarAfarin.Domain.Comment.Entities;
using KarAfarin.Domain.Log.Entities;
using KarAfarin.Domain.Media.Entities;
using KarAfarin.Domain.Slider.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Article> Article { get; }
        DbSet<Category> Category { get; }
        DbSet<Comment> Comment { get; }
        DbSet<Log> Log { get; }
        DbSet<Slider> Slider { get; }
        DbSet<Media> Media { get; }

        #region Authentication

        DbSet<Users> Users { get; }
        DbSet<Roles> Roles { get; }
        DbSet<UserRoles> UserRoles { get; }
        DbSet<RefreshTokens> RefreshTokens { get; }

        #endregion

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}
