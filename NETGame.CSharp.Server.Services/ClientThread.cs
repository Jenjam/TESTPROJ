using NETGame.CSharp.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NETGame.CSharp.Server.Services
{
    public class ClientThread
    {
        private int _threadId;
        private Company _company;
        private bool _isConnected;
        private Services _services;
        private NetworkStream _stream;

        public int ThreadId { get => _threadId; set => _threadId = value; }
        public Company PlayerCompany { get => _company; set => _company = value; }
        public bool IsConnected { get => _isConnected; set => _isConnected = value; }
        public NetworkStream Stream { get => _stream; set => _stream = value; }

        public ClientThread(Services servicesSrc)
        {
            _isConnected = false;
            _services = servicesSrc;
        }

        // Method used by each thread
        public void HandleDevice(Object obj)
        {
            _threadId = Thread.CurrentThread.ManagedThreadId;

            try
            {
                TcpClient client = (TcpClient)obj;
                Stream = client.GetStream();
                _isConnected = true;
                bool status = false;
                string messageToSend;

                do
                {
                    messageToSend = readMessage(Stream, _threadId);
                    sendMessage(Stream, messageToSend, _threadId);
                }
                while (status == false);

                Stream.Close();
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Server> Exception: {0}", e.Message);
                Console.WriteLine("Server> Client " + _threadId + " disconnected.");

                Company temp = null;

                foreach(Company cmp in _services.CurrentGame.PlayerList)
                {
                    if(cmp.PlayerId == ThreadId)
                    {
                        temp = cmp;
                    }
                }

                if(temp != null)
                {
                    _services.CurrentGame.PlayerList.Remove(temp);
                }

                _isConnected = false;
                Thread.CurrentThread.Interrupt();
            }
        }

        /// <summary>
        /// Send a message to the server, then reads server's answer
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="message"></param>
        public void sendMessage(NetworkStream stream, string message, int threadID)
        {
            // Encrypting message using our own Cryptography Class
            string encryptedString = Cryptography.EnryptString(message);
            Byte[] dataLong = Encoding.UTF8.GetBytes(encryptedString);

            // Sending data through Data Stream
            stream.Write(dataLong, 0, dataLong.Length);

            stream.Flush();
        }

        /// <summary>
        /// This method reads server's response
        /// </summary>
        /// <param name="stream"></param>
        public string readMessage(NetworkStream stream, int threadID)
        {
            Int32 bytes = 0;
            Byte[] dataLong = new Byte[4096];
            bytes = stream.Read(dataLong, 0, dataLong.Length);
            string response = Encoding.UTF8.GetString(dataLong, 0, bytes);
            string decryptedString = FunctionCode(Cryptography.DecryptString(response), threadID);
            stream.Flush();
            return decryptedString;
        }

        public string FunctionCode(string received, int threadID)
        {
            var answerRequest = new Request();
            var request = JsonConvert.DeserializeObject<Request>(received);

            switch (request.Code)
            {
                case "createUser":
                    if (request.Type == "client")
                    {
                        Company newCompany = new Company(_services.CurrentGame.GameStartMoney, threadID, request.Payload);
                        _services.CurrentGame.addPlayer(newCompany);
                        answerRequest.Code = "createUser";
                        answerRequest.Type = "server";
                        answerRequest.Payload = "success";
                        Console.WriteLine("Server> createUser request from client " + threadID);
                        Console.WriteLine("Server> Sent success answer to client " + threadID);
                    }
                    break;

                case "setUsername":
                    if (request.Type == "client")
                    {
                        PlayerCompany.CompanyName = request.Payload;
                        answerRequest.Code = "getPlayer";
                        answerRequest.Type = "server";
                        answerRequest.Payload = JsonConvert.SerializeObject(_company);
                        Console.WriteLine("Server> setUsername request from client " + threadID);
                        Console.WriteLine("Server> Sent getPlayer answer to client " + threadID);
                    }
                    break;

                case "getPlayer":
                    if (request.Type == "client")
                    {
                        answerRequest.Code = "getPlayer";
                        answerRequest.Type = "server";
                        answerRequest.Payload = "";

                        Console.WriteLine("Server> getPlayer request from client " + threadID);
                        answerRequest.Payload = JsonConvert.SerializeObject(_company);
                        Console.WriteLine("Server> Sent getPlayer answer to client " + threadID);
                    }
                    break;

                case "refreshGame":
                    if (request.Type == "client")
                    {
                        answerRequest.Code = "refreshGame";
                        answerRequest.Type = "server";
                        answerRequest.Payload = JsonConvert.SerializeObject(_services.CurrentGame);

                        Console.WriteLine("Server> refreshGame request from client " + threadID);
                        Console.WriteLine("Server> Sent Game Object to client " + threadID);
                    }
                    break;

                case "launchGame":
                    if (request.Type == "client")
                    {
                        answerRequest.Code = "launchGame";
                        answerRequest.Type = "server";
                        answerRequest.Payload = "launchGame";

                        Console.WriteLine("Server> launchGame request from client " + threadID);
                        Console.WriteLine("Server> Sent launchGame cmd to client " + threadID);
                    }
                    break;

                default:
                    answerRequest.Code = "unknown";
                    answerRequest.Type = "server";
                    answerRequest.Payload = "Unknown request received. Check out documentation.";
                    Console.WriteLine("Server> Unknown request received from user " + threadID);
                    break;
            }

            return JsonConvert.SerializeObject(answerRequest);
        }
    }
}
