using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Ks.Dust.Camera.MainControl.Camera
{
    public class HikPlayCtrl : INotifyPropertyChanged
    {
        public uint LastErrorCode { get; private set; }

        private int _nPort;

        private bool _isPlayStarted;

        private bool _isPaused;

        private readonly CHCNetSDK.FILEENDCALLBACK _fileendcallback = FilePlayEndCallback;

        public bool IsPlayStarted
        {
            get
            {
                return _isPlayStarted;
            }
            set
            {
                _isPlayStarted = value;
                OnPropertyChanged("IsPlayStarted");
            } 
        }

        public bool IsPaused
        {
            get
            {
                return _isPaused;
            }
            set
            {
                _isPaused = value;
                OnPropertyChanged("IsPaused");
            }
        }

        public HikPlayCtrl()
        {
            CHCNetSDK.PlayM4_GetPort(ref _nPort);
        }

        public bool GetPlayPort()
        {
            var ret = CHCNetSDK.PlayM4_GetPort(ref _nPort);
            if (!ret)
            {
                LastErrorCode = CHCNetSDK.PlayM4_GetLastError(_nPort);
            }

            return ret;
        }

        public bool OpenFile(string sFileName)
        {
            var ret = CHCNetSDK.PlayM4_OpenFile(_nPort, sFileName);

            if (!ret)
            {
                LastErrorCode = CHCNetSDK.PlayM4_GetLastError(_nPort);
            }

            return ret;
        }

        public static void FilePlayEndCallback(int nPort, IntPtr pUser)
        {

        }

        public bool StartPlayLocal(IntPtr hWnd, out uint fileTime)
        {
            var intPtr = Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]);
            CHCNetSDK.PlayM4_SetFileEndCallback(_nPort, _fileendcallback, intPtr);
            var ret = CHCNetSDK.PlayM4_Play(_nPort, hWnd);
            if (!ret)
            {
                LastErrorCode = CHCNetSDK.PlayM4_GetLastError(_nPort);
            }

            fileTime = CHCNetSDK.PlayM4_GetFileTime(_nPort);
            IsPlayStarted = true;

            return ret;
        }

        public bool StopPlayLocal()
        {
            var ret = CHCNetSDK.PlayM4_Stop(_nPort);
            if (!ret)
            {
                LastErrorCode = CHCNetSDK.PlayM4_GetLastError(_nPort);
                return false;
            }

            ret = CHCNetSDK.PlayM4_CloseFile(_nPort);

            if (!ret)
            {
                LastErrorCode = CHCNetSDK.PlayM4_GetLastError(_nPort);
            }

            return ret;
        }

        public uint GetPlayedTime()
        {
            return CHCNetSDK.PlayM4_GetPlayedTime(_nPort);
        }

        public bool PausePlayLocal(uint nPause)
        {
            var ret = CHCNetSDK.PlayM4_Pause(_nPort, nPause);
            if (!ret)
            {
                LastErrorCode = CHCNetSDK.PlayM4_GetLastError(_nPort);
            }

            return ret;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
