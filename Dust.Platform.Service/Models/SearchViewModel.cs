using System;
using Dust.Platform.Storage.Model;
// ReSharper disable InconsistentNaming

namespace Dust.Platform.Service.Models
{
    public class SearchViewModel
    {
        public Guid objectId { get; set; }

        public string objectName { get; set; }

        public AverageCategory objectLevel { get; set; }
    }

    public class SearchPost
    {
        public string searchName { get; set; }
    }
}