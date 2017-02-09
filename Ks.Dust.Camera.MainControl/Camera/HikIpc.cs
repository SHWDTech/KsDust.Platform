using System.Runtime.InteropServices;
using System.Text;
using Dust.Platform.Storage.Model;

namespace Ks.Dust.Camera.MainControl.Camera
{
    /// <summary>
    /// 海康摄像机操作类
    /// </summary>
    internal class HikIpc
    {
        public uint LastErrorCode { get; private set; }

        private string _dvrAddress = string.Empty;

        private ushort _dvrPortNumber = 8000;

        private int _loginUserId;

        private uint _dwAChanTotalNum;

        private uint _dwDChanTotalNum;

        private readonly int[] _iIpDevId = new int[96];

        private readonly int[] _iChannelNum = new int[96];

        private CHCNetSDK.NET_DVR_DEVICEINFO_V30 _deviceInfo;

        private CHCNetSDK.NET_DVR_IPPARACFG_V40 _struIpParaCfgV40;

        private CHCNetSDK.NET_DVR_IPCHANINFO _struChanInfo;

        private CHCNetSDK.NET_DVR_IPCHANINFO_V40 _struChanInfoV40;

        public bool Login(CameraLogin loginInfo)
        {
            var ipAddress = new byte[16];
            uint dwPort = 0;
            if (!CHCNetSDK.NET_DVR_GetDVRIPByResolveSvr_EX("www.hik-online.com", 80, loginInfo.DomainBytes, (ushort)loginInfo.DomainBytes.Length,
                    null, 0, ipAddress, ref dwPort))
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
    }
}
