using System;
using System.Collections.Generic;
using System.Text;

namespace NETGame.CSharp.Entities
{
    public class Certification
    {
        private int _certificationID;
        private string _certificationName;
        private int _certificationMaxLevel;
        private int _certificationMinLevel;

        public int CertificationID { get => _certificationID; set => _certificationID = value; }
        public string CertificationName { get => _certificationName; set => _certificationName = value; }
        public int CertificationMaxLevel { get => _certificationMaxLevel; set => _certificationMaxLevel = value; }
        public int CertificationMinLevel { get => _certificationMinLevel; set => _certificationMinLevel = value; }

        public Certification()
        {

        }

        public Certification(int certID, string certName, int certMaxLevel, int certMinLevel)
        {
            _certificationID = certID;
            _certificationMaxLevel = certMaxLevel;
            _certificationMinLevel = certMinLevel;
            _certificationName = certName;
        }
    }
}
