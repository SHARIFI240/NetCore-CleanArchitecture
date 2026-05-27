using KarAfarin.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Infrastructure.Services
{
    public class FileStorageService(IWebHostEnvironment env) : IFileStorageService
    {
        public async Task<bool> SaveFileAsync(byte[] file, string fileName, string filePath)
        {
			try
			{
                string path = $"{env.WebRootPath}/Upload{filePath}/{fileName}";

                await File.WriteAllBytesAsync(path, file);
                return true;
			}
			catch (Exception ex)
			{
                return false;
			}
        }

        public bool IsExistFile(string path)
        { 
            return File.Exists($"{env.WebRootPath}/Upload" + path);
        }

        public async Task<byte[]> OpenFileAsync(string path)
        {
            byte[] fileBytes = await File.ReadAllBytesAsync($"{env.WebRootPath}/Upload" + path);

            return fileBytes;
        }

        public void DeleteFile(string path)
        {
            var file = $"{env.WebRootPath}/Upload" + path;

            if(File.Exists(file))
                File.Delete(file);
            
        }
    }
}
