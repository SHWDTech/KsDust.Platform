using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using SHWDTech.Platform.Utility;

namespace Ks.Dust.Protocol.Tester
{
    class Program
    {
        private static string baseStr =
            "QN={0};ST=22;CN={1};PW=123456;MN={2};CP=&&DataTime={3};a34001-Avg={4};a34004-Avg={5};a34005-Avg={6};a50001-Avg={7};a01001-Avg={8};a01002-Avg={9};a01007-Avg={10};a01008-Avg={11}&&";

        private static readonly Dictionary<string, Socket> ClientSockets = new Dictionary<string, Socket>();

        private static int _port = 1000;

        private static string[] _devices;

        private static int _count;

        static void Main(string[] args)
        {
            _devices = new[]
            {
                "V87AS7F0000001",
                "V87AS7F0000002",
                "V87AS7F0000003",
                "V87AS7F0000004",
                "V87AS7F0000005",
                "V87AS7F0000006",
                "V87AS7F0000007",
                "V87AS7F0000008",
                "V87AS7F0000009",
                "V87AS7F0000010",
                "V87AS7F0000011"
            };

            Console.ReadKey();
            Connect();
            while (true)
            {
                Send();
                Thread.Sleep(60000);
                _count++;
                if (_count % 15 == 0)
                {
                    SendFifteen();
                }
                if (_count % 60 == 0)
                {
                    SendHour();
                }
            }
        }

        static void Connect()
        {
            foreach (var t in _devices)
            {
                var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Bind(new IPEndPoint(IPAddress.Parse("192.168.1.105"), _port));
                _port++;
                client.Connect(new IPEndPoint(IPAddress.Parse("192.168.1.105"), 18254));
                client.Send(GetData(t, "2011"));
                ClientSockets.Add(t, client);
                Thread.Sleep(100);
            }
        }

        static void Send()
        {
            foreach (var clientSocket in ClientSockets)
            {
                clientSocket.Value.Send(GetData(clientSocket.Key, "2011"));
            }
        }

        static void SendFifteen()
        {
            foreach (var clientSocket in ClientSockets)
            {
                clientSocket.Value.Send(GetData(clientSocket.Key, "2051"));
            }
        }

        static void SendHour()
        {
            foreach (var clientSocket in ClientSockets)
            {
                clientSocket.Value.Send(GetData(clientSocket.Key, "2061"));
            }
        }

        static byte[] GetData(string nodeId, string cmd)
        {
            var qnStr = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var dateStr = DateTime.Now.ToString("yyyyMMddHHmmss");
            var dataStr = string.Format(baseStr, qnStr, cmd, nodeId, dateStr, RandomDouble(0, 1), RandomDouble(0, 1), RandomDouble(0, 1), RandomDouble(50, 80), RandomDouble(20, 30), RandomDouble(25, 70), RandomDouble(0, 15), RandomDouble(0, 360));
            var crc = Globals.GetCrcModBus(Encoding.ASCII.GetBytes(dataStr));
            dataStr = $"##{dataStr.Length:D4}{dataStr}";
            dataStr += crc;
            dataStr += "\r\n";
            return Encoding.ASCII.GetBytes(dataStr);
        }

        static string RandomDouble(int start, int end)
        {
            var rd = new Random();
            return (rd.Next(start * 100, end * 100) / 100.0).ToString("F1");
        }
    }
}
