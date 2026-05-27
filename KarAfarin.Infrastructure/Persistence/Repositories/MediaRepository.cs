using KarAfarin.Application.Common.Interfaces;
using KarAfarin.Application.Common.Interfaces.Services;
using KarAfarin.Application.Common.Models.Media;
using KarAfarin.Domain.Media.Entities;
using KarAfarin.Domain.Media.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Infrastructure.Persistence.Repositories
{
    public class MediaRepository(ApplicationDbContext context, IFileStorageService fileStorageService) : IMediaRepository
    {
        public async Task<object> UploadFiles(List<MediaUploadDataModel> medias, CancellationToken cancellationToken)
        {

            List<Media> mediaLst = new List<Media>();

            foreach (var media in medias)
            {
                var resultOfSave = await fileStorageService.SaveFileAsync(media.File, media.FileName, media.FilePath);

                if (resultOfSave)
                {
                    Media model = new Media()
                    {
                        EntityRef = media.EntityRef,
                        CreateDate = DateTime.Now,
                        FileName = media.FileName,
                        FilePath = media.FilePath,
                        MediaEntityTarget = media.MediaEntityTarget,
                    };

                    await context.Media.AddAsync(model, cancellationToken);
                    mediaLst.Add(model);

                }
            }

            if (await context.SaveChangesAsync(cancellationToken) > 0)
            {
                return mediaLst.Select(g=> new {
                    g.FileName,
                    g.FilePath,
                });
            }

            return 0;

        }

        public async Task<List<MediaUploadDataModel>> GetFilesByEntityAsync(MediaEntityTarget target, int? entityRef, CancellationToken cancellationToken)
        {
            List<MediaUploadDataModel> lst = new List<MediaUploadDataModel>();

            var query = context.Media.Where(e => e.MediaEntityTarget == target).AsNoTracking();

            if (entityRef.HasValue)
                query = query.Where(g=>g.EntityRef == entityRef);

            var result = await query.OrderByDescending(g=>g.Id).ToListAsync(cancellationToken);

            foreach (var item in result)
            {
                if (fileStorageService.IsExistFile($"{item.FilePath}/{item.FileName}"))
                    lst.Add(new MediaUploadDataModel(){
                        FileName = item.FileName,
                        FilePath=item.FilePath,
                        EntityRef = item.EntityRef,
                        MediaEntityTarget=item.MediaEntityTarget,
                        File = await fileStorageService.OpenFileAsync($"{item.FilePath}/{item.FileName}")
                    });
            }

            return lst;
        }

        public async Task DeleteMediaAsync(MediaEntityTarget target, int entityRef, CancellationToken cancellationToken)
        {
            var files = await context.Media.Where(g => g.MediaEntityTarget == target && g.EntityRef == entityRef).ToListAsync();

            foreach (var file in files)
            {
                fileStorageService.DeleteFile($"{file.FilePath}/{file.FileName}");
                context.Media.Remove(file);
            }

            await context.SaveChangesAsync(cancellationToken);

        }
    }
}
