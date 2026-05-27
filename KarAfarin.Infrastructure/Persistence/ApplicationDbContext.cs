using KarAfarin.Application.Common.Interfaces;
using KarAfarin.Domain.Authentication.Entities;
using KarAfarin.Domain.Blog.Entities;
using KarAfarin.Domain.Comment.Entities;
using KarAfarin.Domain.Log.Entities;
using KarAfarin.Domain.Media.Entities;
using KarAfarin.Domain.Slider.Entities;
using KarAfarin.Infrastructure.Persistence.Configuration.Authentication;
using KarAfarin.Infrastructure.Persistence.Configuration.Blog;
using KarAfarin.Infrastructure.Persistence.Configuration.Comment;
using KarAfarin.Infrastructure.Persistence.Configuration.Log;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext , IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
: base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ArticleConfig());
            modelBuilder.ApplyConfiguration(new CommentConfig());
            modelBuilder.ApplyConfiguration(new LogConfig());
            modelBuilder.ApplyConfiguration(new UserRolesConfig());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfig());


            base.OnModelCreating(modelBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {

        }


        public DbSet<Article> Article => Set<Article>();
        public DbSet<Category> Category => Set<Category>();
        public DbSet<Comment> Comment => Set<Comment>();
        public DbSet<Log> Log => Set<Log>();
        public DbSet<Slider> Slider => Set<Slider>();
        public DbSet<Media> Media => Set<Media>();

        #region Authentication

        public DbSet<Users> Users => Set<Users>();
        public DbSet<Roles> Roles => Set<Roles>();
        public DbSet<UserRoles> UserRoles => Set<UserRoles>();
        public DbSet<RefreshTokens> RefreshTokens => Set<RefreshTokens>();

        #endregion

    }
}