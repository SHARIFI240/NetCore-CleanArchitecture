using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KarAfarin.Domain.Authentication.Entities
{
    public class Roles
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
