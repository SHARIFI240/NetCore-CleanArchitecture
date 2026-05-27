using KarAfarin.Application.Common.Models.ResultOperation;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KarAfarin.Application.Features.Blog.Category.Commands.Update
{
    public class UpdateCategoryCommand : IRequest<ResultOperation>
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }
}
