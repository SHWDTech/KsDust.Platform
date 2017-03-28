using System;
using System.ComponentModel.DataAnnotations;
using SHWDTech.Platform.StorageConstrains.Model;

namespace Dust.Platform.Storage.Model
{
    public class Notice : LongModel
    {
        public virtual NoticeType NoticeType { get; set; }

        public virtual DateTime NoticeDateTime { get; set; }

        [MaxLength(36)]
        public virtual string RelatedValue { get; set; }

        [MaxLength(200)]
        public virtual string Title { get; set; }

        [MaxLength(8000)]
        public virtual string Content { get; set; }
    }
}
