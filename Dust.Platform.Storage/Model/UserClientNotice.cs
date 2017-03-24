using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dust.Platform.Storage.Model
{
    public class UserClientNotice
    {
        [Key, Column(Order = 0)]
        public Guid User { get; set; }

        [Key, Column(Order = 1)]
        public NoticeClientType NoticeClientType { get; set; }

        [Key, Column(Order = 2)]
        public long Notice { get; set; }

        public bool IsReaded { get; set; }
    }
}
