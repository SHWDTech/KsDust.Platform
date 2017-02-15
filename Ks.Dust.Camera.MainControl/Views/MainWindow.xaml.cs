using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;
using Dust.Platform.Storage.Model;
using Ks.Dust.Camera.MainControl.Application;
using Ks.Dust.Camera.MainControl.Camera;
using Ks.Dust.Camera.MainControl.Storage;
using Newtonsoft.Json;
using MessageBox = System.Windows.MessageBox;

namespace Ks.Dust.Camera.MainControl.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private List<CameraNode> CameraNodes { get; set; }

        private List<CameraLogin> CameraLogins { get; set; }

        private CameraLogin _selectedCameraLogin;

        private CameraNode _selectedCameraNode;

        private CameraNode _loginCameraNode;

        private HikNvr _contorlSdk;

        private bool IsCameraLoged { get; set; }

        private readonly Timer _playbackTimer = new Timer() { Enabled = false, Interval = 1000 };

        private readonly Timer _downloadTimer = new Timer() { Enabled = false, Interval = 1000 };

        private readonly Timer _localPlayTimer = new Timer() { Enabled = false, Interval = 1000 };

        private uint _fileTime;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            LoadLocalStorage();
            InitSdk();
            _playbackTimer.Elapsed += PlaybackTimerOnElapsed;
            _downloadTimer.Elapsed += DownloadTimerOnElapsed;
            _localPlayTimer.Elapsed += LocalPlayTimerOnElapsed;
            DpStart.Text = $"{DateTime.Now.AddDays(-7): yyyy-MM-dd HH:mm:ss}";
            DpEnd.Text = $"{DateTime.Now: yyyy-MM-dd HH:mm:ss}";
        }

        private void LocalPlayTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            var playedTime = HikNvr.GetPlayedTime();
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
                if ((pos > DownloadProgressBar.Minimum) && (pos <= DownloadProgressBar.Maximum))
                {
                    DownloadProgressBar.Value = pos;
                }

                if (pos == 100)
                {
                    _contorlSdk.StopDownload();
                    _downloadTimer.Stop();
                    BtnDownloadPlaybackByName.IsEnabled = BtnDownloadPlaybackByTime.IsEnabled = true;
                    UpdateInfo("下载完成！");
                }

                if (pos == 200) //网络异常，回放失败
                {
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
            var configWindow = new AppSetup()
            {
                Owner = this
            };
            configWindow.ShowDialog();
        }

        private void UpdateCameraNodes(object sender, RoutedEventArgs args)
        {
            var updatePanel = new UpdatePanel() { Owner = this };
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
            if (_selectedCameraLogin == null) return;
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

        private void StopPlayback(object sender, RoutedEventArgs args)
        {
            var ret = _contorlSdk.StopPlayback();
            if (ret)
            {
                BtnStartPlaybackByName.IsEnabled = BtnStartPlaybackByTime.IsEnabled = true;
                BtnStopPlayback.IsEnabled = false;
                _playbackTimer.Stop();
                UpdateInfo("回放结束！");
            }
            else
            {
                UpdateInfo($"停止回放失败，错误码：{_contorlSdk.LastErrorCode}");
            }
        }

        private void DownloadPlaybackByName(object sender, RoutedEventArgs args)
        {
            DownloadProgressBar.Value = 0;
            var record = LvHistory.SelectedItem as CameraHistoryRecord;
            if (!Config.VedioStorageReady)
            {
                MessageBox.Show("视频存储目录未通过检查，请检查设置！", "警告！", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (record == null)
            {
                MessageBox.Show("必须选择一个记录！", "提示！", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var camera = _loginCameraNode.Id;
            if (!Directory.Exists($"{Config.VedioStorageDirectory}\\{camera}"))
            {
                try
                {
                    Directory.CreateDirectory($"{Config.VedioStorageDirectory}\\{camera}");
                }
                catch (Exception)
                {
                    MessageBox.Show("创建存储目录失败，请检查设置！", "警告！", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            var ret = _contorlSdk.DownloadFileByName(record.FileName, $"{Config.VedioStorageDirectory}\\{camera}\\{record.FileName}.mp4");
            if (ret)
            {
                _downloadTimer.Start();
                BtnDownloadPlaybackByName.IsEnabled = BtnDownloadPlaybackByTime.IsEnabled = false;
                UpdateInfo("下载开始！");
            }
            else
            {
                UpdateInfo($"启动下载失败，错误码：{_contorlSdk.LastErrorCode}");
            }
        }

        private void DownloadPlaybackByTime(object sender, RoutedEventArgs args)
        {
            DownloadProgressBar.Value = 0;
            if (!Config.VedioStorageReady)
            {
                MessageBox.Show("视频存储目录未通过检查，请检查设置！", "警告！", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (DpStart.Value == null || DpEnd.Value == null)
            {
                MessageBox.Show("请选择开始时间和结束时间", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var camera = _loginCameraNode.Id;
            if (!Directory.Exists($"{Config.VedioStorageDirectory}\\{camera}"))
            {
                try
                {
                    Directory.CreateDirectory($"{Config.VedioStorageDirectory}\\{camera}");
                }
                catch (Exception)
                {
                    MessageBox.Show("创建存储目录失败，请检查设置！", "警告！", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
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

            var ret = _contorlSdk.DownloadFileByTime(struDownPara, $"{Config.VedioStorageDirectory}\\{camera}\\{DpStart.Value:yyyy-MM-dd(HH-mm-ss)}-{DpEnd.Value:yyyy-MM-dd(HH-mm-ss)}.mp4");
            if (ret)
            {
                _downloadTimer.Start();
                BtnDownloadPlaybackByName.IsEnabled = BtnDownloadPlaybackByTime.IsEnabled = false;
                UpdateInfo("下载开始！");
            }
            else
            {
                UpdateInfo($"启动下载失败，错误码：{_contorlSdk.LastErrorCode}");
            }
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
            if (!CheckLocalPlayPrepare()) return;
            if (LvLocalVedio.SelectedItem == null)
            {
                MessageBox.Show("请先选择一个视频文件！", "提示！", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var fileName = ((CameraVedioStorage) LvLocalVedio.SelectedItem).FileFullPathWithName;
            var ret = HikNvr.OpenFile(fileName);
            if (!ret)
            {
                UpdateInfo($"打开视频文件失败，错误码：{HikNvr.LastPlayErrorCode}");
                return;
            }
            uint fileTime;
            ret = HikNvr.StartPlayLocal(PbLocalPreview.Handle, out fileTime);
            if (!ret)
            {
                UpdateInfo($"播放视频文件失败，错误码：{HikNvr.LastPlayErrorCode}");
            }

            LocalPlayProgressBar.Maximum = _fileTime = fileTime;
            _localPlayTimer.Start();
        }

        private void StopPlayLocalVedio(object sender, RoutedEventArgs args)
        {
            var ret = HikNvr.StopPlayLocal();
            if (!ret)
            {
                UpdateInfo($"停止播放视频文件失败，错误码：{HikNvr.LastPlayErrorCode}");
            }
            LocalPlayProgressBar.Value = 0;
            _localPlayTimer.Stop();
        }

        private void PausePlayLocalVedio(object sender, RoutedEventArgs args)
        {
            var ret = HikNvr.PausePlayLocal(1);
            if (!ret)
            {
                UpdateInfo($"暂停播放视频文件失败，错误码：{HikNvr.LastPlayErrorCode}");
            }
        }

        private void ResumePlayLocalVedio(object sender, RoutedEventArgs args)
        {
            var ret = HikNvr.PausePlayLocal(0);
            if (!ret)
            {
                UpdateInfo($"恢复播放视频文件失败，错误码：{HikNvr.LastPlayErrorCode}");
            }
        }

        private bool CheckLocalPlayPrepare()
        {
            if (!Config.LocalVedioPortReady)
            {
                MessageBox.Show("本地视频播放端口未就绪，请尝试重新启动！","错误！",MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void UpdateInfo(string message)
        {
            Dispatcher.Invoke(() =>
            {
                TbAppInfo.Text = message;
            });
        }
    }
}
