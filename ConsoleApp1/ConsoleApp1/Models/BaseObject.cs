using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal abstract class BaseObject
    {
        public string name;
        public Dictionary<string, string> availableActions = new Dictionary<string, string>();
        public List<Action> components = new List<Action>(); 

        public BaseObject(string name = "") {
            this.name = name;
        }

        
    }
}
