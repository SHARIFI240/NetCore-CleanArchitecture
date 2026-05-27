using KarAfarin.Application.Common.Interfaces;
using KarAfarin.Domain.Media.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Common.Models.Media
{
    public class MediaUploadDataModel
    {
        public string FileName { get; set; }

        public string FilePath { get; set; }

        public MediaEntityTarget MediaEntityTarget { get; set; }

        public int? EntityRef { get; set; }

        public byte[] File { get; set; }
    }

}
