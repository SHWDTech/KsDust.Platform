using System;
using SHWDTech.Platform.StorageConstrains.Model;

namespace Dust.Platform.Storage.Model
{
    public class DeviceOnlineStatus : LongModel
    {
        public Guid DeviceGuid { get; set; }

        public bool IsOnline { get; set; }

        public DateTime UpdateTime { get; set; }

        public AverageType StatusType { get; set; }
    }
}
