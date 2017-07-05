using System;
using System.Collections.Generic;
using System.Web;
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

    public class ExceedPhotoUploadViewModel
    {
        public Guid Id { get; set; }

        public HttpPostedFileBase File { get; set; }

        public string Comment { get; set; }
    }

    public class ExceedPhotoViewModel
    {
        public string PhotoName { get; set; }

        public string PhotoPath { get; set; }

        public string ThumbPath { get; set; }
    }
}