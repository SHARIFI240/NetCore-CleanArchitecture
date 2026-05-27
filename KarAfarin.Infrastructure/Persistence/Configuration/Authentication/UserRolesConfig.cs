using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Infrastructure.Persistence.Configuration.Authentication
{
    public class UserRolesConfig : IEntityTypeConfiguration<Domain.Authentication.Entities.UserRoles>
    {
        public void Configure(EntityTypeBuilder<Domain.Authentication.Entities.UserRoles> builder)
        {
            builder.HasOne(bb => bb.User)
                   .WithMany()
                   .HasForeignKey(bb => bb.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(bb => bb.Role)
               .WithMany()
               .HasForeignKey(bb => bb.RoleId)
               .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
