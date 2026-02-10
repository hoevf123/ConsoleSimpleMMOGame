using ConsoleApp1.Spaces;
using System.Collections.Generic;

namespace ConsoleApp1
{
    internal class VirtualSpaceService
    {
        private readonly Dictionary<string, VirtualSpace> spaces = new Dictionary<string, VirtualSpace>();

        public IReadOnlyDictionary<string, VirtualSpace> Spaces
        {
            get { return spaces; }
        }

        public bool Create(string spaceName)
        {
            if (string.IsNullOrWhiteSpace(spaceName)) return false;
            if (spaces.ContainsKey(spaceName)) return false;

            spaces[spaceName] = new VirtualSpace(spaceName);
            return true;
        }

        public bool Delete(string spaceName)
        {
            if (!spaces.ContainsKey(spaceName)) return false;

            spaces[spaceName].ClearLinks();
            spaces.Remove(spaceName);
            return true;
        }

        public bool TryGet(string spaceName, out VirtualSpace virtualSpace)
        {
            return spaces.TryGetValue(spaceName, out virtualSpace);
        }

        public void ClearAllLinks()
        {
            foreach (VirtualSpace virtualSpace in spaces.Values)
            {
                virtualSpace.ClearLinks();
            }
        }
    }
}
