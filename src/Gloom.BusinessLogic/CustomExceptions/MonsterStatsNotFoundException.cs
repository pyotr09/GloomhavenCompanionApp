using System;

namespace Gloom.CustomExceptions
{
    [Serializable]
    public class MonsterStatsNotFoundException : Exception
    {
        public MonsterStatsNotFoundException()
        {
        }

        public MonsterStatsNotFoundException(string message) 
            : base(message)
        {
        }

        public MonsterStatsNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public MonsterStatsNotFoundException(string message, string monsterName)
        : this(message)
        {
            MonsterName = monsterName;
        }

        public MonsterStatsNotFoundException(string message, string monsterName, int level)
            : this(message, monsterName)
        {
            Level = level;
        }
        
        public string MonsterName { get; }
        public int Level { get; set; }
    }
}