using System;
using System.Collections.Generic;
using System.Text;

namespace NETGame.CSharp.Entities
{
    public class Project
    {
        private string _projectName;
        private float _remuneration;
        private int _duration;
        private List<Developer> _developersAttribuated;
        private List<Skill> _skillNeeded;
        private int _playerID;
        private float _failRate;

        public string ProjectName { get => _projectName; set => _projectName = value; }
        public int Duration { get => _duration; set => _duration = value; }
        public int PlayerID { get => _playerID; set => _playerID = value; }
        public float FailRate { get => _failRate; set => _failRate = value; }

        public Project()
        {
        }

        public Project(string projectName, string speciality, float totalCost, float prizeWon, int duration, int minDev, int maxDev, int playerID, float failRate):this()
        {
            _projectName = projectName;
            _duration = duration;
            _playerID = playerID;
            _failRate = failRate;
        }
    }
}
