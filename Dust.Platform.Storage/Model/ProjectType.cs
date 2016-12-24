namespace Dust.Platform.Storage.Model
{
    /// <summary>
    /// 工程类型
    /// </summary>
    public enum ProjectType : byte
    {
        /// <summary>
        /// 所有工程类型
        /// </summary>
        AllType = 0x00,

        /// <summary>
        /// 建筑工地
        /// </summary>
        ConstructionSite = 0x01,

        /// <summary>
        /// 市政工地
        /// </summary>
        MunicipalWorks = 0x02,

        /// <summary>
        /// 搅拌站
        /// </summary>
        MixingPlant = 0x03
    }
}
