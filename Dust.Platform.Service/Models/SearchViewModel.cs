using Dust.Platform.Storage.Model;
// ReSharper disable InconsistentNaming

namespace Dust.Platform.Service.Models
{
    public class SearchViewModel
    {
        public string name { get; set; }

        public AverageCategory category { get; set; }
    }

    public class SearchPost
    {
        public string objectId { get; set; }
    }
}