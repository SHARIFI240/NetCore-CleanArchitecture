using KarAfarin.Application.Common.Interfaces;
using KarAfarin.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Features.Blog.Category.Queries.GetCategories
{
    public record GetCategoriesWithPaginationQuery(int page, bool paginated, string? searchParam) : IRequest<PaginatedList<CategoryGridDto>>;
   
    public class GetCategoriesWithPaginationHandler(ICategoryRepository categoryRepository) : IRequestHandler<GetCategoriesWithPaginationQuery, PaginatedList<CategoryGridDto>>
    {
        public async Task<PaginatedList<CategoryGridDto>> Handle(GetCategoriesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await categoryRepository.GetPagedCategoriesAsync(request.page, request.paginated, request.searchParam, cancellationToken);
        }
    }
}
