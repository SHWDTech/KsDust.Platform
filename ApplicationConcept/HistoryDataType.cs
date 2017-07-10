namespace ApplicationConcept
{
    /// <summary>
    /// 均值类型
    /// </summary>
    public enum HistoryDataType
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
        MonthAvg = 0x03,

        /// <summary>
        /// 季节均值
        /// </summary>
        Season = 0x04,

        /// <summary>
        /// 半年均值
        /// </summary>
        HalfYear = 0x05,

        /// <summary>
        /// 年均值
        /// </summary>
        Year = 0x06
    }
}
