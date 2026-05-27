using KarAfarin.Application.Common.Models;
using KarAfarin.Application.Features.Blog.Article.Queries.GetArticleById;
using KarAfarin.Application.Features.Blog.Article.Queries.GetArticlesPaginatedForAdmin;
using KarAfarin.Application.Features.Blog.Article.Queries.GetLastArticles;
using KarAfarin.Domain.Blog.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Common.Interfaces
{
    public interface IArticleRepository
    {
        Task<int> CreateAsync(Article article, CancellationToken cancellationToken);
        Task<PaginatedList<GetArticlesPaginatedForAdminDto>> GetArticlesPaginatedForAdminAsync(int page, int categoryRef, string? searchParam, CancellationToken cancellationToken);
        Task<Article> GetArticleByIdAsync(int id, CancellationToken cancellationToken);
        Task<int> UpdateArticleAsync(Article article, CancellationToken cancellationToken);
        Task DeleteArticleAsync(int id, CancellationToken cancellationToken);
        Task<List<GetLastArticleDto>> GetLastArticlesAsync();
    }
}
