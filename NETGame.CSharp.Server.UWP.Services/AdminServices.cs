using NETGame.CSharp.Entities;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;

namespace NETGame.CSharp.Client.Services
{
    public class AdminServices
    {
        Game _currentGame = new Game();
        TcpClient _tcpClient;
        NetworkStream _stream;
        IPAddress _ipAddr;
        int _port;

        public Game CurrentGame { get => _currentGame; set => _currentGame = value; }
        public TcpClient TcpClient { get => _tcpClient; set => _tcpClient = value; }
        public NetworkStream Stream { get => _stream; set => _stream = value; }
        public IPAddress IpAddr { get => _ipAddr; set => _ipAddr = value; }
        public int Port { get => _port; set => _port = value; }

        public AdminServices()
        {
            CurrentGame.GameName = "not sync with server";
        }

        public Request FunctionCode(string received)
        {
            var answerRequest = new Request();
            var request = JsonConvert.DeserializeObject<Request>(received);

            switch (request.Code)
            {
                case "setGame":
                    if (request.Type == "server")
                    {
                        CurrentGame = JsonConvert.DeserializeObject<Game>(request.Payload);
                        Console.WriteLine("Server> setGame request");
                        Console.WriteLine("Server> Game set successfully! Here are all game info:\n\n\n");
                        Console.WriteLine("Game Info> Game title: " + CurrentGame.GameName);
                        Console.WriteLine("Game Info> Money to start with: " + CurrentGame.GameStartMoney);
                        Console.WriteLine("Game Info> Actual round: " + CurrentGame.ActualRound + " of " + CurrentGame.GameTour);
                        Console.WriteLine("Game Info> Number of players: " + CurrentGame.PlayerList.Count + " of " + CurrentGame.PlayerNeeded);
                    }
                    break;

                default:
                    answerRequest.Code = "unknown";
                    answerRequest.Type = "admin";
                    answerRequest.Payload = "Unknown request received. Check out documentation.";
                    Console.WriteLine("Server> Unknown request received from server");
                    break;
            }

            return answerRequest;
        }

        public Request CreateRequest(string source)
        {
            Request tmpRequest = new Request();

            tmpRequest.Type = "admin";
            tmpRequest.Code = source;
            tmpRequest.Payload = "";

            switch (tmpRequest.Code)
            {
                case "setGame":
                    tmpRequest.Payload = "setGame";
                    break;

                default:
                    tmpRequest = new Request();
                    break;
            }

            return tmpRequest;
        }
    }
}