using System.Collections.Generic;

namespace ConsoleApp1
{
    internal interface ICommandDefinitionProvider
    {
        Dictionary<string, CommandDefinition> LoadProtocolDefinitions();
        Dictionary<string, GameActionDefinition> LoadGameActionDefinitions();
    }
}
