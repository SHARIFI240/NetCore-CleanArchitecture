using KarAfarin.Domain.Blog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Infrastructure.Persistence.Configuration.Blog
{
    public class ArticleConfig : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasOne(bb => bb.Category)
                   .WithMany()
                   .HasForeignKey(bb => bb.CategoryRef)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
