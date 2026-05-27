using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KarAfarin.Domain.Authentication.Entities
{
    public class UserRoles
    {
        [Key]
        public int Id { get; set; }

        public virtual Users User { get; set; }
        public int UserId { get; set; }


        public virtual Roles Role { get; set; }
        public int RoleId { get; set; }


    }
}
