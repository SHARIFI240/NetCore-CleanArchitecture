using KarAfarin.Application.Common.Models.ResultOperation;
using MediatR;

namespace KarAfarin.Application.Features.Blog.Article.Commands.Update
{
    public class UpdateArticleCommand : IRequest<ResultOperation>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int CategoryRef { get; set; }

        public string Summary { get; set; }

        public string Content { get; set; }

        public string Keywords { get; set; }

        public string? CoverImage { get; set; }
    }
}