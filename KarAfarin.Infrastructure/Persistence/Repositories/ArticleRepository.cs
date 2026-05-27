using KarAfarin.Application.Common.Interfaces;
using KarAfarin.Application.Common.Models;
using KarAfarin.Application.Features.Blog.Article.Queries.GetArticleById;
using KarAfarin.Application.Features.Blog.Article.Queries.GetArticlesPaginatedForAdmin;
using KarAfarin.Application.Features.Blog.Article.Queries.GetLastArticles;
using KarAfarin.Application.Features.Blog.Category.Queries.GetCategories;
using KarAfarin.Domain.Blog.Entities;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Infrastructure.Persistence.Repositories
{
    public class ArticleRepository(ApplicationDbContext context, IMediaRepository mediaRepository) : IArticleRepository
    {
        public async Task<int> CreateAsync(Article article, CancellationToken cancellationToken)
        {
            await context.Article.AddAsync(article, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return article.Id;
        }

        public async Task<PaginatedList<GetArticlesPaginatedForAdminDto>> GetArticlesPaginatedForAdminAsync(int page, int categoryRef, string? searchParam, CancellationToken cancellationToken)
        {
            int take = 20;
            int skip = (page - 1) * take;

            var query = context.Article.AsNoTracking();

            if (searchParam != null)
            {
                query = query.Where(e => e.Title.Contains(searchParam) || e.Summary.Contains(searchParam));
            }

            if (categoryRef > 0)
            {
                query = query.Where(g => g.CategoryRef == categoryRef);
            }

            var count = await query.CountAsync(cancellationToken);

            List<GetArticlesPaginatedForAdminDto> lst = new List<GetArticlesPaginatedForAdminDto>();

            var items = await query
                .OrderBy(e => e.Title)
                .Skip(skip)
                .Take(take)
                .Include(e => e.Category)
                .ToListAsync(cancellationToken);

            int row = skip;

            foreach (var item in items)
            {
                row++;

                GetArticlesPaginatedForAdminDto dto = new GetArticlesPaginatedForAdminDto()
                {
                    Id = item.Id,
                    Title = item.Title,
                    PublishDate = item.PublishDate.Year + "/" + item.PublishDate.Month + "/" + item.PublishDate.Day,
                    Row = row,
                    ViewCount = item.ViewCount,
                    CategoryTitle = item.Category.Title
                };

                lst.Add(dto);

            }

            return new PaginatedList<GetArticlesPaginatedForAdminDto>(lst, count, page);

        }

        public async Task<Article> GetArticleByIdAsync(int id, CancellationToken cancellationToken)
        {
            var thisArticle = await context.Article.FindAsync(id, cancellationToken);

            var coverThisArticle = mediaRepository.GetFilesByEntityAsync(Domain.Media.Enums.MediaEntityTarget.ArticleCover, id, cancellationToken).Result.FirstOrDefault();

            thisArticle.Cover = coverThisArticle != null ? ("/Upload" + coverThisArticle.FilePath + "/" + coverThisArticle.FileName) : "/Upload/NoImage.jpg";


            return thisArticle;
        }

        public async Task<int> UpdateArticleAsync(Article article, CancellationToken cancellationToken)
        {
            context.Article.Update(article);
            await context.SaveChangesAsync(cancellationToken);
            return article.Id;
        }

        public async Task DeleteArticleAsync(int id, CancellationToken cancellationToken)
        {
            var thisArticle = await GetArticleByIdAsync(id, cancellationToken);
            if (thisArticle != null)
            {
                context.Article.Remove(thisArticle);

                await mediaRepository.DeleteMediaAsync(Domain.Media.Enums.MediaEntityTarget.ArticleCover,id, cancellationToken);              
            }
        }

        public async Task<List<GetLastArticleDto>> GetLastArticlesAsync()
        {
            var result = await (from art in context.Article.OrderByDescending(q => q.PublishDate)
                                join cover in context.Media
                                .Where(e => e.MediaEntityTarget == Domain.Media.Enums.MediaEntityTarget.ArticleCover)
                                .OrderByDescending(q => q.CreateDate)
                                on art.Id equals cover.EntityRef
                                select new GetLastArticleDto()
                                {
                                    Url = $"/Article/{art.Id}/{art.Title.Replace(" ","-").Replace("--", "-")}",
                                    Summary = art.Summary,
                                    Title = art.Title,
                                    ViewCount = art.ViewCount,
                                    Cover = $"/Upload/Article/{cover.FileName}",
                                    UpdateDate = art.UpdateDate != null ? $"{art.UpdateDate.Value.Year}/{art.UpdateDate.Value.Month}/{art.UpdateDate.Value.Day}" : $"{art.PublishDate.Year}/{art.PublishDate.Month}/{art.PublishDate.Day}",
                                }).Take(4).AsNoTracking().ToListAsync();
            return result;
        }

    }
}
