namespace Dust.Platform.Web.Models.Table
{
    public class TotalDistrictsTable
    {
        public string Name { get; set; }

        public int ProjectsCount { get; set; }

        public int DevicesCount { get; set; }

        public double TotalOccupiedArea { get; set; }

        public double TotalFloorage { get; set; }

        public double LastDayValue { get; set; }

        public double LastMonthValue { get; set; }
    }
}