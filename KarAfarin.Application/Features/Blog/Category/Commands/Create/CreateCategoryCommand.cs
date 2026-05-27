using KarAfarin.Application.Common.Models.ResultOperation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Features.Blog.Category.Commands.Create
{
    public class CreateCategoryCommand : IRequest<ResultOperation>
    {
        public string Title { get; set; }
    }
}
