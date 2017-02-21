using System.Data.Entity;
using Dust.Platform.Storage.Model;

namespace Dust.Platform.Storage.Repository
{
    /// <summary>
    /// 昆山扬尘数据上下文
    /// </summary>
    public class KsDustDbContext : DbContext
    {
        /// <summary>
        /// 创建默认的DbContext
        /// </summary>
        public KsDustDbContext() : base("Ks_Dust_Platform")
        {
            
        }

        public KsDustDbContext(string connString) : base(connString)
        {
            
        }

        /// <summary>
        /// 区县
        /// </summary>
        public virtual DbSet<District> Districts { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public virtual DbSet<Vendor> Vendors { get; set; }

        /// <summary>
        /// 施工单位
        /// </summary>
        public virtual DbSet<Enterprise> Enterprises { get; set; }

        /// <summary>
        /// 工程
        /// </summary>
        public virtual DbSet<KsDustProject> KsDustProjects { get; set; }

        /// <summary>
        /// 设备
        /// </summary>
        public virtual DbSet<KsDustDevice> KsDustDevices { get; set; }

        /// <summary>
        /// 监测数据
        /// </summary>
        public virtual DbSet<KsDustMonitorData> KsDustMonitorDatas { get; set; }

        /// <summary>
        /// 摄像头
        /// </summary>
        public virtual DbSet<KsDustCamera> KsDustCameras { get; set; }

        /// <summary>
        /// 报警值
        /// </summary>
        public virtual DbSet<KsDustAlarm> KsDustAlarms { get; set; }

        /// <summary>
        /// 均值
        /// </summary>
        public virtual DbSet<AverageMonitorData> AverageMonitorDatas { get; set; }

        /// <summary>
        /// 系统配置
        /// </summary>
        public virtual DbSet<SystemConfiguration> SystemConfigurations { get; set; }

        /// <summary>
        /// 用户关联的实体（区域、施工单位、工程、设备）
        /// </summary>
        public virtual DbSet<UserRelatedEntity> UserRelatedEntities { get; set; }

        /// <summary>
        /// 设备在线率统计
        /// </summary>
        public virtual DbSet<DeviceOnlineStatus> DeviceOnlineStatuses { get; set; }

        /// <summary>
        /// 在线率统计（区县、工程）
        /// </summary>
        public virtual DbSet<OnlineStatistics> OnlineStatisticses { get; set; }

        /// <summary>
        /// 报表数据
        /// </summary>
        public virtual DbSet<Report> Reports { get; set; }

        /// <summary>
        /// 设备维保记录
        /// </summary>
        public virtual DbSet<DeviceMantanceRecord> DeviceMantanceRecords { get; set; }
    }
}
