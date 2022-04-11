using System;

namespace Gloom.CustomExceptions
{
    public class AllMonsterNumbersUsedException : Exception
    {
        public AllMonsterNumbersUsedException()
        {
        }

        public AllMonsterNumbersUsedException(string message)
            : base(message)
        {
        }

        public AllMonsterNumbersUsedException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public AllMonsterNumbersUsedException(string message, string monsterName)
            : this(message)
        {
            MonsterName = monsterName;
        }
        
        public string MonsterName { get; }
    }
}