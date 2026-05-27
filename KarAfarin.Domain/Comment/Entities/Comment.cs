using KarAfarin.Domain.Authentication.Entities;
using KarAfarin.Domain.Comment.Enums;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KarAfarin.Domain.Comment.Entities
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }

        public virtual Users? User { get; set; }
        public int? UserId { get; set; }

        public string Url { get; set; }

        public CommentTargetType TargetType { get; set; }

        public int? TargetRef { get; set; }
    }
}
