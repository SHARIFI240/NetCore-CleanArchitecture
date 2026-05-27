using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Common.Interfaces.Services
{
    public interface IHashService
    {
        string Hash(string inp);
        bool Verify(string inp, string hash);
    }
}
