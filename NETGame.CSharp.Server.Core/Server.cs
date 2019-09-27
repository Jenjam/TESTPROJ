/*
 * Filename: Server.cs
 * Description: Main code file of the NETGame.CSharp.Server.Core project
 * Project: Server code
*/

// Adding dependencies
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using NETGame.CSharp.Entities;
using NETGame.CSharp.Server.Services;
using Newtonsoft.Json;

namespace NETGame.CSharp.Server.Core
{
    class Server
    {
        // Will listen for TCP Connexion
        TcpListener server = null;

        // Global Variables
        IPAddress _ipAddr;
        int _port;
        string serverNameInConsole = "Server";

        Services.Services servicesObject;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="servicesSrc"></param>
        public Server(string ip, int port, Services.Services servicesSrc)
        {
            servicesObject = servicesSrc;

            _ipAddr = IPAddress.Parse(ip);
            _port = port;

            server = new TcpListener(_ipAddr, _port);
            server.Start();

            printLog(serverNameInConsole, "server successfully started!");

            // Method called by the thread forever
            StartListener();
        }

        // Method called when you want to print something in the console
        private void printLog(string entity, string message)
        {
            Console.WriteLine(entity + "> " + message);
        }

        // Code called all the time by the thread, dedicated by client
        public void StartListener()
        {
            try
            {
                // Infinite loop while a client is connected
                while (true)
                {
                    printLog(serverNameInConsole, "Waiting for clients...");

                    // Create a TCPClient when a client will try to connect to the server
                    TcpClient client = server.AcceptTcpClient();

                    printLog(serverNameInConsole, "new client connected! Initializing thread...");

                    // New thread by client
                    ClientThread clientThreadObject = new ClientThread(servicesObject);
                    List<ClientThread> threadsToDelete = new List<ClientThread>();
                    foreach (ClientThread threads in servicesObject.ClientList)
                    {
                        if (!threads.IsConnected)
                        {
                            threadsToDelete.Add(threads);
                        }
                    }

                    foreach (ClientThread threads in threadsToDelete)
                    {
                        servicesObject.ClientList.Remove(threads);
                    }

                    servicesObject.ClientList.Add(clientThreadObject);

                    Thread clientThread = new Thread(new ParameterizedThreadStart(clientThreadObject.HandleDevice));
                    clientThread.Start(client);
                    printLog(serverNameInConsole, "new thread successfully created!");
                }
            }

            catch (Exception ex)
            {
                printLog(serverNameInConsole, "Error while initializing new thread, error: " + ex.Message);
            }
        }
    }
}