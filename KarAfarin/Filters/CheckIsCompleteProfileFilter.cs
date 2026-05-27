using KarAfarin.Application.Features.Authentication.Commands.CheckIsCompleteProfile;
using KarAfarin.Domain.Authentication.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace KarAfarin.Filters
{
    public class CheckIsCompleteProfileFilter(IMediator mediator) : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                await next();
                return;
            }

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
            {
                await next();
                return;
            }

            var userId = int.Parse(userIdClaim);

            var hasCmpleted = await mediator.Send(new CheckIsCompleteProfileCommand(userId));

            var action = context.RouteData.Values["action"]?.ToString();

            if (action != "EditProfile" && action != "UpdateProfile" && !hasCmpleted)
            {
                context.Result = new RedirectToActionResult("EditProfile", "Account", routeValues: new { area = "" });

                return;
            }

            await next();
        }
    }
}
