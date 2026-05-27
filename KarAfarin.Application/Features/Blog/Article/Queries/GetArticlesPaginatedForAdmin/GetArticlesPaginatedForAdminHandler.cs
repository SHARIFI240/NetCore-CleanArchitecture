using KarAfarin.Application.Common.Interfaces;
using KarAfarin.Application.Common.Models;
using KarAfarin.Application.Features.Blog.Category.Queries.GetCategories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Features.Blog.Article.Queries.GetArticlesPaginatedForAdmin
{
    public record GetArticlesPaginatedForAdminQuery(int page, int categoryRef, string? searchParam) : IRequest<PaginatedList<GetArticlesPaginatedForAdminDto>>;

    public class GetArticlesPaginatedForAdminHandler(IArticleRepository articleRepository) : IRequestHandler<GetArticlesPaginatedForAdminQuery, PaginatedList<GetArticlesPaginatedForAdminDto>>
    {
        public async Task<PaginatedList<GetArticlesPaginatedForAdminDto>> Handle(GetArticlesPaginatedForAdminQuery requet, CancellationToken cancellationToken)
        {
            return await articleRepository.GetArticlesPaginatedForAdminAsync(requet.page, requet.categoryRef,requet.searchParam ,cancellationToken);
        }
    }
}
