using System.Collections.Generic;
using Dust.Platform.Storage.Model;

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

        public string id { get; set; }

        public AverageCategory viewType { get; set; }

        public List<Nodes> children { get; set; }

        public string ajaxurl { get; set; }

        public object routeValue { get; set; }

        public string callBack { get; set; }
    }
}