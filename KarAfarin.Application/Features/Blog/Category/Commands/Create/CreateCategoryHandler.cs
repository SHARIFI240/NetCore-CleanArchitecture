using KarAfarin.Application.Common.Interfaces;
using KarAfarin.Application.Common.Models.ResultOperation;
using KarAfarin.Domain.Blog.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Features.Blog.Category.Commands.Create
{
    public class CreateCategoryHandler(ICategoryRepository categoryRepository) : IRequestHandler<CreateCategoryCommand, ResultOperation>
    {
        public async Task<ResultOperation> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {

            if (await categoryRepository.IsNameUniqueAsync(request.Title, cancellationToken)) {


                var category = new Domain.Blog.Entities.Category() { Title = request.Title };

                var result = await categoryRepository.CreateCategoryAsync(category, cancellationToken);

                return ResultOperation.Ok(result, "ثبت دسته بندی با موفقیت انجام شد");
            }


            return ResultOperation.Duplicate("اطلاعات ثبت شده تکراری می باشد");
        }
    }
}
