using System;
using Newtonsoft.Json;
using NETGame.CSharp.Entities;
using System.Collections.Generic;

namespace NETGame.CSharp.Server.Services
{
    public class Services
    {
        private Game _currentGame;
        private List<ClientThread> _clientList;
        private DeveloperInstantation _developerInstantation;
        private List<string> _names;
        private TCPServer _tcpServer;

        public Game CurrentGame { get => _currentGame; set => _currentGame = value; }
        public List<ClientThread> ClientList { get => _clientList; set => _clientList = value; }
        public DeveloperInstantation DeveloperInstantation { get => _developerInstantation; set => _developerInstantation = value; }
        public TCPServer TcpServer { get => _tcpServer; set => _tcpServer = value; }

        public Services()
        {
            CurrentGame = new Game();
            _clientList = new List<ClientThread>();
            _developerInstantation = new DeveloperInstantation();
            _tcpServer = new TCPServer();
        }

        public void AddPlayer(ClientThread playerThread)
        {
            _clientList.Add(playerThread);
            _currentGame.PlayerList.Add(playerThread.PlayerCompany);
        }

        public void DeleteThread(ClientThread playerThread) => _clientList.Remove(playerThread);

        public List<string> GenerateNames(int numberOfNamesNeeded)
        {
            try
            {
                return _developerInstantation.GetNameAPI(numberOfNamesNeeded);
            }
            catch (Exception)
            {
                //In case there is no internet connexion but and the game is on a local network, use a json file to create the names
                return _developerInstantation.GetNameJson(numberOfNamesNeeded);
            }
        }
        /// <summary>
        /// Create a random number of developpers 
        /// </summary>
        /// <param name="numberOfDev"></param>
        /// <param name="round"></param>
        /// <param name="certifMin"></param>
        public void CreateDeveloppersGame(int numberOfDev, int round, int certifMin)
        {
            for (int i = 0; i < numberOfDev; i++)
            {
                _currentGame.DeveloperList.Add(new Developer
                {
                    Name = GenerateNames(1)[0],
                    Cert = CreateCertifications(round, certifMin, GenerateNames(1)[0]),
                    School = "Lycée Gustave Eiffel",
                    Skills = new List<Skill>(),
                    Spec = "Unknown",
                    CostPerMonth = 2000.0f,
                    Status = 0
                });
            }
        }
        /// <summary>
        /// Create a random number of certifications
        /// </summary>
        /// <param name="numberMaxOfCertif"></param>
        /// <param name="minNivCertif"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Certification> CreateCertifications(int numberMaxOfCertif, int minNivCertif, string name)
        {
            List<Certification> certifications = new List<Certification>();
            int random = new Random((int)DateTime.Now.Ticks).Next(0, numberMaxOfCertif);
            for (int i = 0; i < random; i++)
            {
                certifications.Add(new Certification
                {
                    CertificationID = i,
                    CertificationMaxLevel = new Random((int)DateTime.Now.Ticks).Next(2, 3),
                    CertificationMinLevel = new Random((int)DateTime.Now.Ticks).Next(0, minNivCertif),
                    CertificationName = name + new Random((int)DateTime.Now.Ticks).Next(0, 1000)
                });
            }
            return certifications;

        }
    }

}