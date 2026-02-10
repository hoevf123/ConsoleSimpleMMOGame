using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    internal class GameObjectRepository
    {
        private readonly List<BaseObject> gameObjects = new List<BaseObject>();

        public int Count
        {
            get { return gameObjects.Count; }
        }

        public IReadOnlyList<BaseObject> All
        {
            get { return gameObjects; }
        }

        public void Add(BaseObject target)
        {
            if (target == null) return;
            gameObjects.Add(target);
        }

        public void Clear()
        {
            gameObjects.Clear();
        }

        public int FindCountByName(string name)
        {
            return gameObjects.Count((obj) => obj != null && string.Equals(obj.name, name, StringComparison.Ordinal));
        }

        public List<BaseObject> FindAllByName(string name)
        {
            return gameObjects
                .Where((obj) => obj != null && string.Equals(obj.name, name, StringComparison.Ordinal))
                .ToList();
        }

        public Dictionary<string, int> GroupCountByName()
        {
            return gameObjects
                .Where((obj) => obj != null)
                .GroupBy((obj) => obj.name)
                .ToDictionary((group) => group.Key, (group) => group.Count());
        }
    }
}
