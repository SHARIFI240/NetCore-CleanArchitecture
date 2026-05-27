using KarAfarin.Application.Common.Interfaces;
using KarAfarin.Application.Common.Models.ResultOperation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Features.Blog.Article.Queries.GetLastArticles
{
    public record GetLatestArticleQuery : IRequest<List<GetLastArticleDto>>;

    public class GetLastArticleHandler(IArticleRepository articleRepository) : IRequestHandler<GetLatestArticleQuery, List<GetLastArticleDto>>
    {
        public async Task<List<GetLastArticleDto>> Handle(GetLatestArticleQuery request, CancellationToken cancellationToken)
        {
            return await articleRepository.GetLastArticlesAsync();
        }
    }
}
