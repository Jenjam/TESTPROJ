using System.Collections.Generic;
using System.Linq;

namespace NETGame.CSharp.Entities
{
    public class Game
    {
        private string _gameName;
        private int _gameTour;
        private float _gameStartMoney;
        private int _playersNeeded;
        private List<Company> _playerList;
        private List<Developer> _developerList;
        private List<School> _schoolList;
        private List<Project> _projectList;
        private List<Skill> _skillList;
        private List<Certification> _certificationList;
        private int _actualRound;
        private int _developerPerRound;
        private int _projectPerRound;

        public string GameName { get => _gameName; set => _gameName = value; }
        public int GameTour { get => _gameTour; set => _gameTour = value; }
        public float GameStartMoney { get => _gameStartMoney; set => _gameStartMoney = value; }
        public List<Company> PlayerList { get => _playerList; set => _playerList = value; }
        public int PlayerNeeded { get => _playersNeeded; set => _playersNeeded = value; }
        public int ActualRound { get => _actualRound; set => _actualRound = value; }
        public List<Developer> DeveloperList { get => _developerList; set => _developerList = value; }
        public int DeveloperPerRound { get => _developerPerRound; set => _developerPerRound = value; }
        public int ProjectPerRound { get => _projectPerRound; set => _projectPerRound = value; }

        public Game()
        {
            _gameName = "Default name";
            _gameTour = 10;
            _gameStartMoney = 50000.0f;
            _playerList = new List<Company>();
            _developerList = new List<Developer>();
            _schoolList = new List<School>();
            _projectList = new List<Project>();
            _skillList = new List<Skill>();
            _certificationList = new List<Certification>();
            _actualRound = -1;
            _developerPerRound = 1;
            _projectPerRound = 1;
        }

        public Game(string gameName, int gameTour, float gameMoney, int playersNeeded, int devPerRound, int projPerRound) : this()
        {
            _gameName = gameName;
            _gameTour = gameTour;
            _gameStartMoney = gameMoney;
            _playersNeeded = playersNeeded;
            _developerPerRound = devPerRound;
            _projectPerRound = projPerRound;
        }

        public void addPlayer(Company companySrc) => PlayerList.Add(companySrc);

        public void deletePlayer(Company player) => PlayerList.Remove(player);

        public Company getPlayer(int playerID) => PlayerList.Where(p => p.PlayerId == playerID).ToList()[0];
    }
}
