using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{ 
    internal class Program
    {
        static Queue<string> commandQueue = new Queue<string>();
        static GameProgram gameProgram = new GameProgram();


        //메인 함수. 프로그램 시작점
        static void Main(string[] args)
        {
            Thread networkThread = new Thread(NetworkMain);
            Thread gameThread = new Thread(GameMain);
            networkThread.Start();
            System.Console.WriteLine("Hello, World!");
            string toType = "";
            while (true)
            {
                toType = Console.ReadLine();
                System.Console.WriteLine($"스피키 {toType} 네르지 마세요!");
                commandQueue.Enqueue( toType );
            }
            
        }

        static void GameMain()
        {

        }

        static void NetworkMain()
        {
            while (true)
            {
                Thread.Sleep(10000);
                Console.WriteLine("스피키 네르지 마세요!");
                while(commandQueue.Count > 0)
                {
                    string toDeliver = commandQueue.Dequeue();
                    gameProgram.AssertCommand(toDeliver);

                }
                
            }
           
        }
    }
}
