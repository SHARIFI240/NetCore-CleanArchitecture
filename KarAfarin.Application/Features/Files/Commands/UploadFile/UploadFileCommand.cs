using KarAfarin.Application.Common.Interfaces;
using KarAfarin.Application.Common.Models.Media;
using KarAfarin.Application.Common.Models.ResultOperation;
using KarAfarin.Domain.Media.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Features.Files.Commands.UploadFile
{
    public class UploadFileCommand : IRequest<ResultOperation>
    {
        public List<FileModel> Files { get; set; }

        public MediaEntityTarget MediaEntityTarget { get; set; }

        public int? EntityRef { get; set; }

        public string Path { get; set; }
    }

    public class FileModel
    {
        public byte[] File { get; set; }

        public string FileName { get; set; }
    }

}