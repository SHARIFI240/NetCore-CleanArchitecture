using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Features.Blog.Article.Queries.GetArticlesPaginatedForAdmin
{
    public class GetArticlesPaginatedForAdminDto
    {
        public int Id { get; set; }

        public int Row { get; set; }

        public string Title { get; set; }

        public string PublishDate { get; set; }

        public int ViewCount { get; set; }

        public string CategoryTitle { get; set; }
    }
}
