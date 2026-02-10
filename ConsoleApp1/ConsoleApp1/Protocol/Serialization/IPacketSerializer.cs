namespace ConsoleApp1
{
    internal interface IPacketSerializer
    {
        string Serialize(CommandSet commandSet);
    }
}
