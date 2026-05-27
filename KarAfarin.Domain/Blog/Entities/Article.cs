using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KarAfarin.Domain.Blog.Entities
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string Content { get; set; }

        public DateTime PublishDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int ViewCount { get; set; }

        public virtual Category Category { get; set; }
        public int CategoryRef { get; set; }

        public string Keywords { get; set; }

        [NotMapped]
        public string Cover { get; set; }
    }
}
