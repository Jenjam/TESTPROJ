using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NETGame.CSharp.Server.Services
{
    public class TCPServer
    {
        TcpListener _server;
        IPAddress _ipAddr;
        int _port;

        public TcpListener Server { get => _server; set => _server = value; }
        public IPAddress IpAddr { get => _ipAddr; set => _ipAddr = value; }
        public int Port { get => _port; set => _port = value; }

        public TCPServer()
        {

        }
    }
}
