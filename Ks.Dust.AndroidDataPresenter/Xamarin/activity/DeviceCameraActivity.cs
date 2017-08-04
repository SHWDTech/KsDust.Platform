using System;
using System.Linq;
using System.Threading;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using CheeseBind;
using Com.Hikvision.Netsdk;
using Org.MediaPlayer.PlayM4;

namespace Ks.Dust.AndroidDataPresenter.Xamarin.activity
{
    [Activity(Label = nameof(DeviceCameraActivity))]
    public class DeviceCameraActivity : KsDustBaseActivity, ISurfaceHolderCallback
    {
        [BindView(Resource.Id.surfaceView)]

        protected SurfaceView SurfaceView;

        private NET_DVR_DEVICEINFO_V30 _oNetDvrDeviceInfoV30;

        private int _iLogId = -1; // return by NET_DVR_Login_v30

        private int _iPlayId = -1; // return by NET_DVR_RealPlay_V30

        private int _iPlaybackId = -1; // return by NET_DVR_PlayBackByTime

        private int _iPort = -1; // play port

        private int _iStartChan; // start channel no

        //private int _iChanNum; // channel number

        //private bool _bTalkOn;

        //private bool _bPtzl;

        //private bool _bMultiPlay;

        private bool _bNeedDecode = true;

        //private bool _bSaveRealData;

        //private bool _bStopPlayback;

        public const string Ip = "ip";

        public const string UserName = "userName";

        public const string UserPassword = "password";

        public const string Serialnumber = "serialNumber";

        private const short Port = 7071;

        public const int LoginSuccess = 1;

        public const int LoginFailed = 2;

        private CameraHandler _mainHandler;

        [BindView(Resource.Id.login)] public View LoginBtn { get; set; }

        [BindView(Resource.Id.preview)] public View PreviewBtn { get; set; }

        private string _ip;

        private string _dvrName = "";

        private short _dvrNameLen = 0;

        private string _dvrSerialNumber;

        private short _dvrSerialLen;

        private string _user;

        private string _password;

        private Action _action;

        [OnClick(Resource.Id.login)]
        protected void LoginBtnClick(object sender, EventArgs args)
        {
            new Thread(() =>
            {
                var info = GetDvrIp(_ip, Port, _dvrName, _dvrNameLen, _dvrSerialNumber, _dvrSerialLen);
                try
                {
                    var finalIp = System.Text.Encoding.UTF8.GetString(info.SGetIP.ToArray());
                    finalIp = finalIp.Replace("\0", string.Empty);
                    Login(finalIp, info.DwPort, _user, _password);
                }
                catch (Exception)
                {
                    RunOnUiThread(() =>
                    {
                        Toast.MakeText(this, "登陆摄像头失败。", ToastLength.Short).Show();
                    });
                }
            }).Start();

        }

        [OnClick(Resource.Id.preview)]
        protected void PreviewBtnClick(object sender, EventArgs args)
        {
            Preview();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _action = Finish;
            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            SetContentView(Resource.Layout.activity_device_camera);
            Cheeseknife.Bind(this);

            PreviewBtn.Clickable = false;
            SurfaceView.Holder.AddCallback(this);
            if (!InitSdk())
            {
                Finish();
                return;
            }

            _mainHandler = new CameraHandler(this);
            var bundle = Intent.Extras;
            _ip = bundle.GetString(Ip);
            if (string.IsNullOrWhiteSpace(_ip))
            {
                Toast.MakeText(this, "摄像头尚未配置，IP地址为空", ToastLength.Short).Show();
                _mainHandler.PostDelayed(_action, 3500);
            }

            _dvrSerialNumber = bundle.GetString(Serialnumber);
            if (string.IsNullOrWhiteSpace(_dvrSerialNumber))
            {
                Toast.MakeText(this, "摄像头尚未配置，Serialnumber为空", ToastLength.Short).Show();
                _mainHandler.PostDelayed(_action, 3500);
            }

            _dvrSerialLen = (short)(_dvrSerialNumber.Length);

            _user = bundle.GetString(UserName);
            if (string.IsNullOrWhiteSpace(_user))
            {
                Toast.MakeText(this, "摄像头还没配置,User缺失", ToastLength.Short).Show();
                _mainHandler.PostDelayed(_action, 3500);
                return;
            }

            _password = bundle.GetString(UserPassword);
            if (string.IsNullOrWhiteSpace(_password))
            {
                Toast.MakeText(this, "摄像头还没配置,password缺失", ToastLength.Short).Show();
                _mainHandler.PostDelayed(_action, 3500);
            }
            // Create your application here
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            CleanupUp();
        }

        private bool InitSdk()
        {
            return HCNetSDK.Instance.NET_DVR_Init();
        }

        private NET_DVR_RESOLVE_DEVICEINFO GetDvrIp(string ip, short port, string dvrName, short dvrNameLength, string serialNumber,
            short serialLength)
        {
            var info = new NET_DVR_RESOLVE_DEVICEINFO();
            HCNetSDK.Instance.NET_DVR_GetDVRIPByResolveSvr_EX(ip, port, dvrName, dvrNameLength, serialNumber,
                serialLength, info);

            return info;
        }

        private void Login(string loginIp, int loginPort, string user, string password)
        {
            try
            {
                if (_iLogId < 0)
                {
                    _iLogId = LoginNormalDevice(loginIp, loginPort, user, password);
                    _mainHandler.SendEmptyMessage(_iLogId < 0 ? LoginFailed : LoginSuccess);

                    //var exceptionCallBack = GetExceptionCallBack();
                    //if (exceptionCallBack == null) return;
                    //if (!HCNetSDK.Instance.NET_DVR_SetExceptionCallBack(exceptionCallBack)) return;
                }
                else
                {
                    if (!HCNetSDK.Instance.NET_DVR_Logout_V30(_iLogId)) return;
                    _iLogId = -1;
                }
            }
            catch (Exception)
            {
                RunOnUiThread(() =>
                {
                    Toast.MakeText(this, "登陆摄像头失败。", ToastLength.Short).Show();
                });
            }
        }

        private int LoginNormalDevice(string loginIp, int loginPort, string user, string password)
        {
            _oNetDvrDeviceInfoV30 = new NET_DVR_DEVICEINFO_V30();
            if (_oNetDvrDeviceInfoV30 == null)
            {
                return -1;
            }

            var loginId =
                HCNetSDK.Instance.NET_DVR_Login_V30(loginIp, loginPort, user, password, _oNetDvrDeviceInfoV30);
            if (loginId < 0)
            {
                return -1;
            }

            if (_oNetDvrDeviceInfoV30.ByChanNum > 0)
            {
                _iStartChan = _oNetDvrDeviceInfoV30.ByStartChan;
                //_iChanNum = _oNetDvrDeviceInfoV30.ByChanNum;
            }
            else if (_oNetDvrDeviceInfoV30.ByIPChanNum > 0)
            {
                _iStartChan = _oNetDvrDeviceInfoV30.ByStartDChan;
                //_iChanNum = _oNetDvrDeviceInfoV30.ByChanNum + _oNetDvrDeviceInfoV30.ByHighDChanNum * 256;
            }

            return loginId;
        }

        protected static IExceptionCallBack GetExceptionCallBack()
        {
            var callBack = new ExceptionCallBack();
            return callBack;
        }

        private void Preview()
        {
            try
            {
                if (_iLogId < 0) return;
                if (_bNeedDecode)
                {
                    if (_iPlayId < 0)
                    {
                        StartSinglePreview();
                    }
                    else
                    {
                        StopSinglePreview();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void StartSinglePreview()
        {
            if (_iPlaybackId >= 0) return;
            var realDataCallBack = GetRealPlayerCallBack();
            if (realDataCallBack == null) return;
            var previewInfo = new NET_DVR_PREVIEWINFO
            {
                LChannel = _iStartChan,
                DwStreamType = 0,
                BBlocked = 1
            };

            _iPlayId = HCNetSDK.Instance.NET_DVR_RealPlay_V40(_iLogId, previewInfo, realDataCallBack);
            if (_iPlayId < 0)
            {
                var errorCode = HCNetSDK.Instance.NET_DVR_GetLastError();
                Console.WriteLine(errorCode);
            }
        }

        private void StopSinglePreview()
        {
            if (_iPlayId < 0) return;

            if (!HCNetSDK.Instance.NET_DVR_StopRealPlay(_iPlayId))
            {
                Console.WriteLine("Error");
                return;
            }

            _iPlayId = -1;
            StopSinglePlayer();
        }

        private void StopSinglePlayer()
        {
            if (!Player.Instance.Stop(_iPort)) return;
            if (!Player.Instance.CloseStream(_iPort)) return;
            if (!Player.Instance.FreePort(_iPort)) return;

            _iPort = -1;
        }

        private IRealPlayCallBack GetRealPlayerCallBack()
        {
            var callBack = new RealPlayCallBack(this);
            return callBack;
        }

        public void ProcessRealData(int iplayViewNo, int iDataType, byte[] pDataBuffer, int iDataSize, int iStreamMode)
        {
            if (!_bNeedDecode)
            {

            }
            else
            {
                if (HCNetSDK.NetDvrSyshead == iDataType)
                {
                    if (_iPort >= 0) return;
                    _iPort = Player.Instance.Port;
                    if (_iPort == -1) return;
                    if (iDataSize > 0)
                    {
                        if (!Player.Instance.SetStreamOpenMode(_iPort, iStreamMode)) return;
                        if (!Player.Instance.OpenStream(_iPort, pDataBuffer, iDataSize, 2 * 1024 * 1024)) return;
                        if (!Player.Instance.Play(_iPort, SurfaceView.Holder)) return;
                    }
                }
                else
                {
                    if (!Player.Instance.InputData(_iPort, pDataBuffer, iDataSize))
                    {
                        for (var i = 0; i < 4000 && _iPlaybackId >= 0; i++)
                        {
                            if (Player.Instance.InputData(_iPort, pDataBuffer, iDataSize)) break;

                            if (i % 100 == 0)
                            {

                            }

                            try
                            {
                                Thread.Sleep(10);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                    }
                }
            }
        }

        public void SurfaceChanged(ISurfaceHolder holder, Format format, int width, int height)
        {

        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            SurfaceView.Holder.SetFormat(Format.Translucent);
            if (_iPort == -1) return;
            var surface = holder.Surface;
            if (surface.IsValid)
            {
                //if (!Player.Instance.SetVideoWindow(_iPort, 0, holder)) return;
            }
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            if (_iPort == -1) return;
            if (holder.Surface.IsValid)
            {
                //if (!Player.Instance.SetVideoWindow(_iPort, 0, null)) return;
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutInt(nameof(_iPort), _iPort);
            base.OnSaveInstanceState(outState);
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            _iPort = savedInstanceState.GetInt(nameof(_iPort));
            base.OnRestoreInstanceState(savedInstanceState);
        }

        public void CleanupUp()
        {
            if (_iPort > 0)
            {
                Player.Instance.FreePort(_iPort);
            }
            _iPort = -1;
            HCNetSDK.Instance.NET_DVR_Cleanup();
        }
    }

    public class CameraHandler : Handler
    {
        private readonly DeviceCameraActivity _activity;

        public CameraHandler(DeviceCameraActivity dev)
        {
            _activity = dev;
        }

        public override void HandleMessage(Message msg)
        {
            switch (msg.What)
            {
                case DeviceCameraActivity.LoginSuccess:
                    _activity.PreviewBtn.Clickable = true;
                    Toast.MakeText(_activity, "连接摄像头成功", ToastLength.Short).Show();
                    return;
                case DeviceCameraActivity.LoginFailed:
                    _activity.PreviewBtn.Clickable = false;
                    Toast.MakeText(_activity, "摄像头异常", ToastLength.Short).Show();
                    return;

            }
        }
    }

    public class ExceptionCallBack : Java.Lang.Object, IExceptionCallBack
    {
        public void FExceptionCallBack(int p0, int p1, int p2)
        {
            Console.WriteLine("recv exception, type:" + p0);
        }
    }

    public class RealPlayCallBack : Java.Lang.Object, IRealPlayCallBack
    {
        private readonly DeviceCameraActivity _activity;

        public RealPlayCallBack(DeviceCameraActivity activity)
        {
            _activity = activity;
        }

        public void FRealDataCallBack(int p0, int p1, byte[] p2, int p3)
        {
            _activity.ProcessRealData(1, p1, p2, p3, Player.StreamRealtime);
        }
    }

}