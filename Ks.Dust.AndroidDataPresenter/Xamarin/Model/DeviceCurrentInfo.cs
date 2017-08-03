﻿// ReSharper disable InconsistentNaming

namespace Ks.Dust.AndroidDataPresenter.Xamarin.Model
{
    public class DeviceCurrentInfo
    {
        public string name { get; set; }

        public string address{ get; set; }

        public string vendor{ get; set; }

        public string enterprise{ get; set; }

        public string superintend{ get; set; }

        public string mobile{ get; set; }

        public bool isOnline{ get; set; }

        public double tsp{ get; set; }

        public double pm25{ get; set; }

        public double pm100{ get; set; }

        public double noise{ get; set; }

        public double temperature{ get; set; }

        public double humidity{ get; set; }

        public double windspeed{ get; set; }

        public int winddirection{ get; set; }

        public string updatetime{ get; set; }

        public string camera{ get; set; }

        public string ipServerAddr{ get; set; }

        public string serialNumber{ get; set; }

        public string userName{ get; set; }

        public string password{ get; set; }
    }
}