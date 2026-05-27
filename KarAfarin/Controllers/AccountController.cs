
using KarAfarin.Application.Common.Models.Authentication;
using KarAfarin.Application.Features.Authentication.Commands.GeerateNewVerifyCode;
using KarAfarin.Application.Features.Authentication.Commands.LoginWithPassword;
using KarAfarin.Application.Features.Authentication.Commands.LogOut;
using KarAfarin.Application.Features.Authentication.Commands.SendVerificationCode;
using KarAfarin.Application.Features.Authentication.Commands.UpdateProfile;
using KarAfarin.Application.Features.Authentication.Commands.VerifyCode;
using KarAfarin.Application.Features.Authentication.Queries.GetUserById;
using KarAfarin.ViewModels.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Runtime.CompilerServices;

namespace KarAfarin.Controllers
{
    public class AccountController(IMediator mediator) : BaseController
    {

        public async Task<IActionResult> LoginWithPassword()
        {
            await ExitAccount();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginWithPassword([FromForm] LoginWithPasswordCommand cmd)
        {
            if (ModelState.IsValid)
            {
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

                cmd.IpAddress = ip;

                var result = await mediator.Send(cmd);

                if (result.ErrorMessage == null)
                {
                    SetAuthCookies(result);
                    return Redirect(result.ReturnToAddress);
                }
                else
                {
                    ViewBag.errorMessage = result.ErrorMessage;
                    return View("LoginWithPassword", new LoginWithPasswordCommand() { PhoneNumber = cmd.PhoneNumber });
                }

            }

            return View();
        }

        public async Task<IActionResult> Login()
        {
            await ExitAccount();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendVerificationCode([FromForm] SendVerificationCodeCommand cmd)
        {
            if (ModelState.IsValid)
            {
                var result = await mediator.Send(cmd);

                if (result.Status == Application.Common.Models.ResultOperation.ResultStatus.Success)
                {

                    ViewBag.phoneNumber = result.Data;

                    return View("VerifyCode");
                }
                else
                {

                    return View("Login", result.Message);
                }

            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyCode([FromForm] VerifyCodeCommand cmd)
        {
            if (ModelState.IsValid)
            {
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

                cmd.IpAddress = ip;

                var result = await mediator.Send(cmd);

                if (result.ErrorMessage == null)
                {
                    SetAuthCookies(result);
                    return Redirect(result.ReturnToAddress);
                }
                else
                {
                    ViewBag.errorMessage = result.ErrorMessage;
                    return View("VerifyCode", new VerifyCodeCommand() { PhoneNumber = cmd.PhoneNumber });
                }



            }

            return Redirect("/Home/Index");
        }

        [HttpPost]
        public async Task<IActionResult> GetNewCode(string phoneNumber)
        {
            return Json(await mediator.Send(new GenerateNewVerifyCodeCommand(phoneNumber)));
        }

        public IActionResult AccessDenied() => View();

        public async Task<IActionResult> LogOut()
        {
            await ExitAccount();

            return Redirect("/Home/Index");
        }

        private async Task ExitAccount()
        {
            await mediator.Send(new LogOutCommand(GetCurrentUserId()));

            // حذف کوکی JWT (که معمولاً "AccessToken" نام دارد)
            if (Request.Cookies.ContainsKey("access_token"))
            {
                Response.Cookies.Delete("access_token");
            }


            // حذف هرگونه کوکی احراز هویت (اگر CookieAuthentication استفاده شده)
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        }

        private void SetAuthCookies(TokenResponse token)
        {
            Response.Cookies.Append("access_token",
               token.AccessToken,
               new CookieOptions
               {
                   HttpOnly = true,
                   Secure = true,
                   SameSite = SameSiteMode.Strict,
                   Expires = DateTimeOffset.Now.AddSeconds(token.ExpiresIn)
               }
            );

            Response.Cookies.Append("refresh_token",
                token.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.Now.AddDays(30)
                }
            );

        }

        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var user = await mediator.Send(new GetUserByIdQuery(GetCurrentUserId()));

            EditProfileViewModel vModel = new EditProfileViewModel()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName
            };

            ViewBag.phoneNumber = user.PhoneNumber;

            return View(vModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileCommand cmd)
        {
            if (ModelState.IsValid)
            {
                cmd.Id = GetCurrentUserId();


                await mediator.Send(cmd);
            }
            return Redirect("/Home/Index");
        }

    }
}
