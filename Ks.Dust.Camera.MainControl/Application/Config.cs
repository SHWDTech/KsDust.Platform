using System;
using System.Configuration;
using System.IO;
using System.Windows;
using Ks.Dust.Camera.MainControl.Camera;

namespace Ks.Dust.Camera.MainControl.Application
{
    internal class Config
    {
        private static string _serverAddress;

        private static string _serverPort;

        private static string _ipServerAddress;

        private static string _ipServerPort;

        private static string _vedioStorageDirectory;

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

        public static string IpServerPort
        {
            get
            {
                _ipServerPort = AppSettings.AppSettings.Settings["IpServerPort"].Value;

                return _ipServerPort;
            }
            set
            {
                AppSettings.AppSettings.Settings["IpServerPort"].Value = value;
                AppSettings.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        public static string VedioStorageDirectory
        {
            get
            {
                _vedioStorageDirectory = AppSettings.AppSettings.Settings["VedioStorageDirectory"].Value;

                return _vedioStorageDirectory;
            }
            set
            {
                if (!Directory.Exists(value))
                {
                    try
                    {
                        Directory.CreateDirectory(value);
                        VedioStorageReady = true;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("无法创建视频存储目录，请重新设置！", "警告！", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                AppSettings.AppSettings.Settings["VedioStorageDirectory"].Value = value;
                AppSettings.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettngs");
            }
        }

        public static bool VedioStorageReady { get; set; }

        public static bool LocalVedioPortReady { get; set; }

        public static string CameraNodesJsonFile 
            => $"{Environment.CurrentDirectory}\\Storage\\cameraNodes.json";

        public static string CameraNodesTempJsonFile
            => $"{Environment.CurrentDirectory}\\Storage\\_cameraNodes_temp.json";

        public static void ConfigCheck()
        {
            if (!Directory.Exists(VedioStorageDirectory))
            {
                try
                {
                    Directory.CreateDirectory(VedioStorageDirectory);
                    VedioStorageReady = true;
                }
                catch (Exception)
                {
                    MessageBox.Show("无法创建视频存储目录，请检查设置！", "警告！", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                VedioStorageReady = true;
            }

            LocalVedioPortReady = HikNvr.GetPlayPort();
        }
    }
}
