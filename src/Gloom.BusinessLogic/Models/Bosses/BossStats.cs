using System;
using System.Collections.Generic;
using System.Linq;
using Gloom.Common;
using Gloom.CustomExceptions;
using Gloom.Data;

namespace Gloom.Models.Bosses
{
    public class BossStats
    {
        public BossStats(string bossName, int numCharacters)
        {
            var rawStats = MonsterStatsDeserialized.Instance.Bosses
                .FirstOrDefault(b => b.Name == bossName);

            InitializeStats(bossName, rawStats, numCharacters);
        }

        public BaseBossStats GetStatsByLevel(int level)
        {
            return _statsByLevel[level];
        }

        private void InitializeStats(string bossName, Data.Boss rawStats, int numCharacters)
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

                var healthMultiplier = GetHealthMultiplier(rawStatsForLevel.Health, bossName);
                Console.WriteLine("Health from card: " + rawStatsForLevel.Health);
                Console.WriteLine("Health multiplier: " + healthMultiplier);
                _statsByLevel[i] = new BaseBossStats
                {
                    BaseAttackFormula = GetAttackValueOrFormula(rawStatsForLevel, numCharacters),
                    BaseMove = rawStatsForLevel.Move,
                    BaseRange = rawStatsForLevel.Range,
                    HealthMultiplier = healthMultiplier,
                    Health = healthMultiplier * numCharacters,
                    Immunities = ConvertImmunitiesToStatusTypes(rawStatsForLevel.Immunities, bossName),
                    Special1Actions = rawStatsForLevel.Special1,
                    Special2Actions = rawStatsForLevel.Special2
                };
            }
        }

        private static string GetAttackValueOrFormula(Levels rawStatsForLevel, int numberOfCharacters)
        {
            var attackFormula = rawStatsForLevel.Attack.ToString();
            if (attackFormula == 'V'.ToString() || attackFormula.Contains('X')) 
            {
                return attackFormula;
            }
            if (int.TryParse(attackFormula, out var basic))
            {
                return basic.ToString();
            }
            if (int.TryParse(attackFormula.Substring(0,attackFormula.IndexOf('+')), out var adder))
            {
                return (adder + numberOfCharacters).ToString();
            }
            if (!int.TryParse(attackFormula.Substring(0, attackFormula.IndexOf('x')), out var multiplier))
            {
                throw new Exception($"Failed to determine boss attack");
            }
            return (multiplier * numberOfCharacters).ToString();
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