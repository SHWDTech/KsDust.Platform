using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SHWDTech.Platform.StorageConstrains.Model;

namespace Dust.Platform.Storage.Model
{
    public class OnlineStatistics : LongModel
    {
        [Index("Ix_ProjectType_Target_AverageType_DateTime", IsClustered = true, IsUnique = true, Order = 1)]
        public Guid TargetGuid { get; set; }

        [Index("Ix_ProjectType_Target_AverageType_DateTime", IsClustered = true, IsUnique = true, Order = 0)]
        public ProjectType Category { get; set; }

        [Index("Ix_ProjectType_Target_AverageType_DateTime", IsClustered = true, IsUnique = true, Order = 3)]
        public DateTime UpdateTime { get; set; }

        [Index("Ix_ProjectType_Target_AverageType_DateTime", IsClustered = true, IsUnique = true, Order = 2)]
        public AverageType StatusType { get; set; }

        public double Statistics { get; set; }
    }
}
