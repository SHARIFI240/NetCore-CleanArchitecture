using KarAfarin.Application.Common.Models.ResultOperation;
using KarAfarin.Application.Features.Authentication.Commands.ActiveOrDeactiveUser;
using KarAfarin.Application.Features.Users.Queries.GetUsersWithPagination;
using KarAfarin.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace KarAfarin.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController(IMediator mediator) : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Select(int page, string? searchParam)
        {
            var query = new GetUsersWithPaginationQuery(page, searchParam);
            return Json(ResultOperation.Ok(await mediator.Send(query), "اطلاعات با موفقیت فراخوانی شد"));
        }

        [HttpPost]
        public async Task<IActionResult> ActiveOrDeactive(int userRef)
        {
            return Json(await mediator.Send(new ActiveOrDeactiveUserCommand(userRef)));
        }

    }
}
