namespace Dust.Platform.Storage.Model
{
    /// <summary>
    /// 监测数据类型
    /// </summary>
    public enum MonitorType : byte
    {
        /// <summary>
        /// 实时值
        /// </summary>
        RealTime = 0x00,

        /// <summary>
        /// 十五分钟均值
        /// </summary>
        FifteenMin = 0x01,

        /// <summary>
        /// 小时均值
        /// </summary>
        HourAvg = 0x02,

        /// <summary>
        /// 日均值
        /// </summary>
        DayAvg = 0x03,

        /// <summary>
        /// 月均值
        /// </summary>
        MonthAvg = 0x04
    }
}
