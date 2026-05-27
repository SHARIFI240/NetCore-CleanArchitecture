using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KarAfarin.Domain.Authentication.Entities
{
    public class RefreshTokens
    {
        [Key]
        public int Id { get; set; }

        public virtual Users User { get; set; }
        public int UserId { get; set; }

        public string Token { get; set; }

        public DateTime ExpireAt { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        public bool IsExpired(DateTime now)
        {
            return now >= ExpireAt;
        }

        public void Revoke(DateTime now)
        {
            RevokedAt = now;
        }

        public string? ReplacedByToken { get; set; }

        public string IpAddress { get; set; }
    }
}