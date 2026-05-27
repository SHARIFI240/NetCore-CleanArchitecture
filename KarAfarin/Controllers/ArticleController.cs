using KarAfarin.Application.Features.Blog.Article.Queries.GetLastArticles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KarAfarin.Controllers
{
    public class ArticleController(IMediator mediator) : BaseController
    {
        public async Task<IActionResult> _SelectLastArticles()
        {
            var articles = await mediator.Send(new GetLatestArticleQuery());

            return View(articles);
        }
    }
}
