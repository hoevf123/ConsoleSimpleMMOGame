using System;
using System.Threading;

namespace ConsoleApp1
{
    internal class NetworkLoop
    {
        private readonly CommandQueueProcessor queueProcessor;
        private readonly int intervalMs;

        public NetworkLoop(CommandQueueProcessor queueProcessor, int intervalMs)
        {
            this.queueProcessor = queueProcessor;
            this.intervalMs = intervalMs;
        }

        public void Run()
        {
            while (true)
            {
                Thread.Sleep(intervalMs);
                Console.WriteLine("스피키 네르지 마세요!");
                queueProcessor.ProcessPending();
            }
        }
    }
}
