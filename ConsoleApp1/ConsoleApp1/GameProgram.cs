using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class GameProgram
    {
        List<BaseObject> gameObjects = new List<BaseObject>();
        public GameProgram()
        {
            
        }

        //임시. 이 기능은 단일 책임 원칙(Single Responsiblility Principle) 준수를 위해 역할 분리를 위해 나중에 Dispatcher로 따로 분리할 예정인 기능.
        public void AssertCommand(string command)
        {
            Console.WriteLine(command);

            //정확히 입력해야만 발동하는 명령어 처리.
            Dictionary<string, Action> commandExecutesAccurate = new Dictionary<string, Action>();
            commandExecutesAccurate.Add("스피키", new Action(() =>
            {
                gameObjects.Add(new TerrainObject("스피키 지형 블록"));
            }));
            commandExecutesAccurate.Add("스핔이", new Action(() =>
            {
                gameObjects.Add(new TerrainObject("호박이 좋아요 블록"));
            }));
            commandExecutesAccurate.Add("버터", new Action(() =>
            {
                gameObjects.Add(new TerrainObject("좋아요 좋아요 버터가 좋아요~ 블록"));
            }));
            commandExecutesAccurate.Add("개체 개수 표시", new Action(() =>
            {
                Console.WriteLine($"현재 개체 개수: {gameObjects.Count}개");

            }));

            commandExecutesAccurate.Add("한정현", new Action(() =>
            {
                Random random = new Random();
                int choose = random.Next(4);
                switch (choose)
                {
                    case 0:
                        gameObjects.Add(new TerrainObject("IBK기업은행 대출 10억"));
                        break;
                    case 1:
                        gameObjects.Add(new TerrainObject("체중 118kg"));
                        break;
                    case 2:
                        gameObjects.Add(new TerrainObject("대뾰니"));
                        break;
                    case 3:
                        gameObjects.Add(new TerrainObject("에르핀"));
                        break;
                    default:
                        break;

                }
            }));

            commandExecutesAccurate.Add("대뾴니", commandExecutesAccurate["한정현"]);
            commandExecutesAccurate.Add("대뾰니", commandExecutesAccurate["한정현"]);


            //명령어로 시작하여 뒤쪽인수를 받는 명령어 처리.
            Dictionary<string, Action<string>> commandExecutesStartsWith = new Dictionary<string, Action<string>>();
            commandExecutesStartsWith.Add("찾기", new Action<string>((paramString) =>
            {
                findGameObjects(paramString);
            }));

            commandExecutesAccurate.Add("모두찾기", new Action(() =>
            {
                findAllGameObjects();
            }));

            commandExecutesAccurate.Add("주말농장", new Action(() =>
            {
                gameObjects.Clear();
                Console.WriteLine("남아있던 네르들을 전부 주말농장 보내버렸습니다.");
            }));

            commandExecutesStartsWith.Add("/hostalarm", new Action<string>((paramString) =>
            {
                Console.WriteLine($"★☆★ {paramString} ★☆★");
            }));



            //명렁어 처리
            //1. 정확한 입력을 처리하는 명령어 처리.
            if (commandExecutesAccurate.ContainsKey(command))
            {
                commandExecutesAccurate[command].Invoke();
            }
            //2. 명령어로 시작하는 명령어 처리.
            else if(commandExecutesStartsWith.ContainsKey(command.Trim().Split(' ')[0]))
            {
                string keyString = command.Trim().Split(' ')[0];
                string paramString = command.Substring(keyString.Length).TrimStart();
                commandExecutesStartsWith[keyString].Invoke(paramString);
            }
            //명령어를 처리하지 않는 건 따로 처리를 하지 않는다.
            //(물론, 1, 2를 거치기 전에 콘솔 명령어로 무엇을 입력했는지 상단에 Console.WriteLine(command);을 적어두긴 했지만 말이다.
            
        }

        public int findGameObjects(string name)
        {
            int ret_val = 0;
            ret_val = gameObjects.Aggregate(0, (a, c) => a = a + (c.name.Equals(name) ? 1 : 0));
            Console.WriteLine($"대상 \"{name}\"은/는 {ret_val}개 있습니다.");
            return ret_val;
        }
        public void findAllGameObjects()
        {
            Dictionary<string, List<BaseObject>> allObjectsByName = gameObjects.Aggregate(new Dictionary<string, List<BaseObject>>(), (a, c) =>
            {
                if (c == null) return a;
                string name = c.name;
                if(!a.ContainsKey(name)) a[name] = new List<BaseObject>();
                a[name].Add(c);
                return a;
            });
            Console.WriteLine($"개체 목록: 총 {gameObjects.Count}개");
            foreach(string argname in allObjectsByName.Keys)
            {
                Console.WriteLine($"[{argname}]: {allObjectsByName[argname].Count}개");
            }
        }
    }
}
