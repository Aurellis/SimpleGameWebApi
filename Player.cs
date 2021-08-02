using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TestTaskGame
{
    public class Player
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Coins { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
    }
}
