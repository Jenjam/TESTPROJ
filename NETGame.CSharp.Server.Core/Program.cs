/*
 * Filename: Program.cs
 * Description: Main code file of the NETGame.CSharp.Server.Core project
 * Project: .NET Game, first project of the year, Team C
*/ 

// Adding dependencies
using System;
using System.Globalization;
using System.Net;
using System.Threading;

namespace NETGame.CSharp.Server.Core
{
    class Program
    {
        // Constructor
        static void Main(string[] args)
        {
            Services.Services servicesObject = new Services.Services();
            Console.WriteLine("Server Core - .NET Game - Team C");
            Console.WriteLine("==================================================");

            // Default IP and port
            var ipAddr = "127.0.0.1";
            var port = 32101;
            var tempString = "";

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
                Console.WriteLine("==================================================\n");
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
                Console.WriteLine("==================================================\n");
            }

            var status = false;
            var isDefault = false;
            Console.WriteLine("Now, let's setup the game. Do you want to set it up by default? (Type Y or N): ");
            var setup = Console.ReadLine();

            while (status == false)
            {
                if (setup == "Y" || setup == "y")
                {
                    status = true;
                    isDefault = true;
                }

                else if (setup == "N" || setup == "n")
                {
                    status = true;
                    isDefault = false;
                }

                else
                {
                    Console.WriteLine("Now, let's setup the game. Do you want to set it up by default? (Type Y or N): ");
                    setup = Console.ReadLine();
                }
            }

            if (isDefault == true)
            {
                Console.WriteLine("Game settings set to default, see documentation.");
            }

            else
            {
                Console.WriteLine("Enter game name: ");
                servicesObject.CurrentGame.GameName = Console.ReadLine();

                Console.WriteLine("Game duration (nbr of rounds): ");
                servicesObject.CurrentGame.GameTour = Int32.Parse(Console.ReadLine());

                Console.WriteLine("Money to start with: ");
                servicesObject.CurrentGame.GameStartMoney = float.Parse(Console.ReadLine(), CultureInfo.InvariantCulture.NumberFormat);

                Console.WriteLine("Nbr of players: ");
                servicesObject.CurrentGame.PlayerNeeded = Int32.Parse(Console.ReadLine());
            }

            Console.WriteLine("Every settings are set, now launching the servers...");
            Console.WriteLine("==================================================\n");

            launchThreads(ipAddr, port, servicesObject);
        }

        private static void launchThreads(string ip, int port, Services.Services servicesSrc)
        {
            // Creating new thread containing TCPServer
            Thread players = new Thread(delegate ()
            {
                // replace the IP with your system IP Address...
                Server myserver = new Server(ip, port, servicesSrc);
            });

            // Launching thread
            players.Start();


            // Creating new thread containing TCPServer
            Thread admin = new Thread(delegate ()
            {
                // replace the IP with your system IP Address...
                AdminServer adminServer = new AdminServer(ip, port + 1, servicesSrc);
            });

            // Launching thread
            admin.Start();

            Thread consoleAdmin = new Thread(delegate ()
            {
                // replace the IP with your system IP Address...
                ConsoleAdmin adminServer = new ConsoleAdmin(servicesSrc);
            });
            consoleAdmin.Start();
        }
    }
}
