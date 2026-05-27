using KarAfarin.Domain.Media.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KarAfarin.Domain.Media.Entities
{
    public class Media
    {
        [Key]
        public int Id { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public DateTime CreateDate { get; set; }

        public MediaEntityTarget MediaEntityTarget { get; set; }

        public int? EntityRef { get; set; }
    }
}