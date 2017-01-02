// ReSharper disable InconsistentNaming
namespace Dust.Platform.Web.Models.Statistics
{
    public class HistoryLineChartViewModel
    {
        public string name { get; set; }

        public object[] value { get; set; } = new object[2];
    }
}