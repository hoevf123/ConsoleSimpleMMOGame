using System.Collections.Generic;

namespace ConsoleApp1
{
    internal class CommandDefinition
    {
        public string prefix { get; set; }
        public List<string> @params { get; set; }
        public bool payloadRequired { get; set; }
        public bool payloadOptional { get; set; }
    }

    internal class GameActionDefinition
    {
        public string category { get; set; }
        public string prefix { get; set; }
        public List<string> @params { get; set; }
        public bool payloadRequired { get; set; }
        public bool payloadOptional { get; set; }
        public string description { get; set; }
    }
}
