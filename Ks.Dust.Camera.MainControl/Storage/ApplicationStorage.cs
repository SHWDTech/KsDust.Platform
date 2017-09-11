using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ks.Dust.Camera.MainControl.Application;

namespace Ks.Dust.Camera.MainControl.Storage
{
    public static class ApplicationStorage
    {
        public static void LoadDownloadFile()
        {
            if (Config.VedioStorageReady)
            {
                var deviceDirs = Directory.GetDirectories(Config.VedioStorageDirectory);
                foreach (var deviceDir in deviceDirs)
                {
                    var folderName = Path.GetFileName(deviceDir);
                    if(string.IsNullOrWhiteSpace(folderName)) continue;
                    var deviceId = Guid.Parse(folderName);
                    var files = Directory.GetFiles($"{deviceDir}");
                    foreach (var file in files)
                    {
                        var storage = new CameraVedioStorage()
                        {
                            DeviceGuid = deviceId,
                            FileDirectory = $"{deviceDir}",
                            FileName = Path.GetFileName(file)
                        };

                        Files.Add(storage);
                    }

                }
            }
        }

        public static void AddNewFile(Guid deviceGuid, string fillnameWithFullPath)
        {
            var deviceDir = Directory.GetDirectories(Config.VedioStorageDirectory).FirstOrDefault(d => d.Contains(deviceGuid.ToString()));
            if (deviceDir != null)
            {
                Files.Add(new CameraVedioStorage
                {
                    DeviceGuid = deviceGuid,
                    FileDirectory = $"{deviceDir}",
                    FileName = Path.GetFileName(fillnameWithFullPath)
                });
            }
        }

        public static List<CameraVedioStorage> Files { get; set; } = new List<CameraVedioStorage>();
    }
}
