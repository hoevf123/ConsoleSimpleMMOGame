using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Spaces
{
    //하나의 공간. 하나의 공간에는 공간 안의 여러 개체들이 존재합니다.
    internal class VirtualSpace
    {
        public List<BaseObject> globalSpaceGameObjects = null;
        public List<BaseObject> localSpaceGameObjects = new List<BaseObject>();
        public VirtualSpace(List<BaseObject> globalSpaceGameObjects, List<BaseObject> localSpaceGameObjects)
        {
            this.SetGlobalSpaceGameObjects(globalSpaceGameObjects);
            
            
            this.localSpaceGameObjects = localSpaceGameObjects;
        }

        public void SetGlobalSpaceGameObjects(List<BaseObject> globalSpaceGameObjects)
        {
            this.globalSpaceGameObjects = globalSpaceGameObjects;
        }

        public List<BaseObject> GetGlobalSpaceGameObjects()
        {
            return this.globalSpaceGameObjects;
        }


    }
}
