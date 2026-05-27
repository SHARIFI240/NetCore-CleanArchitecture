using KarAfarin.Application.Common.Interfaces.Authentication;
using KarAfarin.Application.Common.Models.ResultOperation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Features.Authentication.Commands.ActiveOrDeactiveUser
{
    public record ActiveOrDeactiveUserCommand(int userId) : IRequest<ResultOperation>;
    public class ActiveOrDeactiveUserHandler(IUserRepository userRepository) : IRequestHandler<ActiveOrDeactiveUserCommand, ResultOperation>
    {
        public async Task<ResultOperation> Handle(ActiveOrDeactiveUserCommand request, CancellationToken cancellationToken)
        {
            var thisUser = await userRepository.GetUserByIdAsync(request.userId, cancellationToken);

            if (thisUser != null)
            {
                if (thisUser.IsActive)
                    thisUser.IsActive = false;
                else 
                    thisUser.IsActive = true;

                await userRepository.UpdateUserAsync(thisUser, cancellationToken);

                return ResultOperation.Ok(null ,"عملیات با موفقیت انجام شد");
            }
            return ResultOperation.Fail("هنگام انجام عملیات خطایی رخ داد");
        }
    }
}
