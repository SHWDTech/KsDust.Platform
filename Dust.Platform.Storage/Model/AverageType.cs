namespace Dust.Platform.Storage.Model
{
    /// <summary>
    /// 均值类型
    /// </summary>
    public enum AverageType : byte
    {
        /// <summary>
        /// 十五分钟均值
        /// </summary>
        FifteenAvg = 0x00,

        /// <summary>
        /// 小时均值
        /// </summary>
        HourAvg = 0x01,

        /// <summary>
        /// 日均值
        /// </summary>
        DayAvg = 0x02,

        /// <summary>
        /// 月均值
        /// </summary>
        MonthAvg = 0x03
    }
}
