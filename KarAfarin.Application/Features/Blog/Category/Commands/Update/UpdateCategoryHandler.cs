using KarAfarin.Application.Features.Blog.Category.Commands.Create;
using KarAfarin.Application.Common.Interfaces;
using KarAfarin.Application.Common.Models.ResultOperation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Features.Blog.Category.Commands.Update
{
    public class UpdateCategoryHandler(ICategoryRepository categoryRepository) : IRequestHandler<UpdateCategoryCommand, ResultOperation>
    {
        public async Task<ResultOperation> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var thisCategory = await categoryRepository.GetCategoryByIdAsync(request.Id, cancellationToken);

            if (thisCategory != null)
            {
                thisCategory.Title = request.Title;

                var result = await categoryRepository.UpdateCategoryAsync(thisCategory, cancellationToken);

                return ResultOperation.Ok(result, "ویرایش دسته بندی با موفقیت انجام شد");
            }

            return ResultOperation.NotFound("هیچ دسته بندی پیدا نشد");
        }
    }
}
