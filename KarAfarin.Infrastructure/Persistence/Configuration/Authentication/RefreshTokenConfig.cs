using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Infrastructure.Persistence.Configuration.Authentication
{
    public class RefreshTokenConfig : IEntityTypeConfiguration<Domain.Authentication.Entities.RefreshTokens>
    {
        public void Configure(EntityTypeBuilder<Domain.Authentication.Entities.RefreshTokens> builder)
        {
            builder.HasOne(bb => bb.User)
                   .WithMany()
                   .HasForeignKey(bb => bb.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
