
using System.ComponentModel.DataAnnotations;

namespace KarAfarin.Domain.Authentication.Entities
{
    public class Users
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string? Email { get; set; }

        public bool EmailConfrimed { get; set; }

        public bool IsActive { get; set; }

        public string? PasswordHash { get; set; }

        public string? LoginCodeHash { get; set; }

        public int FailedLoginAttempts { get; set; }

        public DateTime RegisterDate { get; set; }

        public DateTime? LastLoginDate { get; set; }

    }
}