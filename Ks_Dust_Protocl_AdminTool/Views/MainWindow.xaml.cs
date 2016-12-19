using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Threading;
using Ks_Dust_Protocl_AdminTool.Common;
using Ks_Dust_Protocl_AdminTool.Enums;
using Ks_Dust_Protocl_AdminTool.TcpCore;
using SHWDTech.Platform.ProtocolService;

namespace Ks_Dust_Protocl_AdminTool.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// 通信服务宿主
        /// </summary>
        private CommunicationService _tcpService;

        /// <summary>
        /// 状态栏状态更新计时器
        /// </summary>
        private readonly DispatcherTimer _statusBarTimer = new DispatcherTimer();


        public MainWindow()
        {
            InitializeComponent();
            InitService();
        }

        private void InitService()
        {
            _tcpService = new CommunicationService { ClientReceiveBufferSize = AppConfig.TcpBufferSize};
            _statusBarTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            _statusBarTimer.Tick += UpdateStatusBar;
            _statusBarTimer.Start();

            TxtServerIpAddress.Text = $"{AppConfig.ServerIpAddress}";
            TxtServerPort.Text = $"{AppConfig.ServerPort}";
            AliveConnection.Text = $"{_tcpService.AliveConnection}";
            ReportService.Instance.ReportDataAdded += AppendReport;
        }

        private void StartService(object sender, RoutedEventArgs e)
        {
            _tcpService.SetupTcpAddressPort(new IPEndPoint(IPAddress.Parse(TxtServerIpAddress.Text), int.Parse(TxtServerPort.Text)));
            _tcpService.Start();
            TxtServerIpAddress.IsEnabled = TxtServerPort.IsEnabled = false;
            BtnStartServer.IsEnabled = false;
            BtnStopServer.IsEnabled = true;
        }

        private void CloseService(object sender, RoutedEventArgs e)
        {
            _tcpService.Close();
            TxtServerIpAddress.IsEnabled = TxtServerPort.IsEnabled = true;
            BtnStartServer.IsEnabled = true;
            BtnStopServer.IsEnabled = false;
        }

        private void SwitchDateDisplayMode(object sender, RoutedEventArgs e)
        {
            AppConfig.StartDateFormat = AppConfig.StartDateFormat == DateTimeViewFormat.DateTimeWithoutYear
                ? DateTimeViewFormat.DateTimeWithYear
                : DateTimeViewFormat.DateTimeWithoutYear;

            UpdateStatusBar(sender, e);
        }

        /// <summary>
        /// 更新状态栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateStatusBar(object sender, EventArgs e)
        {
            if (_tcpService.Status == ServiceHostStatus.Running)
            {
                ServerIpAddress.Text = $"{_tcpService.ServerIpEndPoint}";
            }
            ServerStartTDateTime.Text = _tcpService.Status != ServiceHostStatus.Running
                                        ? "-"
                                        : $"{_tcpService.StartDateTime.ToString(AppConfig.StartDateFormat)}";

            var runningTime = DateTime.Now - _tcpService.StartDateTime;
            ServerRunningDateTime.Text = _tcpService.Status != ServiceHostStatus.Running
                                        ? "-"
                                        : $"{(int)runningTime.TotalHours}h {runningTime.Minutes}m {runningTime.Seconds}s";

            AliveConnection.Text = $"{_tcpService.AliveConnection}";

            DecodeProtocolCount.Text = $"{_tcpService.DecodedProtocol}";
        }

        /// <summary>
        /// 发送报告文本到界面
        /// </summary>
        /// <param name="e"></param>
        private void AppendReport(EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                var message = ReportService.Instance.PopupReport();

                if (message == null) return;

                TxtReport.AppendText($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}]");
                TxtReport.AppendText(" => ");
                TxtReport.AppendText(message.Message);
                TxtReport.AppendText("\r\n");
                TxtReport.ScrollToEnd();

                if (TxtReport.Text.Length > AppConfig.MaxReportLength)
                {
                    TxtReport.Clear();
                }
            });
        }

        /// <summary>
        /// 清空消息记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearTextReport(object sender, RoutedEventArgs e)
        {
            TxtReport.Clear();
        }

        /// <summary>
        /// 程序关闭时释放服务资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (_tcpService.Status == ServiceHostStatus.Stopped) return;
            _tcpService.Close();
        }
    }
}
