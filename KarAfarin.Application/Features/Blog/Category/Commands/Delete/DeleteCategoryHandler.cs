using KarAfarin.Application.Features.Blog.Category.Queries.GetCategoryById;
using KarAfarin.Application.Common.Interfaces;
using KarAfarin.Application.Common.Models.ResultOperation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Features.Blog.Category.Commands.Delete
{

    public record DeleteCategoryQuery(int id) : IRequest<ResultOperation>;

    public class DeleteCategoryHandler(ICategoryRepository categoryRepository) : IRequestHandler<DeleteCategoryQuery, ResultOperation>
    {
        public async Task<ResultOperation> Handle(DeleteCategoryQuery request, CancellationToken cancellationToken)
        {
            var result = await categoryRepository.DeleteCategoryAsync(request.id, cancellationToken);

            if (result == 0)
                return ResultOperation.Fail("حذف دسته بندی با خطا مواجه شد");
            else if(result == -1)
                return ResultOperation.Fail("به دلیل وجود ارتباط ،امکان حذف این دسته بندی وجود ندارد ");
            else
                return ResultOperation.Ok(null,"حذف دسته بندی با موفقیت انجام شد");

        }
    }
}
