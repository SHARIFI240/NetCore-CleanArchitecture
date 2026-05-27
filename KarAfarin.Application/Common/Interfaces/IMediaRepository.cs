using KarAfarin.Application.Common.Models.Media;
using KarAfarin.Domain.Media.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Common.Interfaces
{
    public interface IMediaRepository
    {
        Task<object> UploadFiles(List<MediaUploadDataModel> medias, CancellationToken cancellationToken);

        Task<List<MediaUploadDataModel>> GetFilesByEntityAsync(MediaEntityTarget target, int? entityRef, CancellationToken cancellationToken);

        Task DeleteMediaAsync(MediaEntityTarget target, int entityRef, CancellationToken cancellationToken);
    }
}
