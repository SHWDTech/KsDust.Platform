namespace Dust.Platform.Storage.Model
{
    /// <summary>
    /// 均值类型
    /// </summary>
    public enum AverageType : byte
    {
        /// <summary>
        /// 日均值
        /// </summary>
        DayAvg = 0x00,

        /// <summary>
        /// 月均值
        /// </summary>
        MonthAvg = 0x01
    }
}
