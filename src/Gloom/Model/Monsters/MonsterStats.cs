using System.Collections.Generic;
using System.Linq;
using Gloom.CustomExceptions;
using Gloom.Data;

namespace Gloom.Model.Monsters
{
    public class MonsterStats
    {
        public MonsterStats(string monsterName)
        {
            var rawStats = MonsterStatsDeserialized.Instance.Monsters
                .FirstOrDefault(m => m.Name == monsterName);
            if (rawStats == null)
            {
                throw new MonsterStatsNotFoundException("Monster Stats not found", monsterName);
            }

            InitializeStats(monsterName, rawStats);
        }

        private void InitializeStats(string monsterName, Data.Monster rawStats)
        {
            NormalStatsByLevel = new BaseMonsterStats[8];
            EliteStatsByLevel = new BaseMonsterStats[8];
            for (int i = 0; i <= 7; i++)
            {
                var rawStatsForLevel = rawStats.Levels.FirstOrDefault(l => l.Level == i);
                if (rawStatsForLevel == null)
                {
                    throw new MonsterStatsNotFoundException("Monster Stats not found for given level",
                        monsterName, i);
                }

                NormalStatsByLevel[i] = new BaseMonsterStats(rawStatsForLevel.Normal.Attributes)
                {
                    HitPoints = rawStatsForLevel.Normal.Health,
                    BaseAttack = rawStatsForLevel.Normal.Attack,
                    BaseMove = rawStatsForLevel.Normal.Move,
                    BaseRange = rawStatsForLevel.Normal.Range
                };

                EliteStatsByLevel[i] = new BaseMonsterStats(rawStatsForLevel.Elite.Attributes)
                {
                    HitPoints = rawStatsForLevel.Elite.Health,
                    BaseAttack = rawStatsForLevel.Elite.Attack,
                    BaseMove = rawStatsForLevel.Elite.Move,
                    BaseRange = rawStatsForLevel.Elite.Range
                };
            }
        }
        
        public BaseMonsterStats GetStatsByLevelAndTier(int level, MonsterTier tier)
        {
            if (tier == MonsterTier.Normal)
            {
                return NormalStatsByLevel[level];
            }
            if (tier == MonsterTier.Elite)
            {
                return EliteStatsByLevel[level];
            }

            return null;
        }

        private BaseMonsterStats[] NormalStatsByLevel { get; set; }
        private BaseMonsterStats[] EliteStatsByLevel { get; set; }
    }
}