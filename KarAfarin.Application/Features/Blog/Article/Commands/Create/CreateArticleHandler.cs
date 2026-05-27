using KarAfarin.Application.Common.Interfaces;
using KarAfarin.Application.Common.Models.ResultOperation;
using KarAfarin.Domain.Blog.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KarAfarin.Application.Features.Blog.Article.Commands.Create
{
    public class CreateArticleHandler(IArticleRepository articleRepository) : IRequestHandler<CreateArticleCommand, ResultOperation>
    {
        public async Task<ResultOperation> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
        {
            Domain.Blog.Entities.Article article = new Domain.Blog.Entities.Article()
            { 
                PublishDate = DateTime.Now,
                Content = request.Content,
                Title = request.Title,
                Keywords = request.Keywords,
                CategoryRef = request.CategoryRef,
                Summary = request.Summary,
                ViewCount = 0,            
            };

            var result = await articleRepository.CreateAsync(article, cancellationToken);


            return ResultOperation.Ok(result, "ذخیره مطلب با موفقیت انجام شد");
        }

    }
}
