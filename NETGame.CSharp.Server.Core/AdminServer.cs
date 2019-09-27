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
    class AdminServer
    {
        // Will listen for TCP Connexion
        TcpListener server = null;

        // Global Variables
        IPAddress _ipAddr;
        int _port;
        string serverNameInConsole = "Admin Server";

        Services.Services servicesObject;

        // Constructor
        public AdminServer(string ip, int port, Services.Services servicesSrc)
        {
            servicesObject = servicesSrc;
            _ipAddr = IPAddress.Parse(ip);
            _port = port;

            server = new TcpListener(_ipAddr, _port);
            server.Start();

            printLog(serverNameInConsole, "Administration server successfully started!");

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
                    AdminThread adminThreadObject = new AdminThread(servicesObject);

                    Thread adminThread = new Thread(new ParameterizedThreadStart(adminThreadObject.HandleDevice));
                    adminThread.Start(client);
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