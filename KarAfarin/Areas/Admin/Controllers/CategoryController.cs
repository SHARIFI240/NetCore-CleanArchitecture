using KarAfarin.Application.Common.Models.ResultOperation;
using KarAfarin.Application.Features.Blog.Category.Commands.Create;
using KarAfarin.Application.Features.Blog.Category.Commands.Delete;
using KarAfarin.Application.Features.Blog.Category.Commands.Update;
using KarAfarin.Application.Features.Blog.Category.Queries.GetCategories;
using KarAfarin.Application.Features.Blog.Category.Queries.GetCategoryById;
using KarAfarin.Controllers;
using KarAfarin.ViewModels.Admin.Category;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace KarAfarin.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController(IMediator mediator) : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        #region Create

        public IActionResult _Create() => PartialView();

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateUpdateCategory vModel)
        {

            if (ModelState.IsValid)
            {
                var result = await mediator.Send(new CreateCategoryCommand()
                {
                    Title = vModel.Title,
                });

                return Json(result);

            }

            return Json(ResultOperation.ValidationError("خطای اعتبار سنجی"));
        }

        #endregion

        #region Update

        public async Task<IActionResult> _Edit(int id)
        {
            var result = await mediator.Send(new GetCategoryByIdQuery(id));

            var viewModel = new CreateUpdateCategory()
            {
                Id = result.Id,
                Title = result.Title,
            };

            return PartialView(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] CreateUpdateCategory vModel)
        {
            if (ModelState.IsValid)
            {
                var result = await mediator.Send(new UpdateCategoryCommand()
                {
                    Title = vModel.Title,
                    Id = vModel.Id
                });

                return Json(result);
            }

            return Json(ResultOperation.ValidationError("خطای  اعتبار سنجی"));
        }

        #endregion

        #region Select


        public async Task<IActionResult> Select(int page, string? searchParam)
        {
            var query = new GetCategoriesWithPaginationQuery(page, true, searchParam);

            return Json(ResultOperation.Ok(await mediator.Send(query), "فراخوانی دسته بندی ها با موفقیت انجام شد"));
        }

        #endregion

        #region Delete

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0) {

                var result = await mediator.Send(new DeleteCategoryQuery(id));

                return Json(result);
            }
            return Json(ResultOperation.Fail("حذف دسته بندی با خطا مواجه شد"));
        }

        #endregion
    }
}
