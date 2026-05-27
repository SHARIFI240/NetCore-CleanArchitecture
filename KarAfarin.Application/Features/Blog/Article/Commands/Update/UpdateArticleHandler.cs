using KarAfarin.Application.Common.Interfaces;
using KarAfarin.Application.Common.Models.ResultOperation;
using MediatR;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace KarAfarin.Application.Features.Blog.Article.Commands.Update
{
    public class UpdateArticleHandler(IArticleRepository articleRepository) : IRequestHandler<UpdateArticleCommand, ResultOperation>
    {
        public async Task<ResultOperation> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
        {
            var thisArticle = await articleRepository.GetArticleByIdAsync(request.Id, cancellationToken);

            if (thisArticle != null)
            {

                thisArticle.Title = request.Title;
                thisArticle.CategoryRef = request.CategoryRef;
                thisArticle.Keywords = request.Keywords;
                thisArticle.Summary = request.Summary;
                thisArticle.Content = request.Content;
                thisArticle.UpdateDate = DateTime.Now;

                var result = await articleRepository.UpdateArticleAsync(thisArticle, cancellationToken);
                return ResultOperation.Ok(result,"ویرایش مطلب با موفقیت انجام شد");

            }

            return ResultOperation.NotFound("مقاله ای برای ویرایش پیدا نشد");
            
        }
    }
}
