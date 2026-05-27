using KarAfarin.Application.Common.Interfaces.Authentication;
using KarAfarin.Application.Common.Models.ResultOperation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Features.Authentication.Commands.UpdateProfile
{
    public class UpdateProfileHandler(IUserRepository userRepository) : IRequestHandler<UpdateProfileCommand, ResultOperation>
    {
        public async Task<ResultOperation> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var thisUser = await userRepository.GetUserByIdAsync(request.Id, cancellationToken);
            if (thisUser != null)
            {
                thisUser.FirstName = request.FirstName;
                thisUser.LastName = request.LastName;
                thisUser.Email = request.Email;

                await userRepository.UpdateUserAsync(thisUser, cancellationToken);

                return ResultOperation.Ok(null, "ویرایش اطلاعات با موفقیت انجام شد");
            }

            return ResultOperation.Fail("ویرایش اطلاعات با خطا مواجه شد");
        }
    }
}
