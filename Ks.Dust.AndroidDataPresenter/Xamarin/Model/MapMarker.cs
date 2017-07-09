// ReSharper disable InconsistentNaming
namespace Ks.Dust.AndroidDataPresenter.Xamarin.Model
{
    public class MapMarker
    {
        public string id { get; set; }

        public string name { get; set; }

        public string time { get; set; }

        public double tsp { get; set; }

        public double pm25 { get; set; }

        public double pm100 { get; set; }

        public int rate { get; set; }

        public double longitude { get; set; }

        public double latitude { get; set; }

        public bool isOnline { get; set; }
    }
}