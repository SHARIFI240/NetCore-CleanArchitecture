using KarAfarin.Application.Common.Models.ResultOperation;
using KarAfarin.Application.Features.Blog.Article.Commands.Create;
using KarAfarin.Application.Features.Blog.Article.Commands.Delete;
using KarAfarin.Application.Features.Blog.Article.Commands.Update;
using KarAfarin.Application.Features.Blog.Article.Queries.GetArticleById;
using KarAfarin.Application.Features.Blog.Article.Queries.GetArticlesPaginatedForAdmin;
using KarAfarin.Application.Features.Blog.Category.Commands.Update;
using KarAfarin.Application.Features.Blog.Category.Queries.GetCategories;
using KarAfarin.Application.Features.Files.Commands.UploadFile;
using KarAfarin.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Logging;

namespace KarAfarin.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ArticleController(IMediator mediator) : BaseController
    {
        public async Task<IActionResult> Index()
        {
            var query = new GetCategoriesWithPaginationQuery(0, false, null);
            var result = await mediator.Send(query);

            ViewBag.categories = result.Items.Select(g => new { g.Id, g.Title }).ToList();


            return View();
        }

        #region Create

        public async Task<IActionResult> _Create()
        {
            var query = new GetCategoriesWithPaginationQuery(0, false, null);
            var result = await mediator.Send(query);

            ViewBag.categories = result.Items.Select(g => new { g.Id, g.Title }).ToList();

            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateArticleCommand cmd)
        {
            if (ModelState.IsValid)
            {
                var result = await mediator.Send(cmd);

                return Json(result);
            }

            return Json(ResultOperation.ValidationError("خطای اعتبارسنجی"));
        }

        #endregion

        #region Select

        public async Task<IActionResult> Select(int page, int categoryRef, string? searchParam)
        {
            var query = new GetArticlesPaginatedForAdminQuery(page, categoryRef, searchParam);


            return Json(ResultOperation.Ok(await mediator.Send(query), "فراخوانی مقالات با موفقیت انجام شد"));
        }

        #endregion

        #region Update

        public async Task<IActionResult> _Edit(int id)
        {
            var thisArticle = await mediator.Send(new GetArticleByIdQuery(id));

            UpdateArticleCommand cmd = new UpdateArticleCommand()
            { 
                Id = id,
                CategoryRef = thisArticle.CategoryRef,
                Content = thisArticle.Content,
                Keywords = thisArticle.Keywords,
                Summary = thisArticle.Summary,
                Title = thisArticle.Title,
                CoverImage = thisArticle.Cover,
            };


            var query = new GetCategoriesWithPaginationQuery(0, false, null);
            var result = await mediator.Send(query);
            ViewBag.categories = result.Items.Select(g => new { g.Id, g.Title }).ToList();


            return PartialView(cmd);    
        }

        public async Task<IActionResult> Update([FromForm] UpdateArticleCommand cmd)
        {
            if (ModelState.IsValid)
            {
                return Json(await mediator.Send(cmd));
            }
            return Json(ResultOperation.ValidationError("خطا در اعتبارسنجی"));
        }

        #endregion

        #region Delete

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            return Json(await mediator.Send(new DeleteArticleCommand(id)));
        }

        #endregion

        [HttpPost]
        public async Task<IActionResult> UploadPostImage(bool articleCover, int? articleId = null)
        {
            var files = Request.Form.Files;

            List<UploadFileCommand> lst = new List<UploadFileCommand>();

            var cmd = new UploadFileCommand();

            if (articleCover)
                cmd = new UploadFileCommand()
                {
                    EntityRef = articleId,
                    MediaEntityTarget = Domain.Media.Enums.MediaEntityTarget.ArticleCover,
                    Files = new List<FileModel>(),
                    Path = "/Article"
                };
            else
                cmd = new UploadFileCommand()
                {
                    EntityRef = null,
                    MediaEntityTarget = Domain.Media.Enums.MediaEntityTarget.ArticleContent,
                    Files = new List<FileModel>(),
                    Path = "/Article/Content"
                };



            foreach (var file in files)
            {
                using var ms = new MemoryStream();             
                await file.CopyToAsync(ms);
                cmd.Files.Add(new FileModel() { 
                    File = ms.ToArray(),
                    FileName = file.FileName,
                });
            }


            return Json(await mediator.Send(cmd));
        }
    }
}
