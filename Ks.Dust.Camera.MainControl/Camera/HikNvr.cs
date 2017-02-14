using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Dust.Platform.Storage.Model;
using Ks.Dust.Camera.MainControl.Application;

namespace Ks.Dust.Camera.MainControl.Camera
{
    internal class HikNvr
    {
        public uint LastErrorCode { get; private set; }

        public int DefaultChannel => _iChannelNum[0];

        private string _dvrAddress = string.Empty;

        private ushort _dvrPortNumber = 8000;

        private int _loginUserId;

        private int _lPlayHandle;

        private int _lDownloadHandle;

        private bool _paused;

        private uint _dwAChanTotalNum;

        private uint _dwDChanTotalNum;

        private readonly int[] _iIpDevId = new int[96];

        private readonly int[] _iChannelNum = new int[96];

        private CHCNetSDK.NET_DVR_DEVICEINFO_V30 _deviceInfo;

        private CHCNetSDK.NET_DVR_IPPARACFG_V40 _struIpParaCfgV40;

        private CHCNetSDK.NET_DVR_IPCHANINFO _struChanInfo;

        private CHCNetSDK.NET_DVR_IPCHANINFO_V40 _struChanInfoV40;

        public static bool Initial()
        {
            return CHCNetSDK.NET_DVR_Init();
        }

        public bool Login(CameraLogin loginInfo)
        {
            var ipAddress = new byte[16];
            uint dwPort = 0;
            if (!CHCNetSDK.NET_DVR_GetDVRIPByResolveSvr_EX(Config.IpServerAddress, ushort.Parse(Config.IpServerPort), null, 0,
                    loginInfo.DomainBytes, (ushort)loginInfo.DomainBytes.Length, ipAddress, ref dwPort))
            {
                LastErrorCode = CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }
            _dvrAddress = Encoding.UTF8.GetString(ipAddress).TrimEnd('\0');
            _dvrPortNumber = (ushort)dwPort;

            _loginUserId = CHCNetSDK.NET_DVR_Login_V30(_dvrAddress, _dvrPortNumber, loginInfo.User, loginInfo.Password, ref _deviceInfo);

            if (_loginUserId < 0)
            {
                LastErrorCode = CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }
            _dwAChanTotalNum = _deviceInfo.byChanNum;
            _dwDChanTotalNum = _deviceInfo.byIPChanNum + 256 * (uint)_deviceInfo.byHighDChanNum;
            return ReadIpcChannel();
        }

        public bool Logout()
        {
            var ret = CHCNetSDK.NET_DVR_Logout(_loginUserId);
            if (!ret)
            {
                LastErrorCode = CHCNetSDK.NET_DVR_GetLastError();
            }
            return ret;
        }

        private bool ReadIpcChannel()
        {
            var dwSize = (uint)Marshal.SizeOf(_struIpParaCfgV40);

            var ptrIpParaCfgV40 = Marshal.AllocHGlobal((int)dwSize);
            Marshal.StructureToPtr(_struIpParaCfgV40, ptrIpParaCfgV40, false);

            uint dwReturn = 0;
            var iGroupNo = 0;  //该Demo仅获取第一组64个通道，如果设备IP通道大于64路，需要按组号0~i多次调用NET_DVR_GET_IPPARACFG_V40获取

            if (!CHCNetSDK.NET_DVR_GetDVRConfig(_loginUserId, CHCNetSDK.NET_DVR_GET_IPPARACFG_V40, iGroupNo, ptrIpParaCfgV40, dwSize, ref dwReturn))
            {
                LastErrorCode = CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }
            _struIpParaCfgV40 = (CHCNetSDK.NET_DVR_IPPARACFG_V40)Marshal.PtrToStructure(ptrIpParaCfgV40, typeof(CHCNetSDK.NET_DVR_IPPARACFG_V40));

            for (var i = 0; i < _dwAChanTotalNum; i++)
            {
                _iChannelNum[i] = i + _deviceInfo.byStartChan;
            }

            uint iDChanNum = 64;

            if (_dwDChanTotalNum < 64)
            {
                iDChanNum = _dwDChanTotalNum; //如果设备IP通道小于64路，按实际路数获取
            }

            for (var i = 0; i < iDChanNum; i++)
            {
                _iChannelNum[i + _dwAChanTotalNum] = i + (int)_struIpParaCfgV40.dwStartDChan;
                var byStreamType = _struIpParaCfgV40.struStreamMode[i].byGetStreamType;

                dwSize = (uint)Marshal.SizeOf(_struIpParaCfgV40.struStreamMode[i].uGetStream);
                switch (byStreamType)
                {
                    //目前NVR仅支持直接从设备取流 NVR supports only the mode: get stream from device directly
                    case 0:
                        var ptrChanInfo = Marshal.AllocHGlobal((int)dwSize);
                        Marshal.StructureToPtr(_struIpParaCfgV40.struStreamMode[i].uGetStream, ptrChanInfo, false);
                        _struChanInfo = (CHCNetSDK.NET_DVR_IPCHANINFO)Marshal.PtrToStructure(ptrChanInfo, typeof(CHCNetSDK.NET_DVR_IPCHANINFO));

                        _iIpDevId[i] = _struChanInfo.byIPID + _struChanInfo.byIPIDHigh * 256 - iGroupNo * 64 - 1;

                        Marshal.FreeHGlobal(ptrChanInfo);
                        break;
                    case 6:
                        var ptrChanInfoV40 = Marshal.AllocHGlobal((int)dwSize);
                        Marshal.StructureToPtr(_struIpParaCfgV40.struStreamMode[i].uGetStream, ptrChanInfoV40, false);
                        _struChanInfoV40 = (CHCNetSDK.NET_DVR_IPCHANINFO_V40)Marshal.PtrToStructure(ptrChanInfoV40, typeof(CHCNetSDK.NET_DVR_IPCHANINFO_V40));

                        _iIpDevId[i] = _struChanInfoV40.wIPID - iGroupNo * 64 - 1;

                        Marshal.FreeHGlobal(ptrChanInfoV40);
                        break;
                }
            }
            Marshal.FreeHGlobal(ptrIpParaCfgV40);
            return true;
        }

        public List<CameraHistoryRecord> SearchHistory(CHCNetSDK.NET_DVR_FILECOND_V40 struFileCondV40)
        {
            var records = new List<CameraHistoryRecord>();
            var mLFindHandle = CHCNetSDK.NET_DVR_FindFile_V40(_loginUserId, ref struFileCondV40);

            if (mLFindHandle < 0)
            {
                LastErrorCode = CHCNetSDK.NET_DVR_GetLastError();
                return records;
            }
            var struFileData = new CHCNetSDK.NET_DVR_FINDDATA_V30();
            while (true)
            {
                //逐个获取查找到的文件信息 Get file information one by one.
                var result = CHCNetSDK.NET_DVR_FindNextFile_V30(mLFindHandle, ref struFileData);

                if (result == CHCNetSDK.NET_DVR_ISFINDING)  //正在查找请等待 Searching, please wait
                {
                }
                else if (result == CHCNetSDK.NET_DVR_FILE_SUCCESS) //获取文件信息成功 Get the file information successfully
                {
                    var record = new CameraHistoryRecord
                    {
                        FileName = struFileData.sFileName,
                        StartDateTime = Convert.ToString(struFileData.struStartTime.dwYear) + "-" +
                                        Convert.ToString(struFileData.struStartTime.dwMonth) + "-" +
                                        Convert.ToString(struFileData.struStartTime.dwDay) + " " +
                                        Convert.ToString(struFileData.struStartTime.dwHour) + ":" +
                                        Convert.ToString(struFileData.struStartTime.dwMinute) + ":" +
                                        Convert.ToString(struFileData.struStartTime.dwSecond),
                        EndDateTime = Convert.ToString(struFileData.struStopTime.dwYear) + "-" +
                                      Convert.ToString(struFileData.struStopTime.dwMonth) + "-" +
                                      Convert.ToString(struFileData.struStopTime.dwDay) + " " +
                                      Convert.ToString(struFileData.struStopTime.dwHour) + ":" +
                                      Convert.ToString(struFileData.struStopTime.dwMinute) + ":" +
                                      Convert.ToString(struFileData.struStopTime.dwSecond)
                    };

                    records.Add(record);
                }
                else if (result == CHCNetSDK.NET_DVR_FILE_NOFIND || result == CHCNetSDK.NET_DVR_NOMOREFILE)
                {
                    break; //未查找到文件或者查找结束，退出   No file found or no more file found, search is finished 
                }
                else
                {
                    break;
                }
            }

            return records;
        }

        public bool StartPlaybackByName(string fileName, IntPtr hwnd)
        {
            _lPlayHandle = CHCNetSDK.NET_DVR_PlayBackByName(_loginUserId, fileName, hwnd);
            uint iOutValue = 0;
            if (!CHCNetSDK.NET_DVR_PlayBackControl_V40(_lPlayHandle, CHCNetSDK.NET_DVR_PLAYSTART, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue))
            {
                LastErrorCode = CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }

            return _lPlayHandle >= 0;
        }

        public bool StartPlaybackByTime(CHCNetSDK.NET_DVR_VOD_PARA struVodPara)
        {
            _lPlayHandle = CHCNetSDK.NET_DVR_PlayBackByTime_V40(_loginUserId, ref struVodPara);
            if (_lPlayHandle < 0)
            {
                LastErrorCode = CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }

            uint iOutValue = 0;
            if (!CHCNetSDK.NET_DVR_PlayBackControl_V40(_lPlayHandle, CHCNetSDK.NET_DVR_PLAYSTART, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue))
            {
                LastErrorCode = CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }

            return true;
        }

        public bool StopPlayback()
        {
            if (!CHCNetSDK.NET_DVR_StopPlayBack(_lPlayHandle))
            {
                LastErrorCode = CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }

            return true;
        }

        public bool StopDownload()
        {
            if (!CHCNetSDK.NET_DVR_StopGetFile(_lDownloadHandle))
            {
                LastErrorCode = CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }

            return true;
        }

        public bool PausePlayback()
        {
            if (_paused) return false;
            uint iOutValue = 0;
            if (!CHCNetSDK.NET_DVR_PlayBackControl_V40(_lPlayHandle, CHCNetSDK.NET_DVR_PLAYPAUSE, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue))
            {
                LastErrorCode = CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }
            _paused = true;
            return true;
        }

        public bool ContinuePlayback()
        {
            if (!_paused) return false;
            uint iOutValue = 0;
            if (!CHCNetSDK.NET_DVR_PlayBackControl_V40(_lPlayHandle, CHCNetSDK.NET_DVR_PLAYRESTART, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue))
            {
                LastErrorCode = CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }
            _paused = false;
            return true;
        }

        public void SlowPlayback()
        {
            uint iOutValue = 0;

            if (!CHCNetSDK.NET_DVR_PlayBackControl_V40(_lPlayHandle, CHCNetSDK.NET_DVR_PLAYSLOW, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue))
            {
                LastErrorCode = CHCNetSDK.NET_DVR_GetLastError();
            }
        }

        public void FastPlayback()
        {
            uint iOutValue = 0;

            if (!CHCNetSDK.NET_DVR_PlayBackControl_V40(_lPlayHandle, CHCNetSDK.NET_DVR_PLAYFAST, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue))
            {
                LastErrorCode = CHCNetSDK.NET_DVR_GetLastError();
            }
        }

        public void ResumePlayback()
        {
            uint iOutValue = 0;

            if (!CHCNetSDK.NET_DVR_PlayBackControl_V40(_lPlayHandle, CHCNetSDK.NET_DVR_PLAYNORMAL, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue))
            {
                LastErrorCode = CHCNetSDK.NET_DVR_GetLastError();
            }
        }

        public int GetPlaybackTpos()
        {
            uint iOutValue = 0;

            var lpOutBuffer = Marshal.AllocHGlobal(4);
            CHCNetSDK.NET_DVR_PlayBackControl_V40(_lPlayHandle, CHCNetSDK.NET_DVR_PLAYGETPOS, IntPtr.Zero, 0, lpOutBuffer, ref iOutValue);
            var pos = (int)Marshal.PtrToStructure(lpOutBuffer, typeof(int));
            Marshal.FreeHGlobal(lpOutBuffer);
            return pos;
        }

        public int GetDownloadTpos()
        {
            return CHCNetSDK.NET_DVR_GetDownloadPos(_lDownloadHandle);
        }

        public bool DownloadFileByName(string recordName, string fileName)
        {
            if (_lDownloadHandle > 0) return false;
            _lDownloadHandle = CHCNetSDK.NET_DVR_GetFileByName(_loginUserId, recordName, fileName);
            if (_lDownloadHandle < 0)
            {
                LastErrorCode = CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }

            uint iOutValue = 0;
            if (!CHCNetSDK.NET_DVR_PlayBackControl_V40(_lDownloadHandle, CHCNetSDK.NET_DVR_PLAYSTART, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue))
            {
                LastErrorCode = CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }

            return true;
        }

        public bool DOwnloadFileByTime(CHCNetSDK.NET_DVR_PLAYCOND struDownPara, string fileName)
        {
            _lDownloadHandle = CHCNetSDK.NET_DVR_GetFileByTime_V40(_loginUserId, fileName, ref struDownPara);
            if (_lDownloadHandle < 0)
            {
                LastErrorCode = CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }

            uint iOutValue = 0;
            if (!CHCNetSDK.NET_DVR_PlayBackControl_V40(_lDownloadHandle, CHCNetSDK.NET_DVR_PLAYSTART, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue))
            {
                LastErrorCode = CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }

            return true;
        }
    }
}
