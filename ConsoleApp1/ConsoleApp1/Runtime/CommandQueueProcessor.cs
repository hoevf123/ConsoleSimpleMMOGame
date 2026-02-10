using System.Collections.Generic;

namespace ConsoleApp1
{
    internal class CommandQueueProcessor
    {
        private readonly Queue<string> commandQueue;
        private readonly GameProgram gameProgram;

        public CommandQueueProcessor(Queue<string> commandQueue, GameProgram gameProgram)
        {
            this.commandQueue = commandQueue;
            this.gameProgram = gameProgram;
        }

        public void ProcessPending()
        {
            while (true)
            {
                string command;
                lock (commandQueue)
                {
                    if (commandQueue.Count == 0) break;
                    command = commandQueue.Dequeue();
                }

                gameProgram.AssertCommand(command);
            }
        }
    }
}
