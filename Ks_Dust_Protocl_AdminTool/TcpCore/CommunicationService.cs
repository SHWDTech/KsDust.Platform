using System;
using System.Net;
using Ks_Dust_Protocl_AdminTool.Common;
using SHWDTech.Platform.ProtocolService;
using SHWDTech.Platform.ProtocolService.DataBase;

namespace Ks_Dust_Protocl_AdminTool.TcpCore
{
    public class CommunicationService : TcpServiceHost
    {
        /// <summary>
        /// 服务器IP地址
        /// </summary>
        public IPEndPoint ServerIpEndPoint { get; private set; }

        /// <summary>
        /// 活跃的客户端连接数
        /// </summary>
        public int AliveConnection => ActiveClients.Count;

        /// <summary>
        /// 已经解析成功的协议总数
        /// </summary>
        public int DecodedProtocol { get; private set; }

        public override void Start()
        {
            try
            {
                base.Start();
                ReportService.Instance.Info("服务器启动成功！");
            }
            catch (Exception ex)
            {
                ReportService.Instance.Warning("服务器启动失败!", ex);
            }
        }

        public override void Close()
        {
            base.Close();
            ReportService.Instance.Info("服务器关闭成功。");
        }

        public override void SetupTcpAddressPort(IPEndPoint endPoint)
        {
            base.SetupTcpAddressPort(endPoint);
            ServerIpEndPoint = endPoint;
        }

        protected override void AddClient(IActiveClient client)
        {
            client.ClientAuthenticated += ClientAuthenticated;
            client.ClientAuthenticateFailed += ClientAuthenticaFaild;
            client.ClientDisconnect += ClientDisconnected;
            client.ClientDecodeFalied += ClientDecodeFailed;
            client.ClientDecoded += ClientDecoded;
            base.AddClient(client);
        }

        protected override void OnClientAccedped(IActiveClient client)
        {
            base.OnClientAccedped(client);
            ReportService.Instance.Info($"接收客户端连接请求成功，连接建立成功。客户端地址：{client.ClientAddress}");
        }

        protected override void OnClientAcceptFailed(Exception ex)
        {
            base.OnClientAcceptFailed(ex);
            ReportService.Instance.Info($"接收客户端连接请求失败,服务已经尝试重启。{ex}");
        }

        private void ClientDecoded(ActiveClientEventArgs args)
        {
            DecodedProtocol++;
            using (var ctx = new ProtocolContext())
            {
                ctx.ProtocolDatas.Add((ProtocolData)args.ProtocolData);
                ctx.SaveChanges();
            }
        }

        private static void ClientAuthenticated(ActiveClientEventArgs args)
        {
            ReportService.Instance.Info($"客户端授权通过，设备地址：{args.SourceActiveClient.ClientAddress}，设备ID号：{args.SourceActiveClient.ClientIdentity}");
        }

        private static void ClientAuthenticaFaild(ActiveClientEventArgs args)
        {
            ReportService.Instance.Info($"客户端授权失败，设备地址：{args.SourceActiveClient.ClientAddress}，设备ID号：{args.SourceActiveClient.ClientIdentity}");
        }

        private static void ClientDisconnected(ActiveClientEventArgs args)
        {
            ReportService.Instance.Info($"客户端连接断开，设备地址：{args.SourceActiveClient.ClientAddress}，设备ID号：{args.SourceActiveClient.ClientIdentity}，异常信息：{args.ExceptionMessage}", args.Exception);
        }

        private static void ClientDecodeFailed(ActiveClientEventArgs args)
        {
            ReportService.Instance.Error($"客户端解码失败，设备地址：{args.SourceActiveClient.ClientAddress}，设备ID号：{args.SourceActiveClient.ClientIdentity}，异常信息：{args.ExceptionMessage}", args.Exception);
        }
    }
}
