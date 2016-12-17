using System;
using System.Configuration;
using System.Net;
using Ks_Dust_Protocl_AdminTool.Enums;

namespace Ks_Dust_Protocl_AdminTool.Common
{
    public class AppConfig
    {
        /// <summary>
        /// TCP接收Buffer大小
        /// </summary>
        public static readonly int TcpBufferSize;

        /// <summary>
        /// 完整时间格式
        /// </summary>
        public const string FullDateFormat = "yyyy-MM-dd HH:mm:ss fff";

        /// <summary>
        /// 短时间格式
        /// </summary>
        public const string ShortDateFormat = "HH:mm:ss fff";

        /// <summary>
        /// 服务开始时间显示格式
        /// </summary>
        public static string StartDateFormat { get; set; } = DateTimeViewFormat.DateTimeWithoutYear;

        /// <summary>
        /// 服务器地址
        /// </summary>
        public static IPAddress ServerIpAddress { get; }

        /// <summary>
        /// 服务器端口号
        /// </summary>
        public static int ServerPort { get; }

        /// <summary>
        /// 设备连接检查时间间隔
        /// </summary>
        public static readonly double DeviceConnectionChevkInterval;

        /// <summary>
        /// 设备超时连接周期
        /// </summary>
        public static readonly TimeSpan DeviceDisconnectInterval;

        /// <summary>
        /// 日志文件最大容量
        /// </summary>
        public static readonly int MaxReportLength;

        static AppConfig()
        {
            TcpBufferSize = int.Parse(ConfigurationManager.AppSettings["TcpBufferSize"]);

            ServerIpAddress = IPAddress.Parse(ConfigurationManager.AppSettings["ServerAddress"]);

            ServerPort = int.Parse(ConfigurationManager.AppSettings["ServerPort"]);

            DeviceConnectionChevkInterval = double.Parse(ConfigurationManager.AppSettings["DeviceConnectionChevkInterval"]);

            DeviceDisconnectInterval = new TimeSpan(0, 0, int.Parse(ConfigurationManager.AppSettings["DeviceDisconnectInterval"]));

            MaxReportLength = int.Parse(ConfigurationManager.AppSettings["MaxReportLength"]);
        }
    }
}
