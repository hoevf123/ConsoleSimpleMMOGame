using System.Collections.Generic;

namespace ConsoleApp1
{
    internal interface ICommandParser
    {
        CommandSet ParseProtocol(string commandString, Dictionary<string, CommandDefinition> definitions);
        CommandSet ParseGameAction(string commandString, Dictionary<string, GameActionDefinition> definitions);
    }
}
