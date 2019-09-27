using System;
using System.Collections.Generic;
using System.Text;

namespace NETGame.CSharp.Server.Services
{
    public class Name
    {
        private string _title;
        private string _first;
        private string _last;

        public string Title { get => _title; set => _title = value; }
        public string First { get => _first; set => _first = value; }
        public string Last { get => _last; set => _last = value; }

        public Name()
        {

        }
        public Name(string title, string first, string last)
        {
            _title = title;
            _first = first;
            _last = last;
        }
    }
}
