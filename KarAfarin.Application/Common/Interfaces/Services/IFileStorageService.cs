using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Common.Interfaces.Services
{
    public interface IFileStorageService
    {
        Task<bool> SaveFileAsync(byte[] file, string fileName, string filePath);
        bool IsExistFile(string path);
        Task<byte[]> OpenFileAsync(string path);
        void DeleteFile(string path);
    }
}
