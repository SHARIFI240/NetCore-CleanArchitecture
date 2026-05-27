using KarAfarin.Application.Common.Models.ResultOperation;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace KarAfarin.Application.Features.Blog.Article.Commands.Create
{
    public class CreateArticleCommand : IRequest<ResultOperation>
    {
        public string Title { get; set; }

        public int CategoryRef { get; set; }

        public string Summary { get; set; }

        public string Content { get; set; }

        public string Keywords { get; set; }

    }
}