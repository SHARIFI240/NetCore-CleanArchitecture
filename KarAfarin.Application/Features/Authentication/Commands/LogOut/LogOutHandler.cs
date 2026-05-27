using KarAfarin.Application.Common.Interfaces.Authentication;
using KarAfarin.Application.Common.Models.ResultOperation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Features.Authentication.Commands.LogOut
{
    public record LogOutCommand(int userId) : IRequest<ResultOperation>;
    public class LogOutHandler(IRefreshTokenRepository refreshTokenRepository) : IRequestHandler<LogOutCommand, ResultOperation>
    {
        public async Task<ResultOperation> Handle(LogOutCommand request, CancellationToken cancellationToken)
        {
            await refreshTokenRepository.DeactivateTokensByUserIdAsync(request.userId, cancellationToken);
            return ResultOperation.Ok(null,"توکن ها با موفقیت غیر فعال شد");
        }
    }
}
