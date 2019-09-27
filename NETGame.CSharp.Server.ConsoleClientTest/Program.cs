using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Newtonsoft.Json;
using NETGame.CSharp.Entities;
using NETGame.CSharp.Client.Services;

namespace NETGame.CSharp.Server.ConsoleClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ConsoleClientTest - .NET Game - Team C");
            Console.WriteLine("==================================================");

            // Default IP and port
            var ipAddr = "127.0.0.1";
            var port = 32101;
            var tempString = "";
            var username = "";

            // if some args were given at soft launch
            // First arg: IP Address of this server
            // Second arg: Server port
            // Default: 127.0.0.1 32101
            if (args.Length > 0 && args.Length == 2)
            {
                Console.WriteLine("Arg 0: " + args[0]);
                Console.WriteLine("Arg 1: " + args[1]);

                ipAddr = args[0];
                port = Int32.Parse(args[1]);

                Console.WriteLine("==================================================");
                Console.WriteLine("Selected @IP: " + IPAddress.Parse(ipAddr) + '\n');
                Console.WriteLine("Selected port: " + port + '\n');

                Console.WriteLine("Selected port: " + port + '\n');
                Console.Write("Please enter your username for this game : ");
                username = Console.ReadLine();
                Console.WriteLine("Every settings are set, now connecting to the server...");
            }

            // If no args were given
            else
            {
                // Prompt user to type in @IP and port
                Console.WriteLine("If you want to set it to default, don't type anything, and press ENTER.");
                Console.Write("Please enter server IP Address (default: localhost): ");

                ipAddr = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(ipAddr))
                {
                    ipAddr = "127.0.0.1";
                }

                Console.WriteLine("Selected @IP: " + IPAddress.Parse(ipAddr) + '\n');

                Console.Write("Please enter server port (default: 32101): ");

                tempString = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(tempString))
                {
                    port = 32101;
                }

                else
                {
                    port = Int32.Parse(tempString);
                }

                Console.WriteLine("Selected port: " + port + '\n');
                Console.Write("Please enter your username for this game : ");
                username = Console.ReadLine();

                Console.WriteLine("Every settings are set, now connecting to the server...");
            }
            Console.WriteLine("==================================================\n");

            new Thread(() =>
            {
                Connect(ipAddr, username, port);
            }).Start();
        }

        /// <summary>
        /// Send a message to the server, then reads server's answer
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="message"></param>
        public static void sendMessage(NetworkStream stream, Request message, ClientServices servicesSrc)
        {
            var serializedObject = JsonConvert.SerializeObject(message);
            // Encrypting message using our own Cryptography Class
            string encryptedString = Cryptography.EnryptString(serializedObject);
            Byte[] dataLong = Encoding.UTF8.GetBytes(encryptedString);

            // Sending data through Data Stream
            stream.Write(dataLong, 0, dataLong.Length);

            stream.Flush();

            // Reading response
            readMessage(stream, servicesSrc);
        }

        /// <summary>
        /// This method reads server's response
        /// </summary>
        /// <param name="stream"></param>
        public static void readMessage(NetworkStream stream, ClientServices servicesSrc)
        {
            Int32 bytes = 0;
            Byte[] dataLong = new Byte[4096];
            bytes = stream.Read(dataLong, 0, dataLong.Length);
            string response = Encoding.UTF8.GetString(dataLong, 0, bytes);
            string decryptedString = Cryptography.DecryptString(response);

            Request receivedRequest = servicesSrc.FunctionCode(decryptedString);
        }


        // Connect method
        static void Connect(String server, String firstMessage, Int32 port)
        {
            try
            {
                ClientServices servicesObject = new ClientServices();
                Request request = new Request();
                TcpClient client = new TcpClient(server, port);
                NetworkStream stream = client.GetStream();
                bool status = false;
                request.Code = "setUsername";
                request.Type = "client";
                request.Payload = firstMessage;

                // Sending first command, to get our dedicated Player object (also called Company)
                sendMessage(stream, request, servicesObject);

                do
                {
                    Thread.Sleep(500);

                    request = servicesObject.CreateRequest(Console.ReadLine());

                    if (request.Equals(default(Request)))
                    {
                        Console.WriteLine("invalid function code, please try again");
                    }

                    else if (request.Code != "")
                    {
                        sendMessage(stream, request, servicesObject);
                    }
                }
                while (status == false);

                stream.Close();
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Client> Exception: {0}", e);
            }
        }
    }
}