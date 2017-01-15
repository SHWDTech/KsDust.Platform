using System;
using Dust.Platform.Storage.Model;
// ReSharper disable InconsistentNaming

namespace Dust.Platform.Service.Models
{
    public class StatisticsDetailPost
    {
        public ProjectType ProjectType { get; set; }

        public AverageCategory elementType { get; set; }

        public Guid elementId { get; set; }
    }
}