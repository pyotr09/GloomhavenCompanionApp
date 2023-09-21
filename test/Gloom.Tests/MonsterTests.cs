using System;
using Gloom.CustomExceptions;
using Gloom.Models;
using Gloom.Models.Monsters;
using Xunit;
using Monster = Gloom.Models.Monsters.Monster;

namespace Gloom.Tests
{
    public class MonsterTests
    {
        private static Monster SetupLevelOneBanditGuard()
        {
            var group = SetupBanditGuardGroup(1);
            Monster guard1 = new Monster(group.NormalStats, 1, MonsterTier.Normal);
            return guard1;
        }

        private static MonsterGrouping SetupBanditGuardGroup(int level)
        {
            string name = "Bandit Guard";
            MonsterType banditGuard = new MonsterType (name, "Guard")
            {
            };

            MonsterGrouping group = new MonsterGrouping(banditGuard, level);
            return group;
        }
        
        private static MonsterGrouping SetupFlameDemonGroup(int level)
        {
            string name = "Flame Demon";
            MonsterType flameDemon = new MonsterType (name)
            {
            };

            MonsterGrouping group = new MonsterGrouping(flameDemon, level);
            return group;
        }

        [Fact]
        public void EndOfRound_RetaliateTest()
        {
            var guard1 = SetupLevelOneBanditGuard();

            guard1.CurrentRetaliate += 2;
            guard1.RefreshForEndOfRound();
            
            Assert.Equal(0, guard1.CurrentRetaliate);
        }
        
        [Fact]
        public void StatusAddedOnCurrentTurn_DoesntClear()
        {
            var guard1 = SetupLevelOneBanditGuard();
            guard1.SetStatus(StatusType.Disarm, true, true);
            guard1.RefreshForEndOfTurn();
            Assert.True(guard1.Statuses.Disarm.IsActive);
        }
        
        [Fact]
        public void StatusAddedOnPreviousTurn_DoesClear()
        {
            var guard1 = SetupLevelOneBanditGuard();
            guard1.SetStatus(StatusType.Immobilize, true, true);
            guard1.RefreshForEndOfTurn();
            guard1.RefreshForEndOfRound();
            guard1.RefreshForEndOfTurn();
            Assert.False(guard1.Statuses.Immobilize.IsActive);
        }
        
        [Fact]
        public void StatusAddedNotOnTurn_DoesClear()
        {
            var guard1 = SetupLevelOneBanditGuard();
            guard1.SetStatus(StatusType.Stun, true, false);
            guard1.RefreshForEndOfTurn();
            Assert.False(guard1.Statuses.Stun.IsActive);
        }

        [Fact]
        public void AddAllMonsters()
        {
            MonsterGrouping group = SetupBanditGuardGroup(3);
            group.AddMonster(MonsterTier.Normal, 3);
            group.AddMonster(MonsterTier.Normal, 3);
            group.AddMonster(MonsterTier.Normal, 3);
            group.AddMonster(MonsterTier.Elite, 3);
            group.AddMonster(MonsterTier.Elite, 3);
            group.AddMonster(MonsterTier.Elite, 3);

            Assert.Throws<AllMonsterNumbersUsedException>(() =>
                group.AddMonster(MonsterTier.Normal, 3));
        }

        [Fact]
        public void VerifyStats_BanditGuard()
        {
            var level0 = SetupBanditGuardGroup(0);
            var stats0N = level0.NormalStats;
            var stats0E = level0.EliteStats;
            var level1 = SetupBanditGuardGroup(1);
            var stats1N = level1.NormalStats;
            var stats1E = level1.EliteStats;
            var level2 = SetupBanditGuardGroup(2);
            var stats2N = level2.NormalStats;
            var stats2E = level2.EliteStats;
            var level3 = SetupBanditGuardGroup(3);
            var stats3N = level3.NormalStats;
            var stats3E = level3.EliteStats;
            var level4 = SetupBanditGuardGroup(4);
            var stats4N = level4.NormalStats;
            var stats4E = level4.EliteStats;
            var level5 = SetupBanditGuardGroup(5);
            var stats5N = level5.NormalStats;
            var stats5E = level5.EliteStats;
            var level6 = SetupBanditGuardGroup(6);
            var stats6N = level6.NormalStats;
            var stats6E = level6.EliteStats;
            var level7 = SetupBanditGuardGroup(7);
            var stats7N = level7.NormalStats;
            var stats7E = level7.EliteStats;
            
            
            // Level 0 Normal
            Assert.Equal(5, stats0N.Health);
            Assert.Equal(2, stats0N.BaseMove);
            Assert.Equal(2, int.Parse(stats0N.BaseAttackFormula));
            Assert.Equal(0, stats0N.BaseRange);
            Assert.Equal(0, stats0N.BaseShield);
            Assert.Equal(0, stats0N.BaseRetaliate);
            Assert.Equal(0, stats0N.BaseRetaliateRange);
            Assert.Equal(0, stats0N.BasePierce);
            Assert.Equal(1, stats0N.BaseTarget);
            
            // Level 0 Elite
            Assert.Equal(9, stats0E.Health);
            Assert.Equal(2, stats0E.BaseMove);
            Assert.Equal(3, int.Parse(stats0E.BaseAttackFormula));
            Assert.Equal(0, stats0E.BaseRange);
            Assert.Equal(0, stats0E.BaseShield);
            Assert.Equal(0, stats0E.BaseRetaliate);
            Assert.Equal(0, stats0E.BaseRetaliateRange);
            Assert.Equal(0, stats0E.BasePierce);
            Assert.Equal(1, stats0E.BaseTarget);
            
            // Level 1 Normal
            Assert.Equal(6, stats1N.Health);
            Assert.Equal(3, stats1N.BaseMove);
            Assert.Equal(2, int.Parse(stats1N.BaseAttackFormula));
            Assert.Equal(0, stats1N.BaseRange);
            Assert.Equal(0, stats1N.BaseShield);
            Assert.Equal(0, stats1N.BaseRetaliate);
            Assert.Equal(0, stats1N.BaseRetaliateRange);
            Assert.Equal(0, stats1N.BasePierce);
            Assert.Equal(1, stats1N.BaseTarget);
                                 
            // Level 1 Elite     
            Assert.Equal(9, stats1E.Health);
            Assert.Equal(2, stats1E.BaseMove);
            Assert.Equal(3, int.Parse(stats1E.BaseAttackFormula));
            Assert.Equal(0, stats1E.BaseRange);
            Assert.Equal(1, stats1E.BaseShield);
            Assert.Equal(0, stats1E.BaseRetaliate);
            Assert.Equal(0, stats1E.BaseRetaliateRange);
            Assert.Equal(0, stats1E.BasePierce);
            Assert.Equal(1, stats1E.BaseTarget);
            
            // Level 2 Normal
            Assert.Equal(6, stats2N.Health);
            Assert.Equal(3, stats2N.BaseMove);
            Assert.Equal(3, int.Parse(stats2N.BaseAttackFormula));
            Assert.Equal(0, stats2N.BaseRange);
            Assert.Equal(0, stats2N.BaseShield);
            Assert.Equal(0, stats2N.BaseRetaliate);
            Assert.Equal(0, stats2N.BaseRetaliateRange);
            Assert.Equal(0, stats2N.BasePierce);
            Assert.Equal(1, stats2N.BaseTarget);
                                 
            // Level 2 Elite     
            Assert.Equal(10, stats2E.Health);
            Assert.Equal(2, stats2E.BaseMove);
            Assert.Equal(4, int.Parse(stats2E.BaseAttackFormula));
            Assert.Equal(0, stats2E.BaseRange);
            Assert.Equal(1, stats2E.BaseShield);
            Assert.Equal(0, stats2E.BaseRetaliate);
            Assert.Equal(0, stats2E.BaseRetaliateRange);
            Assert.Equal(0, stats2E.BasePierce);
            Assert.Equal(1, stats2E.BaseTarget);
            
            // Level 3 Normal
            Assert.Equal(9, stats3N.Health);
            Assert.Equal(3, stats3N.BaseMove);
            Assert.Equal(3, int.Parse(stats3N.BaseAttackFormula));
            Assert.Equal(0, stats3N.BaseRange);
            Assert.Equal(0, stats3N.BaseShield);
            Assert.Equal(0, stats3N.BaseRetaliate);
            Assert.Equal(0, stats3N.BaseRetaliateRange);
            Assert.Equal(0, stats3N.BasePierce);
            Assert.Equal(1, stats3N.BaseTarget);
                                 
            // Level 3 Elite     
            Assert.Equal(10, stats3E.Health);
            Assert.Equal(3, stats3E.BaseMove);
            Assert.Equal(4, int.Parse(stats3E.BaseAttackFormula));
            Assert.Equal(0, stats3E.BaseRange);
            Assert.Equal(2, stats3E.BaseShield);
            Assert.Equal(0, stats3E.BaseRetaliate);
            Assert.Equal(0, stats3E.BaseRetaliateRange);
            Assert.Equal(0, stats3E.BasePierce);
            Assert.Equal(1, stats3E.BaseTarget);
            
            // Level 4 Normal
            Assert.Equal(10, stats4N.Health);
            Assert.Equal(4, stats4N.BaseMove);
            Assert.Equal(3, int.Parse(stats4N.BaseAttackFormula));
            Assert.Equal(0, stats4N.BaseRange);
            Assert.Equal(0, stats4N.BaseShield);
            Assert.Equal(0, stats4N.BaseRetaliate);
            Assert.Equal(0, stats4N.BaseRetaliateRange);
            Assert.Equal(0, stats4N.BasePierce);
            Assert.Equal(1, stats4N.BaseTarget);
                                 
            // Level 4 Elite     
            Assert.Equal(11, stats4E.Health);
            Assert.Equal(3, stats4E.BaseMove);
            Assert.Equal(4, int.Parse(stats4E.BaseAttackFormula));
            Assert.Equal(0, stats4E.BaseRange);
            Assert.Equal(2, stats4E.BaseShield);
            Assert.Equal(0, stats4E.BaseRetaliate);
            Assert.Equal(0, stats4E.BaseRetaliateRange);
            Assert.Equal(0, stats4E.BasePierce);
            Assert.Equal(1, stats4E.BaseTarget);
            Assert.Contains(StatusType.Muddle, stats4E.StatusesInflicted);
            
            // Level 5 Normal
            Assert.Equal(11, stats5N.Health);
            Assert.Equal(4, stats5N.BaseMove);
            Assert.Equal(4, int.Parse(stats5N.BaseAttackFormula));
            Assert.Equal(0, stats5N.BaseRange);
            Assert.Equal(0, stats5N.BaseShield);
            Assert.Equal(0, stats5N.BaseRetaliate);
            Assert.Equal(0, stats5N.BaseRetaliateRange);
            Assert.Equal(0, stats5N.BasePierce);
            Assert.Equal(1, stats5N.BaseTarget);
                                 
            // Level 5 Elite     
            Assert.Equal(12, stats5E.Health);
            Assert.Equal(3, stats5E.BaseMove);
            Assert.Equal(5, int.Parse(stats5E.BaseAttackFormula));
            Assert.Equal(0, stats5E.BaseRange);
            Assert.Equal(2, stats5E.BaseShield);
            Assert.Equal(0, stats5E.BaseRetaliate);
            Assert.Equal(0, stats5E.BaseRetaliateRange);
            Assert.Equal(0, stats5E.BasePierce);
            Assert.Equal(1, stats5E.BaseTarget);
            Assert.Contains(StatusType.Muddle, stats5E.StatusesInflicted);
            
            // Level 6 Normal
            Assert.Equal(14, stats6N.Health);
            Assert.Equal(4, stats6N.BaseMove);
            Assert.Equal(4, int.Parse(stats6N.BaseAttackFormula));
            Assert.Equal(0, stats6N.BaseRange);
            Assert.Equal(0, stats6N.BaseShield);
            Assert.Equal(0, stats6N.BaseRetaliate);
            Assert.Equal(0, stats6N.BaseRetaliateRange);
            Assert.Equal(0, stats6N.BasePierce);
            Assert.Equal(1, stats6N.BaseTarget);
                                 
            // Level 6 Elite     
            Assert.Equal(14, stats6E.Health);
            Assert.Equal(3, stats6E.BaseMove);
            Assert.Equal(5, int.Parse(stats6E.BaseAttackFormula));
            Assert.Equal(0, stats6E.BaseRange);
            Assert.Equal(2, stats6E.BaseShield);
            Assert.Equal(0, stats6E.BaseRetaliate);
            Assert.Equal(0, stats6E.BaseRetaliateRange);
            Assert.Equal(0, stats6E.BasePierce);
            Assert.Equal(1, stats6E.BaseTarget);
            Assert.Contains(StatusType.Muddle, stats6E.StatusesInflicted);
            
            // Level 7 Normal
            Assert.Equal(16, stats7N.Health);
            Assert.Equal(5, stats7N.BaseMove);
            Assert.Equal(4, int.Parse(stats7N.BaseAttackFormula));
            Assert.Equal(0, stats7N.BaseRange);
            Assert.Equal(0, stats7N.BaseShield);
            Assert.Equal(0, stats7N.BaseRetaliate);
            Assert.Equal(0, stats7N.BaseRetaliateRange);
            Assert.Equal(0, stats7N.BasePierce);
            Assert.Equal(1, stats7N.BaseTarget);
                                 
            // Level 7 Elite     
            Assert.Equal(14, stats7E.Health);
            Assert.Equal(3, stats7E.BaseMove);
            Assert.Equal(5, int.Parse(stats7E.BaseAttackFormula));
            Assert.Equal(0, stats7E.BaseRange);
            Assert.Equal(3, stats7E.BaseShield);
            Assert.Equal(0, stats7E.BaseRetaliate);
            Assert.Equal(0, stats7E.BaseRetaliateRange);
            Assert.Equal(0, stats7E.BasePierce);
            Assert.Equal(1, stats7E.BaseTarget);
            Assert.Contains(StatusType.Muddle, stats7E.StatusesInflicted);
        }

        [Fact]
        public static void VerifyEdgeCaseStats_RangedRetaliate()
        {
            var group = SetupFlameDemonGroup(5);
            var stats5E = group.EliteStats;
            
            Assert.Equal(6, stats5E.Health);
            Assert.Equal(4, stats5E.BaseMove);
            Assert.Equal(4, int.Parse(stats5E.BaseAttackFormula));
            Assert.Equal(5, stats5E.BaseRange);
            Assert.Equal(5, stats5E.BaseShield);
            Assert.Equal(4, stats5E.BaseRetaliate);
            Assert.Equal(3, stats5E.BaseRetaliateRange);
            Assert.Equal(0, stats5E.BasePierce);
            Assert.Equal(1, stats5E.BaseTarget);
            Assert.True(stats5E.IsFlying);
            Assert.False(stats5E.DoAttackersGainDisadvantage);
        }

        [Fact]
        public static void VerifyEdgeCaseStats_AgD()
        {
            string name = "Black Imp";
            MonsterType blackImp = new MonsterType (name, "Imp")
            {
            };

            MonsterGrouping group = new MonsterGrouping(blackImp, 7);
            var stats7E = group.EliteStats;

                        
            Assert.Equal(17, stats7E.Health);
            Assert.Equal(1, stats7E.BaseMove);
            Assert.Equal(4, int.Parse(stats7E.BaseAttackFormula));
            Assert.Equal(5, stats7E.BaseRange);
            Assert.Equal(0, stats7E.BaseShield);
            Assert.Equal(0, stats7E.BaseRetaliate);
            Assert.Equal(0, stats7E.BaseRetaliateRange);
            Assert.Equal(0, stats7E.BasePierce);
            Assert.Equal(1, stats7E.BaseTarget);
            Assert.False(stats7E.IsFlying);
            Assert.Contains(StatusType.Poison, stats7E.StatusesInflicted);
            Assert.True(stats7E.DoAttackersGainDisadvantage);
        }

        [Fact]
        public static void VerifyEdgeCaseStats_Target()
        {
            string name = "Lurker";
            MonsterType lurker = new MonsterType (name)
            {
            };

            MonsterGrouping group = new MonsterGrouping(lurker, 1);
            var stats1E = group.EliteStats;
            
            Assert.Equal(9, stats1E.Health);
            Assert.Equal(2, stats1E.BaseMove);
            Assert.Equal(3, int.Parse(stats1E.BaseAttackFormula));
            Assert.Equal(0, stats1E.BaseRange);
            Assert.Equal(1, stats1E.BaseShield);
            Assert.Equal(0, stats1E.BaseRetaliate);
            Assert.Equal(0, stats1E.BaseRetaliateRange);
            Assert.Equal(1, stats1E.BasePierce);
            Assert.Equal(2, stats1E.BaseTarget);
            Assert.False(stats1E.IsFlying);
            Assert.False(stats1E.DoAttackersGainDisadvantage);
        }
        
        [Fact]
        public static void VerifyEdgeCaseStats_Curse()
        {
            string name = "Forest Imp";
            MonsterType forestImp = new MonsterType (name, "Imp")
            {
            };

            MonsterGrouping group = new MonsterGrouping(forestImp, 7);
            var stats7E = group.EliteStats;

                        
            Assert.Equal(11, stats7E.Health);
            Assert.Equal(4, stats7E.BaseMove);
            Assert.Equal(4, int.Parse(stats7E.BaseAttackFormula));
            Assert.Equal(4, stats7E.BaseRange);
            Assert.Equal(2, stats7E.BaseShield);
            Assert.Equal(0, stats7E.BaseRetaliate);
            Assert.Equal(0, stats7E.BaseRetaliateRange);
            Assert.Equal(0, stats7E.BasePierce);
            Assert.Equal(1, stats7E.BaseTarget);
            Assert.True(stats7E.IsFlying);
            Assert.False(stats7E.DoAttackersGainDisadvantage);
            Assert.False(stats7E.HasAdvantage);
            Assert.Contains(StatusType.Curse, stats7E.StatusesInflicted);
        }

        [Fact]
        public static void InvalidLevel()
        {
            Assert.Throws<IndexOutOfRangeException>(() =>
                SetupBanditGuardGroup(8));
        }

        [Fact]
        public static void InvalidMonsterName()
        {
            string name = "Boogey Man";
            Assert.Throws<MonsterStatsNotFoundException>( () =>new MonsterStats(name));
        }
    }
}