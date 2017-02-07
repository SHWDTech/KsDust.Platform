using System.Configuration;

namespace Ks.Dust.Camera.MainControl.Application
{
    internal class Config
    {
        private static string _serverAddress;

        private static string _serverPort;

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
    }
}
