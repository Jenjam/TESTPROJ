using System;
using System.Collections.Generic;
using System.Text;

namespace NETGame.CSharp.Entities
{
    public class School
    {
        private string _schoolName;
        private List<SessionSkill> _schoolSessions;
        private float _winRate;


        public string SchoolName { get => _schoolName; set => _schoolName = value; }       
        public float WinRate { get => _winRate; set => _winRate = value; }

        public School()
        {
            _schoolSessions = new List<SessionSkill>();
        }
        /// <summary>
        /// Constructor of the class school
        /// </summary>
        /// <param name="schoolID"></param>
        /// <param name="schoolName"></param>
        /// <param name="winRate"></param>
        public School(string schoolName, float winRate):this()
        {
            _schoolName = schoolName;           
            _winRate = winRate;
        }
        /// <summary>
        /// Add a session to the list of the sessions of this school
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public int AddSession(SessionSkill session )
        {
            _schoolSessions.Add(session);
            return _schoolSessions.Count;
        }
        /// <summary>
        /// Une an already existing list of sessions and attribuate it to the school
        /// </summary>
        /// <param name="sessionSkills"></param>
        /// <returns></returns>
        public int AddSession( List<SessionSkill> sessionSkills)
        {
            _schoolSessions = sessionSkills;
            return _schoolSessions.Count;
        }
        /// <summary>
        /// Crée une nouvelle session, en utilisant les paramètres saisits
        /// </summary>
        /// <param name="sessionCost"></param>
        /// <param name="duration"></param>
        /// <param name="sessionName"></param>
        /// <returns></returns>
        public int AddNewSession( float sessionCost , int duration , string sessionName)
        {
            _schoolSessions.Add(new SessionSkill(sessionName, this, sessionCost, duration));
            return _schoolSessions.Count;
        }
    }
}
