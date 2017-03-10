using System.ComponentModel.DataAnnotations.Schema;
using SHWDTech.Platform.StorageConstrains.Model;

namespace Dust.Platform.Storage.Model
{
    public class Report : LongModel
    {
        [Column(TypeName = "text")]
        public string ReportDataJson { get; set; }

        [Index("IX_ReportType", IsClustered = true, Order = 0)]
        public ReportType ReportType { get; set; }

        public string ReportDate { get; set; }
    }
}
