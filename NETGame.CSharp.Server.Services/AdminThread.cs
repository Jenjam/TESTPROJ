using NETGame.CSharp.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NETGame.CSharp.Server.Services
{
    public class AdminThread
    {
        private int _threadId;
        private Services _services;

        public int ThreadId { get => _threadId; set => _threadId = value; }

        public AdminThread(Services servicesSrc)
        {
            _services = servicesSrc;
        }

        // Method used by each thread
        public void HandleDevice(Object obj)
        {
            _threadId = Thread.CurrentThread.ManagedThreadId;

            try
            {
                TcpClient client = (TcpClient)obj;
                NetworkStream stream = client.GetStream();
                bool status = false;
                string messageToSend;

                // Sending first command, to get our dedicated Player object (also called Company)
                messageToSend = readMessage(stream, _threadId);
                sendMessage(stream, messageToSend, _threadId);

                // Sending first command, to get our dedicated Player object (also called Company
                messageToSend = readMessage(stream, _threadId);
                sendMessage(stream, messageToSend, _threadId);

                do
                {
                    messageToSend = readMessage(stream, _threadId);
                    sendMessage(stream, messageToSend, _threadId);
                }
                while (status == false);

                stream.Close();
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Admin> Exception: {0}", e.Message);
                Console.WriteLine("Admin> Admin " + _threadId + " disconnected.");
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
                case "setGame":
                    if (request.Type == "admin")
                    {
                        _services.CurrentGame = JsonConvert.DeserializeObject<Game>(request.Payload);
                        answerRequest.Code = "getGame";
                        answerRequest.Type = "server";
                        answerRequest.Payload = JsonConvert.SerializeObject(_services.CurrentGame);
                        Console.WriteLine("Server> setGame request from admin " + threadID);
                        Console.WriteLine("Server> getGame and send to admin " + threadID);
                    }
                    break;

                case "getGame":
                    if (request.Type == "admin")
                    {
                        answerRequest.Code = "getGame";
                        answerRequest.Type = "server";
                        answerRequest.Payload = JsonConvert.SerializeObject(_services.CurrentGame);
                        Console.WriteLine("Server> getGame request from admin " + threadID);
                        Console.WriteLine("Server> Game set and send to admin " + threadID);
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
