using KarAfarin.Application.Common.Interfaces.Authentication;
using KarAfarin.Application.Common.Models.ResultOperation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net.Quic;
using System.Text;

namespace KarAfarin.Application.Features.Authentication.Commands.CheckIsCompleteProfile
{

    public record CheckIsCompleteProfileCommand(int userRef) : IRequest<bool>;

    public class CheckIsCompleteProfileHandler(IUserRepository userRepository) : IRequestHandler<CheckIsCompleteProfileCommand, bool>
    {
        public async Task<bool> Handle(CheckIsCompleteProfileCommand request, CancellationToken cancellationToken)
        {
            return await userRepository.CheckIsCompleteProfileAsync(request.userRef, cancellationToken);
        }
    }
}
