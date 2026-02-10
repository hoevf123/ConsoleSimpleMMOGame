using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleApp1
{
    internal class Program
    {
        private static readonly Queue<string> commandQueue = new Queue<string>();

        static void Main(string[] args)
        {
            GameProgram gameProgram = new GameProgram();
            CommandQueueProcessor queueProcessor = new CommandQueueProcessor(commandQueue, gameProgram);
            NetworkLoop networkLoop = new NetworkLoop(queueProcessor, 10000);
            ConsoleInputLoop inputLoop = new ConsoleInputLoop(commandQueue);

            Thread networkThread = new Thread(networkLoop.Run);
            networkThread.Start();

            Console.WriteLine("Hello, World!");
            inputLoop.Run();
        }
    }
}
