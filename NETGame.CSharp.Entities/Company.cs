using System;
using System.Collections.Generic;


namespace NETGame.CSharp.Entities
{
    public class Company
    {       
        private float _moneyOwned;
        private List<Project> _projectOwned;        
        private List<Developer> _developersOwned;
        private int _playerId;
        private string _companyName;
       
        public float MoneyOwned { get => _moneyOwned; set => _moneyOwned = value; }
        public List<Project> ProjectOwned { get => _projectOwned; set => _projectOwned = value; }
        public List<Developer> DevelopersOwned { get => _developersOwned; set => _developersOwned = value; }
        public int PlayerId { get => _playerId; set => _playerId = value; }
        public string CompanyName { get => _companyName; set => _companyName = value; }

        public Company()
        {
            _developersOwned = new List<Developer>();
            _projectOwned = new List<Project>();
        }

        public Company(int playerID)
        {
            _developersOwned = new List<Developer>();
            _projectOwned = new List<Project>();
            PlayerId = playerID;
            CompanyName = "Default name";
            MoneyOwned = 100000.0f;
        }

        public Company(int playerID, string playerName)
        {
            _developersOwned = new List<Developer>();
            _projectOwned = new List<Project>();
            PlayerId = playerID;
            CompanyName = playerName;
            MoneyOwned = 100000.0f;
        }

        public Company(float starterMoney, int idPlayer, string companyName):this()
        {
            _moneyOwned = starterMoney;
            _playerId = idPlayer;
            _companyName = companyName;
        }
    }
}
