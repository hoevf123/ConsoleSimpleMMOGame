using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class TerrainObject : BaseObject
    {
        public TerrainObject(string name):base(name)
        {
            Console.WriteLine($"TerrainObject {name}이/가 생성되었습니다.");
        }
    }
}
