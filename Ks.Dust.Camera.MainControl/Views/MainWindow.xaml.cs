using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.Windows;
using Dust.Platform.Storage.Model;
using Ks.Dust.Camera.MainControl.Application;
using Ks.Dust.Camera.MainControl.Camera;
using Ks.Dust.Camera.MainControl.Storage;
using Newtonsoft.Json;
using MessageBox = System.Windows.MessageBox;
using Timer = System.Timers.Timer;

namespace Ks.Dust.Camera.MainControl.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public List<CameraNode> CameraNodes { get; set; }

        public List<CameraLogin> CameraLogins { get; set; }

        private CameraLogin _selectedCameraLogin;

        private CameraNode _selectedCameraNode;

        private CameraNode _loginCameraNode;

        private HikNvr _contorlSdk;

        public HikPlayCtrl LocalPlayCtrl { get; } = new HikPlayCtrl();

        private bool IsCameraLoged { get; set; }

        private bool IsDownloading { get; set; }

        private string FileDownloadFullName { get; set; }

        private readonly Timer _playbackTimer = new Timer { Enabled = false, Interval = 1000 };

        private readonly Timer _downloadTimer = new Timer { Enabled = false, Interval = 1000 };

        private readonly Timer _localPlayTimer = new Timer { Enabled = false, Interval = 1000 };

        private uint _fileTime;

        public MainWindow()
        {
            InitializeComponent();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-cn");
        }

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            LoadLocalStorage();
            InitSdk();
            _playbackTimer.Elapsed += PlaybackTimerOnElapsed;
            _downloadTimer.Elapsed += DownloadTimerOnElapsed;
            _localPlayTimer.Elapsed += LocalPlayTimerOnElapsed;
            DpStart.FormatString = DpEnd.FormatString = "yyyy-MM-dd HH:mm:ss";
            DpStart.Text = $"{DateTime.Now.AddDays(-7):yyyy年MM月dd日 HH:mm:ss}";
            DpEnd.Text = $"{DateTime.Now:yyyy年MM月dd日 HH:mm:ss}";
        }

        private void LocalPlayTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            var playedTime = LocalPlayCtrl.GetPlayedTime();
            Dispatcher.Invoke(() =>
            {
                LocalPlayProgressBar.Value = playedTime;
                if (playedTime == _fileTime)
                {
                    _localPlayTimer.Stop();
                }
            });
        }

        private void DownloadTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            var pos = _contorlSdk.GetDownloadTpos();
            Dispatcher.Invoke(() =>
            {
                if (pos > DownloadProgressBar.Minimum && pos <= DownloadProgressBar.Maximum)
                {
                    DownloadProgressBar.Value = pos;
                }

                if (pos == 100)
                {
                    FileDownloadFullName = string.Empty;
                    _contorlSdk.StopDownload();
                    _downloadTimer.Stop();
                    BtnDownloadPlaybackByName.IsEnabled = BtnDownloadPlaybackByTime.IsEnabled = true;
                    MessageBox.Show("下载完成！", "信息！", MessageBoxButton.OK, MessageBoxImage.Information);
                    SetupDownloadRelatedBtnStatus(true);
                }

                if (pos == 200) //网络异常，回放失败
                {
                    FileDownloadFullName = string.Empty;
                    _downloadTimer.Stop();
                    BtnDownloadPlaybackByName.IsEnabled = BtnDownloadPlaybackByTime.IsEnabled = true;
                    UpdateInfo("网络故障，下载失败！");
                }
            });
        }

        private void PlaybackTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            var pos = _contorlSdk.GetPlaybackTpos();
            Dispatcher.Invoke(() =>
            {
                if ((pos > PlaybackProgressBar.Minimum) && (pos <= PlaybackProgressBar.Maximum))
                {
                    PlaybackProgressBar.Value = pos;
                }

                if (pos == 100)
                {
                    _contorlSdk.StopPlayback();
                    _playbackTimer.Stop();
                    BtnStartPlaybackByName.IsEnabled = BtnStartPlaybackByTime.IsEnabled = true;
                    BtnStopPlayback.IsEnabled = false;
                    UpdateInfo("播放结束！");
                }

                if (pos == 200)
                {
                    MessageBox.Show("网络异常，回放失败！", "警告", MessageBoxButton.OK, MessageBoxImage.Error);
                    _playbackTimer.Stop();
                    BtnStartPlaybackByName.IsEnabled = BtnStartPlaybackByTime.IsEnabled = true;
                    BtnStopPlayback.IsEnabled = false;
                    UpdateInfo("网络异常，回放失败！");
                }
            });
        }

        private void InitSdk()
        {
            if (!HikNvr.Initial())
            {
                MessageBox.Show("初始化摄像头模块失败，请尝试重新启动！", "警告！", MessageBoxButton.OK, MessageBoxImage.Warning);
                Close();
            }
        }

        private void OpenConfigWindow(object sender, RoutedEventArgs args)
        {
            var configWindow = new AppSetup
            {
                Owner = this
            };
            configWindow.ShowDialog();
        }

        private void UpdateCameraNodes(object sender, RoutedEventArgs args)
        {
            var updatePanel = new UpdatePanel { Owner = this };
            updatePanel.Show();
        }

        public void ResfreashCameraNodes(CameraNodeStorage storage)
        {
            if (storage == null) return;
            CameraNodes = storage.Nodes;
            TrCamera.ItemsSource = CameraNodes;
            CameraLogins = storage.Logins;
            UpdateInfo("工地节点更新完成！");
        }

        private void OnNodeSelected(object sender, RoutedEventArgs args)
        {
            var node = (CameraNode)TrCamera.SelectedItem;
            _selectedCameraNode = node;
            _selectedCameraLogin = CameraLogins.FirstOrDefault(obj => obj.DeviceGuid == node.Id);
            if (node.Category == AverageCategory.Device && _selectedCameraLogin != null)
            {
                TxtSelectedCamera.Text = node.Name;
                BtnConnect.IsEnabled = true;
                var localFiles = ApplicationStorage.Files.Where(obj => obj.DeviceGuid == node.Id).ToList();
                LvLocalVedio.ItemsSource = localFiles;
            }
            else
            {
                TxtSelectedCamera.Text = string.Empty;
                BtnConnect.IsEnabled = false;
            }
        }

        private void LoginCamera(object sender, RoutedEventArgs args)
        {
            if (_selectedCameraLogin == null)
            {
                UpdateInfo("登录摄像头失败，未选中可用的摄像头节点！");
                return;
            }
            if (IsCameraLoged)
            {
                IsCameraLoged = !_contorlSdk.Logout();
                if (IsCameraLoged)
                {
                    MessageBox.Show("摄像头退出登录失败，请重新尝试！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            _contorlSdk = new HikNvr();
            IsCameraLoged = _contorlSdk.Login(_selectedCameraLogin);
            _loginCameraNode = _selectedCameraNode;
            OnLogAction();

            UpdateInfo("登录摄像头成功！");
        }

        private void LogOffCamera(object sender, RoutedEventArgs args)
        {
            var stopPlayback = StopPlayBack();
            if (!stopPlayback)
            {
                MessageBox.Show("停止回放失败，请重新尝试！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            StopPlayBackAfter();
            IsCameraLoged = !_contorlSdk.Logout();
            if (IsCameraLoged)
            {
                MessageBox.Show("摄像头退出登录失败，请重新尝试！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            UpdateInfo("退出摄像头成功！");
            OnLogAction();
        }

        private void OnLogAction()
        {
            BtnConnect.IsEnabled = !IsCameraLoged;
            LblConnectStatus.Content = IsCameraLoged ? _selectedCameraNode.Name : string.Empty;
            BtnPausePlayback.IsEnabled = BtnSpeedDown.IsEnabled = BtnSpeedUp.IsEnabled = BtnNormalPlayback.IsEnabled = BtnContinue.IsEnabled =
                BtnDisConnect.IsEnabled = BtnSearchHistory.IsEnabled = BtnStartPlaybackByTime.IsEnabled = BtnDownloadPlaybackByTime.IsEnabled = IsCameraLoged;
            LvHistory.ItemsSource = null;
        }

        private void LoadLocalStorage()
        {
            if (!File.Exists(Config.CameraNodesJsonFile)) return;
            using (var reader = new StreamReader(File.OpenRead(Config.CameraNodesJsonFile)))
            {
                var storage = JsonConvert.DeserializeObject<CameraNodeStorage>(reader.ReadToEnd());
                ResfreashCameraNodes(storage);
            }
        }

        private void SearchHistory(object sender, RoutedEventArgs args)
        {
            BtnSearchHistory.IsEnabled = false;
            LvHistory.ItemsSource = null;//清空文件列表 
            if (DpStart.Value == null || DpEnd.Value == null)
            {
                MessageBox.Show("请选择开始时间和结束时间", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var struFileCondV40 = new CHCNetSDK.NET_DVR_FILECOND_V40
            {
                lChannel = _contorlSdk.DefaultChannel,
                dwFileType = 0xff,
                dwIsLocked = 0xff,
                struStartTime =
                {
                    dwYear = (uint) DpStart.Value.Value.Year,
                    dwMonth = (uint) DpStart.Value.Value.Month,
                    dwDay = (uint) DpStart.Value.Value.Day,
                    dwHour = (uint) DpStart.Value.Value.Hour,
                    dwMinute = (uint) DpStart.Value.Value.Minute,
                    dwSecond = (uint) DpStart.Value.Value.Second
                },
                struStopTime =
                {
                    dwYear = (uint) DpEnd.Value.Value.Year,
                    dwMonth = (uint) DpEnd.Value.Value.Month,
                    dwDay = (uint) DpEnd.Value.Value.Day,
                    dwHour = (uint) DpEnd.Value.Value.Hour,
                    dwMinute = (uint) DpEnd.Value.Value.Minute,
                    dwSecond = (uint) DpEnd.Value.Value.Second
                }
            };

            var records = _contorlSdk.SearchHistory(struFileCondV40);

            LvHistory.ItemsSource = records;
            BtnStartPlaybackByName.IsEnabled = BtnDownloadPlaybackByName.IsEnabled = true;
            BtnSearchHistory.IsEnabled = true;
            UpdateInfo($"搜索完成，共{records.Count}条记录！");
        }

        private void StartPlaybackByName(object sender, RoutedEventArgs args)
        {
            PlaybackProgressBar.Value = 0;
            if (!(LvHistory.SelectedItem is CameraHistoryRecord))
            {
                MessageBox.Show("必须选择一个记录！", "提示！", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var record = (CameraHistoryRecord)LvHistory.SelectedItem;
            var ret = _contorlSdk.StartPlaybackByName(record.FileName, PbPreview.Handle);
            if (ret)
            {
                BtnStartPlaybackByName.IsEnabled = BtnStartPlaybackByTime.IsEnabled = false;
                BtnStopPlayback.IsEnabled = true;
                _playbackTimer.Start();
                UpdateInfo("开始回放！");
            }
            else
            {
                UpdateInfo($"启动回放失败，错误码：{_contorlSdk.LastErrorCode}");
            }
        }

        private void StartPlaybackByTime(object sender, RoutedEventArgs args)
        {
            PlaybackProgressBar.Value = 0;
            if (DpStart.Value == null || DpEnd.Value == null)
            {
                MessageBox.Show("请选择开始时间和结束时间", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var struVodPara = new CHCNetSDK.NET_DVR_VOD_PARA
            {
                hWnd = PbPreview.Handle,
                struBeginTime =
                {
                    dwYear = (uint) DpStart.Value.Value.Year,
                    dwMonth = (uint) DpStart.Value.Value.Month,
                    dwDay = (uint) DpStart.Value.Value.Day,
                    dwHour = (uint) DpStart.Value.Value.Hour,
                    dwMinute = (uint) DpStart.Value.Value.Minute,
                    dwSecond = (uint) DpStart.Value.Value.Second
                },
                struEndTime =
                {
                    dwYear = (uint) DpEnd.Value.Value.Year,
                    dwMonth = (uint) DpEnd.Value.Value.Month,
                    dwDay = (uint) DpEnd.Value.Value.Day,
                    dwHour = (uint) DpEnd.Value.Value.Hour,
                    dwMinute = (uint) DpEnd.Value.Value.Minute,
                    dwSecond = (uint) DpEnd.Value.Value.Second
                }
            };
            struVodPara.dwSize = (uint)Marshal.SizeOf(struVodPara);
            struVodPara.struIDInfo.dwChannel = (uint)_contorlSdk.DefaultChannel;
            var ret = _contorlSdk.StartPlaybackByTime(struVodPara);
            if (ret)
            {
                BtnStartPlaybackByName.IsEnabled = BtnStartPlaybackByTime.IsEnabled = false;
                BtnStopPlayback.IsEnabled = true;
                _playbackTimer.Start();
                UpdateInfo("开始回放！");
            }
            else
            {
                UpdateInfo($"启动回放失败，错误码：{_contorlSdk.LastErrorCode}");
            }
        }

        private void StopPlaybackAction(object sender, RoutedEventArgs args)
        {
            var ret = StopPlayBack();
            if (ret)
            {
                StopPlayBackAfter();
                UpdateInfo("回放结束！");
            }
            else
            {
                UpdateInfo($"停止回放失败，错误码：{_contorlSdk.LastErrorCode}");
            }
        }

        private bool StopPlayBack() => _contorlSdk.StopPlayback();

        private void StopPlayBackAfter()
        {
            BtnStartPlaybackByName.IsEnabled = BtnStartPlaybackByTime.IsEnabled = true;
            BtnStopPlayback.IsEnabled = false;
            _playbackTimer.Stop();
            PbPreview.Refresh();
        }

        private void DownloadPlaybackByName(object sender, RoutedEventArgs args)
        {
            if (!CheckBeforeDownlod(out Guid camera))
            {
                return;
            }

            if (!(LvHistory.SelectedItem is CameraHistoryRecord record))
            {
                MessageBox.Show("必须选择一个记录！", "提示！", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            FileDownloadFullName = $"{Config.VedioStorageDirectory}\\{camera}\\{record.FileName}.mp4";
            StartDownloadProcess(record.FileName, null);
        }

        private void HistoryRecordSelected(object sender, RoutedEventArgs args)
        {
            if (!(LvHistory.SelectedItem is CameraHistoryRecord)) return;
            var record = (CameraHistoryRecord)LvHistory.SelectedItem;
            LblSelectedFileName.Content = record.FileName;
            LblRecordStartDt.Content = record.StartDateTime;
            LblRecordEndDt.Content = record.EndDateTime;
        }

        private void DownloadPlaybackByTime(object sender, RoutedEventArgs args)
        {
            if (!CheckBeforeDownlod(out Guid camera))
            {
                return;
            }

            if (DpStart.Value == null || DpEnd.Value == null)
            {
                MessageBox.Show("请选择开始时间和结束时间", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var struDownPara = new CHCNetSDK.NET_DVR_PLAYCOND
            {
                dwChannel = (uint)_contorlSdk.DefaultChannel,
                struStartTime =
                {
                    dwYear = (uint) DpStart.Value.Value.Year,
                    dwMonth = (uint) DpStart.Value.Value.Month,
                    dwDay = (uint) DpStart.Value.Value.Day,
                    dwHour = (uint) DpStart.Value.Value.Hour,
                    dwMinute = (uint) DpStart.Value.Value.Minute,
                    dwSecond = (uint) DpStart.Value.Value.Second
                },
                struStopTime =
                {
                    dwYear = (uint) DpEnd.Value.Value.Year,
                    dwMonth = (uint) DpEnd.Value.Value.Month,
                    dwDay = (uint) DpEnd.Value.Value.Day,
                    dwHour = (uint) DpEnd.Value.Value.Hour,
                    dwMinute = (uint) DpEnd.Value.Value.Minute,
                    dwSecond = (uint) DpEnd.Value.Value.Second
                }
            };

            FileDownloadFullName = $"{Config.VedioStorageDirectory}\\{camera}\\{DpStart.Value:yyyy-MM-dd(HH-mm-ss)}-{DpEnd.Value:yyyy-MM-dd(HH-mm-ss)}.mp4";
            StartDownloadProcess(null, struDownPara);
        }

        private void StartDownloadProcess(string recordName, CHCNetSDK.NET_DVR_PLAYCOND? cond)
        {
            if (File.Exists(FileDownloadFullName))
            {
                MessageBox.Show("此文件已经存在，下载中断！", "警告！", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var downloadRet = false;
            if (!string.IsNullOrWhiteSpace(recordName))
            {
                _contorlSdk.DownloadFileByName(recordName, FileDownloadFullName);
            }
            else if (cond != null)
            {
                downloadRet = _contorlSdk.DownloadFileByTime(cond.Value, FileDownloadFullName);
            }

            if (downloadRet)
            {
                SetupDownloadRelatedBtnStatus(false);
                _downloadTimer.Start();
                BtnDownloadPlaybackByName.IsEnabled = BtnDownloadPlaybackByTime.IsEnabled = false;
                UpdateInfo("下载开始！");
            }
            else
            {
                UpdateInfo($"启动下载失败，错误码：{_contorlSdk.LastErrorCode}");
            }
        }

        private bool CheckBeforeDownlod(out Guid cameraId)
        {
            cameraId = Guid.Empty;
            DownloadProgressBar.Value = 0;
            if (!Config.VedioStorageReady)
            {
                MessageBox.Show("视频存储目录未通过检查，请检查设置！", "警告！", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            cameraId = _loginCameraNode.Id;
            if (!Directory.Exists($"{Config.VedioStorageDirectory}\\{cameraId}"))
            {
                try
                {
                    Directory.CreateDirectory($"{Config.VedioStorageDirectory}\\{cameraId}");
                }
                catch (Exception)
                {
                    MessageBox.Show("创建存储目录失败，请检查设置！", "警告！", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }

            return true;
        }

        private void SetupDownloadRelatedBtnStatus(bool status)
        {
            BtnDisConnect.IsEnabled = BtnStartPlaybackByName.IsEnabled = BtnStartPlaybackByTime.IsEnabled = status;
            IsDownloading = !status;
        }

        private void PausePlayback(object sender, RoutedEventArgs args)
        {
            _contorlSdk.PausePlayback();
        }

        private void ContinuePlayback(object sender, RoutedEventArgs args)
        {
            _contorlSdk.ContinuePlayback();
        }

        private void SlowPlayback(object sender, RoutedEventArgs args)
        {
            _contorlSdk.SlowPlayback();
        }

        private void FastPlayback(object sender, RoutedEventArgs args)
        {
            _contorlSdk.FastPlayback();
        }

        private void NormalPlayback(object sender, RoutedEventArgs args)
        {
            _contorlSdk.ResumePlayback();
        }

        private void StartPlayLocalVedio(object sender, RoutedEventArgs args)
        {
            if (LvLocalVedio.SelectedItem == null)
            {
                MessageBox.Show("请先选择一个视频文件！", "提示！", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var fileName = ((CameraVedioStorage)LvLocalVedio.SelectedItem).FileFullPathWithName;
            var ret = LocalPlayCtrl.OpenFile(fileName);
            if (!ret)
            {
                UpdateInfo($"打开视频文件失败，错误码：{LocalPlayCtrl.LastErrorCode}");
                return;
            }
            ret = LocalPlayCtrl.StartPlayLocal(PbLocalPreview.Handle, out var fileTime);
            if (!ret)
            {
                UpdateInfo($"播放视频文件失败，错误码：{LocalPlayCtrl.LastErrorCode}");
            }

            LocalPlayProgressBar.Maximum = _fileTime = fileTime;
            _localPlayTimer.Start();
        }

        private void StopPlayLocalVedio(object sender, RoutedEventArgs args)
        {
            var ret = LocalPlayCtrl.StopPlayLocal();
            if (!ret)
            {
                UpdateInfo($"停止播放视频文件失败，错误码：{LocalPlayCtrl.LastErrorCode}");
            }
            LocalPlayProgressBar.Value = 0;
            _localPlayTimer.Stop();
            PbLocalPreview.Refresh();
        }

        private void PausePlayLocalVedio(object sender, RoutedEventArgs args)
        {
            var ret = LocalPlayCtrl.PausePlayLocal(1);
            if (!ret)
            {
                UpdateInfo($"暂停播放视频文件失败，错误码：{LocalPlayCtrl.LastErrorCode}");
            }
        }

        private void ResumePlayLocalVedio(object sender, RoutedEventArgs args)
        {
            var ret = LocalPlayCtrl.PausePlayLocal(0);
            if (!ret)
            {
                UpdateInfo($"恢复播放视频文件失败，错误码：{LocalPlayCtrl.LastErrorCode}");
            }
        }

        private void UpdateInfo(string message)
        {
            Dispatcher.Invoke(() =>
            {
                TbAppInfo.Text = message;
            });
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (IsDownloading)
            {
                var result = MessageBox.Show("正在下载中，是否确退出？", "警告！", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _contorlSdk.StopDownload();
                    Config.NotFinishedDownloadFileName = FileDownloadFullName;
                    _downloadTimer.Stop();
                }
                else
                {
                    e.Cancel = true;
                }
            }
            base.OnClosing(e);
        }
    }
}
