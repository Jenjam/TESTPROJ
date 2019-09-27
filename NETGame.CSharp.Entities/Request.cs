using System;
using System.Collections.Generic;
using System.Text;

namespace NETGame.CSharp.Entities
{
    public class Request
    {
        string _code;
        string _type;
        string _payload;

        public string Code { get => _code; set => _code = value; }
        public string Type { get => _type; set => _type = value; }
        public string Payload { get => _payload; set => _payload = value; }

        public Request()
        {

        }

        public Request(string cd, string type, string payload)
        {
            Code = cd;
            Type = type;
            Payload = payload;
        }
    }
}