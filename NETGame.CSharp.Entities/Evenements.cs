using System;
using System.Collections.Generic;
using System.Text;

namespace NETGame.CSharp.Entities
{
    public class Evenements
    {
        private string _evenementName;
        private int _cost;

        public string EvenementName { get => _evenementName; set => _evenementName = value; }
        public int Cost { get => _cost; set => _cost = value; }


        public Evenements()
        {
                
        }

        public Evenements(string eventName, int cost)
        {
            _cost = cost;
            _evenementName = eventName;
        }
    }
}
