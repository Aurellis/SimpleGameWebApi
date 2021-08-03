using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTaskGame
{
    public class Character
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public bool Unlocked { get; set; }
        public List<Gun> AvailableGuns { get; set; }
    }
}
