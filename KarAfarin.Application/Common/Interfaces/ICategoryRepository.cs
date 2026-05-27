using KarAfarin.Application.Common.Models;
using KarAfarin.Application.Features.Blog.Category.Queries.GetCategories;
using KarAfarin.Domain.Blog.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Common.Interfaces
{
    public interface ICategoryRepository
    {
        Task<int> CreateCategoryAsync(Category category, CancellationToken cancellationToken);
        Task<bool> IsNameUniqueAsync(string title, CancellationToken cancellationToken);
        Task<PaginatedList<CategoryGridDto>> GetPagedCategoriesAsync(int page, bool paginated, string? searchParam, CancellationToken cancellationToken);
        Task<Category> GetCategoryByIdAsync(int id, CancellationToken cancellationToken);
        Task<int> UpdateCategoryAsync(Category category, CancellationToken cancellationToken);
        Task<short> DeleteCategoryAsync(int id, CancellationToken cancellationToken);
    }
}