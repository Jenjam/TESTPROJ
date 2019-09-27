using NETGame.CSharp.Entities;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;

namespace NETGame.CSharp.Client.Services
{
    public class ClientServices
    {
        Game _currentGame = new Game();
        Company _company = new Company();
        TcpClient _tcpClient;
        NetworkStream _stream;
        string _username;
        IPAddress _ipAddr;
        int _port;

        public Game CurrentGame { get => _currentGame; set => _currentGame = value; }
        public Company Company { get => _company; set => _company = value; }
        public TcpClient TcpClient { get => _tcpClient; set => _tcpClient = value; }
        public NetworkStream Stream { get => _stream; set => _stream = value; }
        public string Username { get => _username; set => _username = value; }
        public IPAddress IpAddr { get => _ipAddr; set => _ipAddr = value; }
        public int Port { get => _port; set => _port = value; }

        public ClientServices()
        {
            CurrentGame.GameName = "not sync with server";
        }

        public Request FunctionCode(string received)
        {
            var answerRequest = new Request();
            var request = JsonConvert.DeserializeObject<Request>(received);

            switch (request.Code)
            {
                case "getPlayer":
                    if (request.Type == "server")
                    {
                        Company = JsonConvert.DeserializeObject<Company>(request.Payload);
                    }
                    Console.WriteLine("Server> getPlayer request received from server");
                    break;

                case "refreshGame":
                    if (request.Type == "server")
                    {
                        CurrentGame = JsonConvert.DeserializeObject<Game>(request.Payload);
                    }
                    Console.WriteLine("Server> Game update received and applied.");
                    break;

                case "launchGame":
                    if (request.Type == "server")
                    {
                        answerRequest.Code = "launchGame";
                        answerRequest.Type = "server";
                        Company = JsonConvert.DeserializeObject<Company>(request.Payload);
                        answerRequest.Payload = "";
                    }
                    Console.WriteLine("Server> Game update received and applied.");
                    break;

                case "refreshAvailableDev":
                    if (request.Type == "server")
                    {
                        answerRequest.Code = "refreshAvailableDev";
                        answerRequest.Type = "client";
                        answerRequest.Payload = request.Payload;
                    }
                    Console.WriteLine("Server> Game update received and applied.");
                    break;

                default:
                    answerRequest.Code = "unknown";
                    answerRequest.Type = "client";
                    answerRequest.Payload = "Unknown request received. Check out documentation.";
                    Console.WriteLine("Server> Unknown request received from server");
                    break;
            }

            return answerRequest;
        }

        public Request CreateRequest(string source)
        {
            Request tmpRequest = new Request();

            tmpRequest.Type = "client";
            tmpRequest.Code = source;
            tmpRequest.Payload = "";

            switch (tmpRequest.Code)
            {
                case "refreshUser":
                    tmpRequest.Code = "getPlayer";
                    tmpRequest.Payload = Company.PlayerId.ToString();
                    Console.WriteLine("Client> A user refresh request as been sent to the server");
                    break;

                case "setUsername":
                    Console.Write("Please enter your new username: ");
                    tmpRequest.Payload = Console.ReadLine();
                    Console.WriteLine("New username: " + tmpRequest.Payload);
                    Console.WriteLine("Client> New username sent to server");
                    break;

                case "userInfo":
                    Console.WriteLine("Current user information: \n");
                    Console.WriteLine("Player ID: " + Company.PlayerId);
                    Console.WriteLine("Username: " + Company.CompanyName);
                    Console.WriteLine("Money owned: " + Company.MoneyOwned);
                    Console.WriteLine("List of developers owned:");
                    if (Company.DevelopersOwned.Count == 0)
                    {
                        Console.WriteLine("You don't have any developer yet!");
                    }
                    else
                    {
                        foreach (Developer dev in Company.DevelopersOwned)
                        {
                            Console.WriteLine("Developer name: " + dev.Name);
                        }
                    }
                    Console.WriteLine("List of projects owned:");
                    if (Company.ProjectOwned.Count == 0)
                    {
                        Console.WriteLine("You don't have any developer yet!");
                    }
                    else
                    {
                        foreach (Project project in Company.ProjectOwned)
                        {
                            Console.WriteLine("Project name: " + project.ProjectName);
                        }
                    }

                    tmpRequest.Code = "";
                    break;

                case "gameInfo":
                    Console.WriteLine("Current game information: \n");
                    if (CurrentGame.GameName == "not sync with server")
                    {
                        Console.WriteLine("Game as not been initialized/synchronized with the server.");
                    }

                    else
                    {
                        Console.WriteLine("Game title: " + CurrentGame.GameName);
                        if (CurrentGame.ActualRound == -1)
                        {
                            Console.WriteLine("Round: this game as not started yet.");
                        }

                        else
                        {
                            Console.WriteLine("Round: " + CurrentGame.ActualRound + " of " + CurrentGame.GameTour);
                        }
                        Console.WriteLine("Base money (per user): " + CurrentGame.GameStartMoney);
                        Console.WriteLine("Players needed to start the game: " + CurrentGame.PlayerNeeded + " players");
                    }

                    tmpRequest.Code = "";
                    break;

                case "refreshGame":
                    tmpRequest.Code = "refreshGame";
                    tmpRequest.Payload = "refreshGame";
                    Console.WriteLine("Client> A game refresh request as been sent to the server");
                    break;

                default:
                    tmpRequest = new Request();
                    break;
            }

            return tmpRequest;
        }
    }
}