using System;

namespace ConsoleApp1
{
    internal class PipePacketSerializer : IPacketSerializer
    {
        public string Serialize(CommandSet commandSet)
        {
            if (commandSet == null) return string.Empty;

            string packetString = commandSet.Prefix;
            if (commandSet.Args.Count > 0)
            {
                packetString += "|" + string.Join("|", commandSet.Args);
            }
            if (!string.IsNullOrWhiteSpace(commandSet.Payload))
            {
                packetString += "|" + commandSet.Payload;
            }
            return packetString;
        }
    }
}
