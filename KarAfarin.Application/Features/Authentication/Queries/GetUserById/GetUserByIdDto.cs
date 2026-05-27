using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Channels;

namespace KarAfarin.Application.Features.Authentication.Queries.GetUserById
{
    public class GetUserByIdDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public bool EmailConfrimed { get; set; }

        public DateTime RegisterDate { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public string Profile { get; set; }
    }
}
