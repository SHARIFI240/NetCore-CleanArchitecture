using KarAfarin.Application.Features.Blog.Category.Queries.GetCategories;
using KarAfarin.Application.Common.Interfaces;
using KarAfarin.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Features.Blog.Category.Queries.GetCategoryById
{
    public record GetCategoryByIdQuery(int id) : IRequest<CategoryDetailDto>;


    public class GetCategoryByIdHandler(ICategoryRepository categoryRepository) : IRequestHandler<GetCategoryByIdQuery, CategoryDetailDto>
    {
        public async Task<CategoryDetailDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var thisCategory = await categoryRepository.GetCategoryByIdAsync(request.id, cancellationToken);

            return new CategoryDetailDto() { 
                Id = thisCategory.Id,
                Title = thisCategory.Title,
            };
        }
    }
}
