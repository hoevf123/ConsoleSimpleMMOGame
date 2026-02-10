using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.Spaces
{
    // 렌더링이 없는 논리 공간 단위
    internal class VirtualSpace
    {
        private readonly List<BaseObject> linkedObjects = new List<BaseObject>();

        public string Name { get; private set; }

        public VirtualSpace(string name)
        {
            Name = name;
        }

        public IReadOnlyList<BaseObject> LinkedObjects
        {
            get { return linkedObjects; }
        }

        public bool LinkObject(BaseObject target)
        {
            if (target == null) return false;
            if (linkedObjects.Contains(target)) return false;

            linkedObjects.Add(target);
            return true;
        }

        public int LinkObjects(IEnumerable<BaseObject> targets)
        {
            if (targets == null) return 0;

            int linkedCount = 0;
            foreach (BaseObject target in targets)
            {
                if (LinkObject(target)) linkedCount++;
            }
            return linkedCount;
        }

        public void ClearLinks()
        {
            linkedObjects.Clear();
        }

        public Dictionary<string, int> CountByObjectName()
        {
            return linkedObjects
                .Where((obj) => obj != null)
                .GroupBy((obj) => obj.name)
                .ToDictionary((group) => group.Key, (group) => group.Count());
        }
    }
}
