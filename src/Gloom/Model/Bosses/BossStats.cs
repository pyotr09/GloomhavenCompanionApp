using System;
using System.Collections.Generic;
using System.Linq;
using Gloom.CustomExceptions;
using Gloom.Data;

namespace Gloom.Model.Bosses
{
    public class BossStats
    {
        public BossStats(string bossName)
        {
            var rawStats = MonsterStatsDeserialized.Instance.Bosses
                .FirstOrDefault(b => b.Name == bossName);

            InitializeStats(bossName, rawStats);
        }

        public BaseBossStats GetStatsByLevel(int level)
        {
            return _statsByLevel[level];
        }

        private void InitializeStats(string bossName, Data.Boss rawStats)
        {
            _statsByLevel = new BaseBossStats[8];
            for (int i = 0; i <= 7; i++)
            {
                var rawStatsForLevel = rawStats.Levels.FirstOrDefault(l => l.Level == i);
                if (rawStatsForLevel == null)
                {
                    throw new MonsterStatsNotFoundException("Boss Stats not found for given level",
                        bossName, i);
                }
                _statsByLevel[i] = new BaseBossStats
                {
                    AttackFormula = rawStatsForLevel.Attack.ToString(),
                    BaseMove = rawStatsForLevel.Move,
                    BaseRange = rawStatsForLevel.Range,
                    HealthMultiplier = GetHealthMultiplier(rawStatsForLevel.Health, bossName),
                    Immunities = ConvertImmunitiesToStatusTypes(rawStatsForLevel.Immunities, bossName),
                    Special1Actions = rawStatsForLevel.Special1,
                    Special2Actions = rawStatsForLevel.Special2
                };
            }
        }

        private List<StatusType> ConvertImmunitiesToStatusTypes(List<string> immunities, string bossName)
        {
            var statusTypes = new List<StatusType>();
            foreach (var statusString in immunities)
            {
                statusTypes.Add(Status.ParseStatusString(statusString));
            }
            return statusTypes;
        }

        private int GetHealthMultiplier(string healthText, string bossName)
        {
            var constant = healthText.Substring(0, healthText.IndexOf('x'));
            if (!int.TryParse(constant, out var result))
            {
                throw new Exception($"Boss health parse failed for {bossName}: {healthText}");
            }
            return result;
        }

        private BaseBossStats[] _statsByLevel;
    }
}