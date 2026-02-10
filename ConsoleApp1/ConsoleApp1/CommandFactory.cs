using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    public class CommandSet
    {
        public string CommandType { get; private set; }
        public string Prefix { get; private set; }
        public List<string> Args { get; private set; }
        public string Payload { get; private set; }

        public CommandSet(string commandType, string prefix, List<string> args, string payload)
        {
            CommandType = commandType;
            Prefix = prefix;
            Args = args ?? new List<string>();
            Payload = payload ?? string.Empty;
        }
    }

    internal class CommandFactory
    {
        private readonly Dictionary<string, CommandDefinition> commandDefinitions;
        private readonly Dictionary<string, GameActionDefinition> gameActionDefinitions;

        private readonly ICommandParser parser;
        private readonly IPacketSerializer serializer;

        public CommandFactory()
            : this(new JsonCommandDefinitionProvider(), new CommandParser(), new PipePacketSerializer())
        {
        }

        internal CommandFactory(
            ICommandDefinitionProvider definitionProvider,
            ICommandParser parser,
            IPacketSerializer serializer)
        {
            commandDefinitions = definitionProvider.LoadProtocolDefinitions();
            gameActionDefinitions = definitionProvider.LoadGameActionDefinitions();
            this.parser = parser;
            this.serializer = serializer;
        }

        public CommandSet Execute(string toCommand)
        {
            CommandSet commandSet = parser.ParseProtocol(toCommand, commandDefinitions);
            if (commandSet == null)
            {
                Console.WriteLine("유효하지 않은 통신 명령어 형식입니다: " + toCommand);
                return null;
            }

            Console.WriteLine("[CommandFactory] protocol decode ok => type=" + commandSet.CommandType + ", packet=" + serializer.Serialize(commandSet));
            return commandSet;
        }

        public CommandSet ExecuteGameAction(string toActionCommand)
        {
            CommandSet actionSet = parser.ParseGameAction(toActionCommand, gameActionDefinitions);
            if (actionSet == null)
            {
                Console.WriteLine("유효하지 않은 게임 액션 명령어 형식입니다: " + toActionCommand);
                return null;
            }

            Console.WriteLine("[CommandFactory] game-action decode ok => type=" + actionSet.CommandType + ", packet=" + serializer.Serialize(actionSet));
            return actionSet;
        }

        public string GetGameActionGuideText()
        {
            List<string> lines = new List<string>
            {
                "[GameActionDefinition] 사용 가능한 게임 액션 목록"
            };

            foreach (KeyValuePair<string, GameActionDefinition> pair in gameActionDefinitions)
            {
                List<string> parameters = pair.Value.@params ?? new List<string>();
                string usage = pair.Key + (parameters.Count > 0 ? " " + string.Join(" ", parameters.Select((param) => "<" + param + ">")) : string.Empty);
                if (pair.Value.payloadRequired) usage += " <payload>";
                else if (pair.Value.payloadOptional) usage += " [payload]";

                lines.Add("- " + usage + " => " + pair.Value.description + " (prefix=" + pair.Value.prefix + ")");
            }

            return string.Join(Environment.NewLine, lines);
        }
    }
}
