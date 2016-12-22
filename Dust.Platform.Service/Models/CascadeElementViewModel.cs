using System;
using Dust.Platform.Storage.Model;

namespace Dust.Platform.Service.Models
{
    public class CascadeElementViewModel
    {
        public Guid CascadeElementId { get; set; }

        public AverageCategory CascadeElementLevel { get; set; }

        public string CascadeElementName { get; set; }
    }

    public class CascadeElementPostParams
    {
        public AverageCategory CascadeElementLevel { get; set; }

        public Guid CascadeElementId { get; set; }
    }
}