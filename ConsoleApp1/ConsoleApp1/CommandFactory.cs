using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class CommandSet
    {
        string commnadType;
        string content;
        public CommandSet(string commandType, string content)
        {
            this.commnadType = commandType;
            this.content = content;
        }
    }
    //TODO: 받은 명령어를 해독하여 프로그램 내 기능을 수행해 주는 팩토리 패턴 유형의 코드 해독 - 변환 클래스.
    internal class CommandFactory
    {

        //public Queue<string> commandStringQueue = new Queue<string>();

        //생성자는 딱히..?
        public CommandFactory() { }

        private CommandSet StringToCommandSet(string commandString)
        {
            return null;
        }

        public CommandSet Execute(string toCommand)
        {

            return null;
        }

        
    }
}
