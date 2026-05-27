using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Features.Users.Queries.GetUsersWithPagination
{
    public class GetUsersWithPaginationDto
    {
        public int Id { get; set; }

        public int Row { get; set; }

        public string RegisterDate { get; set; }

        public string PhoneNumber { get; set; }

        public string FullName { get; set; }

        public string LastLoginDate { get; set; }

        public bool IsActive { get; set; }
    }
}