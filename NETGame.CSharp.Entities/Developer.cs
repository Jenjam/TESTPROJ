using System;
using System.Collections.Generic;
using System.Text;

namespace NETGame.CSharp.Entities
{
    public class Developer
    {
        private string _name;
        private float _costPerMonth;
        private string _spec;
        private string _school;
        private int _status;
        private List<Certification> _cert;
        private List<Skill> _skills;

        public string Name { get => _name; set => _name = value; }
        public float CostPerMonth { get => _costPerMonth; set => _costPerMonth = value; }
        public string Spec { get => _spec; set => _spec = value; }
        public string School { get => _school; set => _school = value; }
        public int Status { get => _status; set => _status = value; }
        public List<Certification> Cert { get => _cert; set => _cert = value; }
        public List<Skill> Skills { get => _skills; set => _skills = value; }


        public Developer()
        {
            _cert = new List<Certification>();
            _skills = new List<Skill>();
        }

        public Developer(string name, float costPerMonth, string speciality, string school, int status):this()
        {
            _name = name;
            _costPerMonth = costPerMonth;
            _spec = speciality;
            _school = school;
            _status = status;
        }
    }
}
