using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Security.Cryptography;
using System.Linq;
using Newtonsoft.Json;
using NETGame.CSharp.Entities;
using NETGame.CSharp.Server.Services;
using System.Collections.Generic;

namespace NetGameUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetNameAPITestMethod()
        {
            Game game = new Game
            {
                ActualRound = 0,
                GameName = "GameTest",
                GameStartMoney = 100000,
                GameTour = 10,
                PlayerList = new List<Company>
                {
                    new Company
                    {
                        CompanyName = "Compagny1",
                        DevelopersOwned = new List<Developer>(),
                        MoneyOwned = 100000,
                        PlayerId = 1,
                        ProjectOwned = new List<Project>()
                    },
                    new Company
                    {
                        CompanyName = "Compagny2",
                        DevelopersOwned = new List<Developer>(),
                        MoneyOwned = 100000,
                        PlayerId = 2,
                        ProjectOwned = new List<Project>()
                    }
                },
                PlayerNeeded = 2

            };
            Services services = new Services();
            services.GenerateNames(5);
        }
    }
}


