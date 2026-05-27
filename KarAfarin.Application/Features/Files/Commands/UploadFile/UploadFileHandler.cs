using KarAfarin.Application.Common.Interfaces;
using KarAfarin.Application.Common.Models.Media;
using KarAfarin.Application.Common.Models.ResultOperation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Features.Files.Commands.UploadFile
{
    public class UploadFileHandler(IMediaRepository mediaRepository) : IRequestHandler<UploadFileCommand, ResultOperation>
    {
        public async Task<ResultOperation> Handle(UploadFileCommand request,  CancellationToken cancellationToken)
        {
            List<MediaUploadDataModel> lst = new List<MediaUploadDataModel>();
            foreach (var item in request.Files)
                lst.Add(new MediaUploadDataModel()
                {
                    EntityRef = request.EntityRef,
                    FilePath = request.Path,
                    File = item.File,
                    MediaEntityTarget = request.MediaEntityTarget,
                    FileName = Guid.NewGuid().ToString().Replace("-", "") + "_" + item.FileName,
                });


            var result = await mediaRepository.UploadFiles(lst, cancellationToken);

            if (!result.Equals(0))
            {
                return ResultOperation.Ok(result,"فایل با موفقیت بارگذاری شد");
            }

            return ResultOperation.Fail("هنگام بارگذاری فایل خطایی رخ داد");
        }
    }
}
