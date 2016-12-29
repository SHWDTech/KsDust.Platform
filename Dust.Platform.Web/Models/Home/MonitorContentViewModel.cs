using System;

namespace Dust.Platform.Web.Models.Home
{
    public class MonitorContentPost
    {
        public Guid TargetId { get; set; }

        public string MapContainer { get; set; }

        public ViewType  ViewType { get; set; }
    }

    public enum ViewType : byte
    {
        City = 0x00,

        District = 0x01,

        Enterprise = 0x02,

        Project = 0x03,

        Device = 0x04
    }
}