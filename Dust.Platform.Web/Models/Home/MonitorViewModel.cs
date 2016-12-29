using System.Collections.Generic;
// ReSharper disable InconsistentNaming

namespace Dust.Platform.Web.Models.Home
{
    public class MonitorViewModel
    {
        public List<Nodes> TreeNodes { get; set; } = new List<Nodes>();
    }

    public class Nodes
    {
        public string name { get; set; }

        public List<Nodes> children { get; set; }
    }
}