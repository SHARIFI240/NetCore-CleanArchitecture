using KarAfarin.Application.Common.Interfaces.Authentication;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Features.Authentication.Queries.GetUserById
{
    public record GetUserByIdQuery(int userRef) : IRequest<GetUserByIdDto>;

    public class GetUserByIdHandler(IUserRepository userRepository) : IRequestHandler<GetUserByIdQuery, GetUserByIdDto>
    {
        public async Task<GetUserByIdDto> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetUserByIdAsync(query.userRef, cancellationToken);


            return new GetUserByIdDto() {
                
                Email = user.Email,
                EmailConfrimed = user.EmailConfrimed,
                FirstName = user.FirstName,
                Id = user.Id,
                LastLoginDate = user.LastLoginDate,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Profile = "",
                RegisterDate = user.RegisterDate
            
            };
        }
    }
}
