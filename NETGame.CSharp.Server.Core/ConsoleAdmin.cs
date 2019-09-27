using NETGame.CSharp.Entities;
using NETGame.CSharp.Server.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETGame.CSharp.Server.Core
{
    public class ConsoleAdmin
    {
        private Services.Services _services;

        public ConsoleAdmin(Services.Services servicesSrc)
        {
            _services = servicesSrc;
            CommandHandler();
        }

        private void CommandHandler()
        {
            try
            {
                // Infinite loop while a client is connected
                while (true)
                {
                    Console.WriteLine(FunctionCode(Console.ReadLine()));
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Console> CRITICAL ERROR:" + ex.Message);
            }
        }

        private string FunctionCode(string cmd)
        {
            var cmdToReturn = "";
            Request requestTmp;

            switch (cmd)
            {
                case "gameInfo":
                    cmdToReturn = "Local> Current game info :\n" +
                                  "Game title: " + _services.CurrentGame.GameName + '\n' +
                                  "Current round: " + _services.CurrentGame.ActualRound + " of " + _services.CurrentGame.GameTour + '\n' +
                                  "Money at begining: " + _services.CurrentGame.GameStartMoney + '\n' +
                                  "N° of players: " + _services.CurrentGame.PlayerList.Count + '\n' +
                                  "Player needed to start the game: " + _services.CurrentGame.PlayerNeeded + '\n' +
                                  "N° of dev/round: " + _services.CurrentGame.DeveloperPerRound + '\n' +
                                  "N° of projects/round: " + _services.CurrentGame.ProjectPerRound + '\n';
                    break;

                case "connectedPlayers":
                    cmdToReturn = "Local> Connected players:\n";
                    foreach(Company cmp in _services.CurrentGame.PlayerList)
                    {
                        cmdToReturn = cmdToReturn + "Player " + cmp.PlayerId + '\n';
                    }
                    break;

                case "connectedThreads":
                    cmdToReturn = "Local> Connected threads:\n";
                    foreach (ClientThread thd in _services.ClientList)
                    {
                        if (thd.IsConnected == true)
                        {
                            cmdToReturn = cmdToReturn + "Thread " + thd.ThreadId + '\n';
                        }        
                    }
                    break;

                case "listThreads":
                    cmdToReturn = "Local> List of registered threads:\n";
                    foreach (ClientThread thd in _services.ClientList)
                    {
                        var temp = "";

                        if(thd.IsConnected == true)
                        {
                            temp = "connected";
                        }

                        else
                        {
                            temp = "disconnected";
                        }

                        cmdToReturn = cmdToReturn + "Thread " + thd.ThreadId + " - " + temp + '\n';
                    }
                    break;

                case "sendAll":
                    cmdToReturn = "Local> Sent launchGame arg to every connected players";
                    requestTmp = new Request("launchGame", "server", "");
                    foreach (ClientThread thd in _services.ClientList)
                    {
                        foreach(Company cmp in _services.CurrentGame.PlayerList)
                        {
                            if(cmp.PlayerId == thd.ThreadId)
                            {
                                requestTmp.Payload = JsonConvert.SerializeObject(cmp);
                                thd.sendMessage(thd.Stream, JsonConvert.SerializeObject(requestTmp), thd.ThreadId);
                            }
                        }
                    }
                    break;

                case "refreshAvailableDev":
                    cmdToReturn = "Local> Here are connected players:\n";
                    foreach (Company cmp in _services.CurrentGame.PlayerList)
                    {
                        cmdToReturn = cmdToReturn + "Player " + cmp.PlayerId + " - " + cmp.CompanyName + '\n';
                    }
                    cmdToReturn = cmdToReturn + "What player do you want to send cmd (ID)? ";
                    Console.WriteLine(cmdToReturn);
                    int playId = Int32.Parse(Console.ReadLine());

                    requestTmp = new Request("refreshAvailableDev", "server", "refreshAvailableDev");
                    foreach (ClientThread thd in _services.ClientList)
                    {
                        if (thd.ThreadId == playId)
                        {
                            requestTmp.Payload = JsonConvert.SerializeObject(_services.CurrentGame.DeveloperList);
                            thd.sendMessage(thd.Stream, JsonConvert.SerializeObject(requestTmp), thd.ThreadId);
                        }
                    }
                    cmdToReturn = "Local> Sent refreshAvailableDev to player " + playId;
                    break;

                case "regenerateDevs":
                    _services.CurrentGame.DeveloperList = new List<Developer>();
                    _services.CreateDeveloppersGame(10, 1, 1);
                    cmdToReturn = "Local> Available developers regenerated.";
                    break;

                default:
                    cmdToReturn = "Local> Unknown command. Please check the documentation.";
                    break;
            }

            return cmdToReturn; 
        }
    }
}
