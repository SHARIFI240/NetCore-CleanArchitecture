using KarAfarin.Domain.Blog.Entities;
using KarAfarin.Domain.Comment.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Infrastructure.Persistence.Configuration.Comment
{
    public class CommentConfig : IEntityTypeConfiguration<Domain.Comment.Entities.Comment>
    {
        public void Configure(EntityTypeBuilder<Domain.Comment.Entities.Comment> builder)
        {
            builder.HasOne(bb => bb.User)
                   .WithMany()
                   .HasForeignKey(bb => bb.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
