using System;
using System.ComponentModel.DataAnnotations.Schema;
using SHWDTech.Platform.StorageConstrains.Model;

namespace Dust.Platform.Storage.Model
{
    /// <summary>
    /// 设备在线状态
    /// </summary>
    [Serializable]
    public class DeviceOnlineStatus : LongModel
    {
        /// <summary>
        /// 对应的设备ID
        /// </summary>
        [Index("IX_DeviceGuid_StatusType_UpdateTime", IsClustered = true, Order = 0)]
        public Guid DeviceGuid { get; set; }

        /// <summary>
        /// 在线状态
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Index("IX_DeviceGuid_StatusType_UpdateTime", IsClustered = true, Order = 2)]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 状态类型
        /// </summary>
        [Index("IX_DeviceGuid_StatusType_UpdateTime", IsClustered = true, Order = 1)]
        public AverageType StatusType { get; set; }
    }
}
