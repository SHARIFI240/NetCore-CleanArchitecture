using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Common.Models.Authentication
{
    public class AccessTokenResult
    {
        public string Token { get; init; } = string.Empty;
        public int ExpiresIn { get; init; } // seconds
    }
}
