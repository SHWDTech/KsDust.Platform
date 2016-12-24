using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dust.Platform.Storage.Model
{
    public class UserRelatedEntity
    {
        [Key, Column(Order = 0)]
        public Guid User { get; set; }

        [Key, Column(Order = 1)]
        public Guid Entity { get; set; }
    }
}
