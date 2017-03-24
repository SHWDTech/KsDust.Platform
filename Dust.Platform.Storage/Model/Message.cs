using System;
using System.ComponentModel.DataAnnotations;
using SHWDTech.Platform.StorageConstrains.Model;

namespace Dust.Platform.Storage.Model
{
    public class Message : LongModel
    {
        public virtual MessageType MessageType { get; set; }

        public virtual DateTime MessageDateTime { get; set; }

        [MaxLength(8000)]
        public virtual string Content { get; set; }
    }
}
