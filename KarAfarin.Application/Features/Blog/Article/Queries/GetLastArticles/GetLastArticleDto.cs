using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace KarAfarin.Application.Features.Blog.Article.Queries.GetLastArticles
{
    public class GetLastArticleDto
    {
        public string Url { get; set; }
        
        public string Title { get; set; }

        public string Summary { get; set; }

        public string Cover { get; set; }

        public string UpdateDate { get; set; }

        public int ViewCount { get; set; }
    }
}
