using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    internal class CommandDispatcher
    {
        private readonly Dictionary<string, Action> exactCommands = new Dictionary<string, Action>();
        private readonly Dictionary<string, Action<string>> prefixCommands = new Dictionary<string, Action<string>>();

        public void RegisterExact(string command, Action action)
        {
            exactCommands[command] = action;
        }

        public void RegisterPrefix(string commandPrefix, Action<string> action)
        {
            prefixCommands[commandPrefix] = action;
        }

        public void RegisterAlias(string originalCommand, params string[] aliases)
        {
            if (!exactCommands.ContainsKey(originalCommand)) return;

            Action originalAction = exactCommands[originalCommand];
            foreach (string alias in aliases)
            {
                exactCommands[alias] = originalAction;
            }
        }

        public bool Dispatch(string rawCommand)
        {
            if (string.IsNullOrWhiteSpace(rawCommand)) return false;

            if (exactCommands.ContainsKey(rawCommand))
            {
                exactCommands[rawCommand].Invoke();
                return true;
            }

            string[] tokenized = rawCommand.Trim().Split(new[] { ' ' }, 2, StringSplitOptions.None);
            string prefix = tokenized[0];
            if (!prefixCommands.ContainsKey(prefix)) return false;

            string payload = tokenized.Length > 1 ? tokenized[1] : string.Empty;
            prefixCommands[prefix].Invoke(payload);
            return true;
        }
    }
}
