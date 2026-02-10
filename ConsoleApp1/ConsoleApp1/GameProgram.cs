using ConsoleApp1.Spaces;
using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    internal class GameProgram
    {
        private readonly GameObjectRepository gameObjectRepository;
        private readonly VirtualSpaceService virtualSpaceService;
        private readonly CommandDispatcher commandDispatcher;
        private readonly Random random = new Random();

        public GameProgram()
        {
            gameObjectRepository = new GameObjectRepository();
            virtualSpaceService = new VirtualSpaceService();
            commandDispatcher = new CommandDispatcher();

            RegisterCommands();
        }

        private void RegisterCommands()
        {
            commandDispatcher.RegisterExact("스피키", () => gameObjectRepository.Add(new TerrainObject("스피키 지형 블록")));
            commandDispatcher.RegisterExact("스핔이", () => gameObjectRepository.Add(new TerrainObject("호박이 좋아요 블록")));
            commandDispatcher.RegisterExact("버터", () => gameObjectRepository.Add(new TerrainObject("좋아요 좋아요 버터가 좋아요~ 블록")));
            commandDispatcher.RegisterExact("개체 개수 표시", () => Console.WriteLine($"현재 개체 개수: {gameObjectRepository.Count}개"));

            commandDispatcher.RegisterExact("한정현", CreateRandomTerrainObject);
            commandDispatcher.RegisterAlias("한정현", "대뾴니", "대뾰니");

            commandDispatcher.RegisterPrefix("찾기", FindGameObjectsCommand);
            commandDispatcher.RegisterExact("모두찾기", FindAllGameObjectsCommand);
            commandDispatcher.RegisterExact("주말농장", ClearAllCommand);
            commandDispatcher.RegisterPrefix("/hostalarm", (paramString) => Console.WriteLine($"★☆★ {paramString} ★☆★"));

            commandDispatcher.RegisterPrefix("공간생성", CreateVirtualSpaceCommand);
            commandDispatcher.RegisterPrefix("공간삭제", DeleteVirtualSpaceCommand);
            commandDispatcher.RegisterPrefix("공간에넣기", LinkObjectToVirtualSpaceCommand);
            commandDispatcher.RegisterExact("공간목록", PrintVirtualSpaceList);
            commandDispatcher.RegisterPrefix("공간조회", PrintVirtualSpaceDetailCommand);

            // 이전 월드 명령어 호환
            commandDispatcher.RegisterPrefix("세계생성", CreateVirtualSpaceCommand);
            commandDispatcher.RegisterPrefix("세계삭제", DeleteVirtualSpaceCommand);
            commandDispatcher.RegisterPrefix("세계에넣기", LinkObjectToVirtualSpaceCommand);
            commandDispatcher.RegisterExact("세계목록", PrintVirtualSpaceList);
            commandDispatcher.RegisterPrefix("세계조회", PrintVirtualSpaceDetailCommand);
        }

        private void CreateRandomTerrainObject()
        {
            string[] randomTerrainNames =
            {
                "IBK기업은행 대출 10억",
                "체중 118kg",
                "대뾰니",
                "에르핀"
            };

            int choose = random.Next(randomTerrainNames.Length);
            gameObjectRepository.Add(new TerrainObject(randomTerrainNames[choose]));
        }

        private void ClearAllCommand()
        {
            gameObjectRepository.Clear();
            virtualSpaceService.ClearAllLinks();
            Console.WriteLine("남아있던 네르들을 전부 주말농장 보내버렸습니다.");
        }

        private void CreateVirtualSpaceCommand(string paramString)
        {
            string spaceName = paramString.Trim();
            if (string.IsNullOrWhiteSpace(spaceName))
            {
                Console.WriteLine("사용법: 공간생성 [공간이름]");
                return;
            }

            if (!virtualSpaceService.Create(spaceName))
            {
                Console.WriteLine($"이미 존재하는 공간이거나 이름이 잘못되었습니다: {spaceName}");
                return;
            }

            Console.WriteLine($"공간 생성 완료: {spaceName}");
        }

        private void DeleteVirtualSpaceCommand(string paramString)
        {
            string spaceName = paramString.Trim();
            if (string.IsNullOrWhiteSpace(spaceName))
            {
                Console.WriteLine("사용법: 공간삭제 [공간이름]");
                return;
            }

            if (!virtualSpaceService.Delete(spaceName))
            {
                Console.WriteLine($"삭제할 공간이 없습니다: {spaceName}");
                return;
            }

            Console.WriteLine($"공간 삭제 완료: {spaceName}");
        }

        private void LinkObjectToVirtualSpaceCommand(string paramString)
        {
            string[] tokens = paramString.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length < 2)
            {
                Console.WriteLine("사용법: 공간에넣기 [공간명] [오브젝트명]");
                return;
            }

            string spaceName = tokens[0];
            string objectName = tokens[1].Trim();

            VirtualSpace virtualSpace;
            if (!virtualSpaceService.TryGet(spaceName, out virtualSpace))
            {
                Console.WriteLine($"대상 공간이 없습니다: {spaceName}");
                return;
            }

            List<BaseObject> matchedObjects = gameObjectRepository.FindAllByName(objectName);
            if (matchedObjects.Count == 0)
            {
                Console.WriteLine($"전역 개체 목록에 \"{objectName}\" 이름의 오브젝트가 없습니다.");
                return;
            }

            int linkedCount = virtualSpace.LinkObjects(matchedObjects);
            Console.WriteLine($"공간 [{spaceName}]에 [{objectName}] 오브젝트 {linkedCount}개를 연결했습니다. (중복 제외)");
        }

        private void PrintVirtualSpaceList()
        {
            Console.WriteLine($"공간 목록: 총 {virtualSpaceService.Spaces.Count}개");
            foreach (KeyValuePair<string, VirtualSpace> pair in virtualSpaceService.Spaces)
            {
                Console.WriteLine($"- {pair.Key} (연결된 오브젝트 {pair.Value.LinkedObjects.Count}개)");
            }
        }

        private void PrintVirtualSpaceDetailCommand(string paramString)
        {
            string spaceName = paramString.Trim();
            if (string.IsNullOrWhiteSpace(spaceName))
            {
                Console.WriteLine("사용법: 공간조회 [공간이름]");
                return;
            }

            VirtualSpace targetSpace;
            if (!virtualSpaceService.TryGet(spaceName, out targetSpace))
            {
                Console.WriteLine($"조회할 공간이 없습니다: {spaceName}");
                return;
            }

            Console.WriteLine($"공간 [{spaceName}] 오브젝트: 총 {targetSpace.LinkedObjects.Count}개");
            Dictionary<string, int> byName = targetSpace.CountByObjectName();
            foreach (KeyValuePair<string, int> pair in byName)
            {
                Console.WriteLine($"[{pair.Key}] {pair.Value}개");
            }
        }

        public void AssertCommand(string command)
        {
            Console.WriteLine(command);
            commandDispatcher.Dispatch(command);
        }

        public int findGameObjects(string name)
        {
            int result = gameObjectRepository.FindCountByName(name);
            Console.WriteLine($"대상 \"{name}\"은/는 {result}개 있습니다.");
            return result;
        }

        public void findAllGameObjects()
        {
            FindAllGameObjectsCommand();
        }

        private void FindGameObjectsCommand(string name)
        {
            findGameObjects(name);
        }

        private void FindAllGameObjectsCommand()
        {
            Dictionary<string, int> allObjectsByName = gameObjectRepository.GroupCountByName();
            Console.WriteLine($"개체 목록: 총 {gameObjectRepository.Count}개");
            foreach (KeyValuePair<string, int> pair in allObjectsByName)
            {
                Console.WriteLine($"[{pair.Key}]: {pair.Value}개");
            }
        }
    }
}
