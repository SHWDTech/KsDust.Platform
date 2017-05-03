namespace Dust.Platform.Storage.Model
{
    public enum ReportType : byte
    {
        /// <summary>
        /// 月报表
        /// </summary>
        Month = 0x10,

        /// <summary>
        /// 年报表
        /// </summary>
        Year = 0x20,

        /// <summary>
        /// 在线率统计报表
        /// </summary>
        OnlineStatus = 0x30
    }
}
