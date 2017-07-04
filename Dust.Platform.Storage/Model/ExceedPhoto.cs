using System;
using SHWDTech.Platform.StorageConstrains.Model;

namespace Dust.Platform.Storage.Model
{
    public class ExceedPhoto : LongModel
    {
        public Guid AlarmGuid { get; set; }

        public string PhotoPath { get; set; }

        public string PhotoThumbPath { get; set; }

        public string Comment { get; set; }

        public DateTime UploadDateTime { get; set; }
    }
}
