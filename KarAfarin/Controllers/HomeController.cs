using KarAfarin.Application.Features.Blog.Article.Queries.GetLastArticles;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KarAfarin.Controllers
{
    public class HomeController(IMediator mediator) : BaseController
    {
        public IActionResult Index()
        {
           
           
            return View();
        }

        [Route("/About-Us")]
        public IActionResult AboutUs() => View();

        [Route("/Contact-Us")]
        public IActionResult ContactUs() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
