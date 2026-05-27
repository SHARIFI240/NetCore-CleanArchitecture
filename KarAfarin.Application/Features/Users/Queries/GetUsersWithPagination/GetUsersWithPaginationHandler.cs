using KarAfarin.Application.Common.Interfaces.Authentication;
using KarAfarin.Application.Common.Models;
using KarAfarin.Application.Features.Blog.Category.Queries.GetCategories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Features.Users.Queries.GetUsersWithPagination
{

    public record GetUsersWithPaginationQuery(int page, string? searchParam) : IRequest<PaginatedList<GetUsersWithPaginationDto>>;

    public class GetUsersWithPaginationHandler(IUserRepository userRepository) : IRequestHandler<GetUsersWithPaginationQuery, PaginatedList<GetUsersWithPaginationDto>>
    {
        public async Task<PaginatedList<GetUsersWithPaginationDto>> Handle(GetUsersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await userRepository.GetUsersWithPaginationAsync(request.page, request.searchParam, cancellationToken);
        }
    }
}