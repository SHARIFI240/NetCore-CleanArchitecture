using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace KarAfarin.Application.Features.Blog.Article.Queries.GetArticleById
{
    public class GetArticleByIdDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string Content { get; set; }

        public string Keywords { get; set; }

        public string Cover { get; set; }

        public int CategoryRef { get; set; }


    }
}
