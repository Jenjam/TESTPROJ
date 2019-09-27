using System;
using System.Collections.Generic;
using System.Text;

namespace NETGame.CSharp.Server.Services
{
    public class ComponentService
    {
        private List<Result> results;


        public List<Result> Results { get => results; set => results = value; }


        public ComponentService()
        {

        }
    }
}
