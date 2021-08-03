using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTaskGame
{
    public class Gun
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Damage { get; set; }
        public int RateOfFire { get; set; }
        public int RechargeRate { get; set; }
        public int NumBullets { get; set; }
        public int MaxLevel { get; set; }
        public bool Unlocked { get; set; }
    }
}
