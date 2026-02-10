using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    internal class ConsoleInputLoop
    {
        private readonly Queue<string> commandQueue;

        public ConsoleInputLoop(Queue<string> commandQueue)
        {
            this.commandQueue = commandQueue;
        }

        public void Run()
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (input == null) continue;

                lock (commandQueue)
                {
                    commandQueue.Enqueue(input);
                }
                Console.WriteLine($"스피키 {input} 네르지 마세요!");
            }
        }
    }
}
