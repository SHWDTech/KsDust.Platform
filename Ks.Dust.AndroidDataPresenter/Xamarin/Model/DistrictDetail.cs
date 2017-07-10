// ReSharper disable InconsistentNaming
namespace Ks.Dust.AndroidDataPresenter.Xamarin.Model
{
    public class DistrictDetail
    {
        public string districtName {get; set;}

        /** 监测点名字 */
        public string name {get; set;}

        /** 监测点id */
        public string id {get; set;}

        /** 颗粒物 */
        public double tsp {get; set;}

        public double pm25 {get; set;}

        public double pm100 {get; set;}

        /** 级别 */
        public int rate {get; set;}
    }
}