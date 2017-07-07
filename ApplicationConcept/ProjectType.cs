namespace ApplicationConcept
{
    /// <summary>
    /// 均值对象类型
    /// </summary>
    public enum ProjectType
    {
        /// <summary>
        /// 设备
        /// </summary>
        Device = 0x00,

        /// <summary>
        /// 工程
        /// </summary>
        Project = 0x01,

        /// <summary>
        /// 施工单位
        /// </summary>
        Enterprise = 0x02,

        /// <summary>
        /// 区县
        /// </summary>
        District = 0x03,

        /// <summary>
        /// 全市
        /// </summary>
        WholeCity = 0x04,

        /// <summary>
        /// 未知项目类型
        /// </summary>
        UnKnow = 0xFF
    }
}
