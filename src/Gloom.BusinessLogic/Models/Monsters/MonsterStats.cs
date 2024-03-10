using System.Linq;
using Gloom.Common;
using Gloom.CustomExceptions;
using Gloom.Data;

namespace Gloom.Models.Monsters
{
    public class MonsterStats
    {
        public MonsterStats()
        {
        }
        public MonsterStats(string monsterName)
        {
            var rawStats = MonsterStatsDeserialized.Instance.Monsters
                .FirstOrDefault(m => m.Name == monsterName);
            if (rawStats == null)
            {
                throw new MonsterStatsNotFoundException("Monster Stats not found " + monsterName, monsterName);
            }

            InitializeStats(monsterName, rawStats);
        }

        private void InitializeStats(string monsterName, Data.Monster rawStats)
        {
            Count = rawStats.Count;
            _normalStatsByLevel = new BaseMonsterStats[8];
            _eliteStatsByLevel = new BaseMonsterStats[8];
            for (int i = 0; i <= 7; i++)
            {
                var rawStatsForLevel = rawStats.Levels.FirstOrDefault(l => l.Level == i);
                if (rawStatsForLevel == null)
                {
                    throw new MonsterStatsNotFoundException("Monster Stats not found for given level",
                        monsterName, i);
                }

                _normalStatsByLevel[i] = new BaseMonsterStats(rawStatsForLevel.Normal.Attributes, MonsterTier.Normal)
                {
                    Health = rawStatsForLevel.Normal.Health,
                    BaseAttackFormula = rawStatsForLevel.Normal.Attack.ToString(),
                    BaseMove = rawStatsForLevel.Normal.Move,
                    BaseRange = rawStatsForLevel.Normal.Range
                };

                _eliteStatsByLevel[i] = new BaseMonsterStats(rawStatsForLevel.Elite.Attributes, MonsterTier.Elite)
                {
                    Health = rawStatsForLevel.Elite.Health,
                    BaseAttackFormula = rawStatsForLevel.Elite.Attack.ToString(),
                    BaseMove = rawStatsForLevel.Elite.Move,
                    BaseRange = rawStatsForLevel.Elite.Range
                };
            }
        }
        
        public BaseMonsterStats GetStatsByLevelAndTier(int level, MonsterTier tier)
        {
            if (tier == MonsterTier.Normal)
            {
                return _normalStatsByLevel[level];
            }
            if (tier == MonsterTier.Elite)
            {
                return _eliteStatsByLevel[level];
            }

            return null;
        }

        public int Count;

        private BaseMonsterStats[] _normalStatsByLevel;
        private BaseMonsterStats[] _eliteStatsByLevel;
    }
}