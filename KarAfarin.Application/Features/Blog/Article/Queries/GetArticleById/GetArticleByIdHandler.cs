using KarAfarin.Application.Common.Interfaces;
using KarAfarin.Application.Common.Models;
using KarAfarin.Application.Features.Blog.Article.Queries.GetArticlesPaginatedForAdmin;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Features.Blog.Article.Queries.GetArticleById
{
    public record GetArticleByIdQuery(int id) : IRequest<GetArticleByIdDto>;


    public class GetArticleByIdHandler(IArticleRepository articleRepository) : IRequestHandler<GetArticleByIdQuery, GetArticleByIdDto>
    {
        public async Task<GetArticleByIdDto> Handle(GetArticleByIdQuery query, CancellationToken cancellationToken)
        {
            var thisArticle = await articleRepository.GetArticleByIdAsync(query.id, cancellationToken);

            GetArticleByIdDto dto = new GetArticleByIdDto()
            { 
                CategoryRef = thisArticle.CategoryRef,
                Content = thisArticle.Content,
                Cover = thisArticle.Cover,
                Keywords = thisArticle.Keywords,
                Id = thisArticle.Id,
                Summary = thisArticle.Summary,
                Title = thisArticle.Title
            };

            return dto;
        }
    }
}
