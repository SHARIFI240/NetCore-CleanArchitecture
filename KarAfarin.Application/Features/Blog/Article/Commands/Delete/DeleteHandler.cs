using KarAfarin.Application.Common.Interfaces;
using KarAfarin.Application.Common.Models.ResultOperation;
using KarAfarin.Application.Features.Blog.Article.Queries.GetArticleById;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Features.Blog.Article.Commands.Delete
{
    public record DeleteArticleCommand(int id) : IRequest<ResultOperation>;

    public class DeleteHandler(IArticleRepository articleRepository) : IRequestHandler<DeleteArticleCommand, ResultOperation>
    {
        public async Task<ResultOperation> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
        {
            await articleRepository.DeleteArticleAsync(request.id, cancellationToken);

            return ResultOperation.Ok(null,"حذف مطلب با موفقیت انجام شد");
        }
    }
}
