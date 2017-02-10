using System;
using System.Configuration;

namespace Ks.Dust.Camera.MainControl.Application
{
    internal class Config
    {
        private static string _serverAddress;

        private static string _serverPort;

        private static string _ipServerAddress;

        private static ushort _ipServerPort;

        private static readonly Configuration AppSettings =
            ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        public static string ServerAddress
        {
            get
            {
                _serverAddress = AppSettings.AppSettings.Settings["ServerAddress"].Value;

                return _serverAddress;
            }
            set
            {
                AppSettings.AppSettings.Settings["ServerAddress"].Value = value;
                AppSettings.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        public static string ServerPort
        {
            get
            {
                _serverPort = AppSettings.AppSettings.Settings["ServerPort"].Value;

                return _serverPort;
            }
            set
            {
                AppSettings.AppSettings.Settings["ServerPort"].Value = value;
                AppSettings.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        public static string IpServerAddress
        {
            get
            {
                _ipServerAddress = AppSettings.AppSettings.Settings["IpServerAddress"].Value;

                return _ipServerAddress;
            }
            set
            {
                AppSettings.AppSettings.Settings["IpServerAddress"].Value = value;
                AppSettings.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        public static ushort IpServerPort
        {
            get
            {
                _ipServerPort = ushort.Parse(AppSettings.AppSettings.Settings["IpServerPort"].Value);

                return _ipServerPort;
            }
            set
            {
                AppSettings.AppSettings.Settings["IpServerPort"].Value = value.ToString();
                AppSettings.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        public static string CameraNodesJsonFile 
            => $"{Environment.CurrentDirectory}\\Storage\\cameraNodes.json";

        public static string CameraNodesTempJsonFile
            => $"{Environment.CurrentDirectory}\\Storage\\_cameraNodes_temp.json";
    }
}
