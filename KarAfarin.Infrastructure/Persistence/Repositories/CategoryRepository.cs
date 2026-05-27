using KarAfarin.Application.Common.Interfaces;
using KarAfarin.Application.Common.Models;
using KarAfarin.Application.Features.Blog.Category.Queries.GetCategories;
using KarAfarin.Domain.Blog.Entities;
using KarAfarin.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Nodes;

namespace KarAfarin.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository(ApplicationDbContext context) : ICategoryRepository
    {
        public async Task<int> CreateCategoryAsync(Category category, CancellationToken cancellationToken)
        {
            await context.Category.AddAsync(category, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return category.Id;
        }

        public async Task<bool> IsNameUniqueAsync(string title, CancellationToken cancellationToken)
        {
            // چک کردن وجود نام تکراری در دیتابیس
            return !await context.Category.AnyAsync(x => x.Title == title, cancellationToken);
        }

        public async Task<PaginatedList<CategoryGridDto>> GetPagedCategoriesAsync(int page, bool paginated, string? searchParam, CancellationToken cancellationToken)
        {
            int take = 20;
            int skip = (page - 1) * take;


            var query = context.Category.AsNoTracking();

            if (searchParam != null)
            {
                query = query.Where(e => e.Title.Contains(searchParam));
            }

            var count = await query.CountAsync(cancellationToken);

            List<CategoryGridDto> lst = new List<CategoryGridDto>();

            var items = new List<Category>();

            if (paginated)
            {
                items = await query
                .OrderBy(e => e.Title)
                .Skip(skip)
                .Take(take)
                .ToListAsync(cancellationToken);
            }
            else
            {
                items = await query
                .OrderBy(e => e.Title)
                .ToListAsync(cancellationToken);
            }

            int row = skip;

            foreach (var item in items)
            {
                row++;

                CategoryGridDto model = new CategoryGridDto()
                { 
                    Row = row,
                    Title = item.Title, 
                    Id = item.Id
                };

                lst.Add(model);

            }

            return new PaginatedList<CategoryGridDto>(lst, count, page);

        }

        public async Task<Category> GetCategoryByIdAsync(int id, CancellationToken cancellationToken)
        {
            var thisCategory = await context.Category.FindAsync(id, cancellationToken);

            if (thisCategory != null)
            {
                return thisCategory;
            }
            else {
                return new Category();            
            }


        }

        public async Task<int> UpdateCategoryAsync(Category category, CancellationToken cancellationToken)
        {
            context.Category.Update(category);
            await context.SaveChangesAsync(cancellationToken);
            return category.Id;
        }

        public async Task<short> DeleteCategoryAsync(int id, CancellationToken cancellationToken)
        {

            var thisCategory = await GetCategoryByIdAsync(id, cancellationToken);
            if (thisCategory != null)
            {
    
                var hasRefrence = await context.HasDependenciesAsync<Category>(id, cancellationToken);
                if (!hasRefrence)
                {

                    context.Category.Remove(thisCategory);
                    await context.SaveChangesAsync(cancellationToken);
                    return 1;
                }
                return -1;
            }

            return 0;
        }
    }
}
