using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class ConnectedUser
    {
        public string name;
        private ConnectionAuthenticationInfo connectionAuthenticationInfo;
        List<ICommand> availableCommands = new List<ICommand>();

        public ConnectedUser(string name, ConnectionAuthenticationInfo connectionAuthenticationInfo)
        {
            this.connectionAuthenticationInfo = connectionAuthenticationInfo;
        }
    }
}
