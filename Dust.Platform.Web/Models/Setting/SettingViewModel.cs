using System.Collections.Generic;
using Dust.Platform.Web.Models.Home;

namespace Dust.Platform.Web.Models.Setting
{
    public class SettingViewModel
    {
        public List<Nodes> MenuNodes { get; set; }
    }

    /// <summary>
    /// 维保状态
    /// </summary>
    public enum MantanceStatus : byte
    {
        Mantanced = 0x00,

        NeedMantance = 0x01,

        Overdue = 0xFF
    }
}