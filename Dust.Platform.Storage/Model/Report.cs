using System.ComponentModel.DataAnnotations.Schema;
using SHWDTech.Platform.StorageConstrains.Model;

namespace Dust.Platform.Storage.Model
{
    public class Report : LongModel
    {
        [Column(TypeName = "text")]
        public string ReportDataJson { get; set; }

        public ReportType ReportType { get; set; }

        public string ReportDate { get; set; }
    }
}
