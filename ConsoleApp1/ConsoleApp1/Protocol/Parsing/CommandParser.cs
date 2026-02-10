using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    internal class CommandParser : ICommandParser
    {
        public CommandSet ParseProtocol(string commandString, Dictionary<string, CommandDefinition> definitions)
        {
            return BuildCommandSet(
                commandString,
                definitions.ToDictionary((k) => k.Key, (v) => v.Value.prefix),
                definitions.ToDictionary((k) => k.Key, (v) => v.Value.@params),
                definitions.ToDictionary((k) => k.Key, (v) => v.Value.payloadRequired),
                definitions.ToDictionary((k) => k.Key, (v) => v.Value.payloadOptional));
        }

        public CommandSet ParseGameAction(string commandString, Dictionary<string, GameActionDefinition> definitions)
        {
            return BuildCommandSet(
                commandString,
                definitions.ToDictionary((k) => k.Key, (v) => v.Value.prefix),
                definitions.ToDictionary((k) => k.Key, (v) => v.Value.@params),
                definitions.ToDictionary((k) => k.Key, (v) => v.Value.payloadRequired),
                definitions.ToDictionary((k) => k.Key, (v) => v.Value.payloadOptional));
        }

        private CommandSet BuildCommandSet(
            string commandString,
            Dictionary<string, string> prefixByType,
            Dictionary<string, List<string>> paramsByType,
            Dictionary<string, bool> payloadRequiredByType,
            Dictionary<string, bool> payloadOptionalByType)
        {
            if (string.IsNullOrWhiteSpace(commandString)) return null;

            string[] tokens = commandString.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 0) return null;

            string commandType = tokens[0];
            if (!prefixByType.ContainsKey(commandType)) return null;

            List<string> requiredParams = paramsByType.ContainsKey(commandType) && paramsByType[commandType] != null
                ? paramsByType[commandType]
                : new List<string>();

            List<string> args = new List<string>();
            for (int i = 0; i < requiredParams.Count; i++)
            {
                int tokenIndex = i + 1;
                if (tokenIndex >= tokens.Length) return null;
                args.Add(tokens[tokenIndex]);
            }

            string payload = string.Empty;
            int payloadStartIndex = requiredParams.Count + 1;
            if (tokens.Length > payloadStartIndex)
            {
                payload = string.Join(" ", tokens.Skip(payloadStartIndex));
            }

            bool payloadRequired = payloadRequiredByType.ContainsKey(commandType) && payloadRequiredByType[commandType];
            bool payloadOptional = payloadOptionalByType.ContainsKey(commandType) && payloadOptionalByType[commandType];

            if (payloadRequired && string.IsNullOrWhiteSpace(payload)) return null;
            if (!payloadRequired && !payloadOptional && !string.IsNullOrWhiteSpace(payload)) return null;

            return new CommandSet(commandType, prefixByType[commandType] ?? string.Empty, args, payload);
        }
    }
}
