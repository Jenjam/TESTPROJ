using System.Collections.Generic;

namespace NETGame.CSharp.Entities
{
    public class SessionSkill
    {
        private string _sessionName;
        private School _school;
        private float _sessionCostPerDev;
        private int _duration;
        private List<Certification> _certifications;
        private List<Developer> _developers;


        public string SessionName { get => _sessionName; set => _sessionName = value; }        
        public float SessionCostPerDev { get => _sessionCostPerDev; set => _sessionCostPerDev = value; }
        public int Duration { get => _duration; set => _duration = value; }      
        public List<Certification> Cert { get => _certifications; set => _certifications = value; }
        public List<Developer> Dev { get => _developers; set => _developers = value; }
        public School School { get => _school; set => _school = value; }

        public SessionSkill()
        {
            _developers = new List<Developer>();
            _certifications = new List<Certification>();
        }

        public SessionSkill(string sessionName, School school, float sessionCostPerDev, int duration):this()
        {
            _sessionName = sessionName;
            _school = school;
            _sessionCostPerDev = sessionCostPerDev;
            _duration = duration;
        }
    }
}
