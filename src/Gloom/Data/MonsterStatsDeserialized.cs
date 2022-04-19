using System.Collections.Generic;
using System.IO;
using Gloom.Model;
using Newtonsoft.Json;

namespace Gloom.Data
{
    public class MonsterStatsDeserialized
    {
        public List<Monster> Monsters { get; set; }
        public List<Boss> Bosses { get; set; }
        
        public static MonsterStatsDeserialized Instance
        {
            get
            {
                if (_instance == null)
                {
                    StreamReader r = new StreamReader("Data/MonsterStats.json");
                    string jsonString = r.ReadToEnd();
                    _instance = JsonConvert.DeserializeObject<MonsterStatsDeserialized>(jsonString);
                    r.Dispose();
                }

                return _instance;
            }
        }

        private static MonsterStatsDeserialized _instance;
    }
    
    public class Normal
    {
        public int Health { get; set; }
        public int Move { get; set; }
        public int Attack { get; set; }
        public int Range { get; set; }
        public List<string> Attributes { get; set; }
    }

    public class Elite
    {
        public int Health { get; set; }
        public int Move { get; set; }
        public int Attack { get; set; }
        public int Range { get; set; }
        public List<string> Attributes { get; set; }
    }

    public class Levels
    {
        public int Level { get; set; }
        public Normal Normal { get; set; }
        public Elite Elite { get; set; }
        public string Health { get; set; }
        public int Move { get; set; }
        public object Attack { get; set; }
        public int Range { get; set; }
        public List<string> Special1 { get; set; }
        public List<string> Special2 { get; set; }
        public List<string> Immunities { get; set; }
        public string Notes { get; set; }
    }

    public class Monster
    {
        public string Name { get; set; }
        public List<Levels> Levels { get; set; }
    }

    public class Boss
    {
        public string Name { get; set; }
        public List<Levels> Levels { get; set; }
    }
}