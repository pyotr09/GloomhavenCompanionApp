using System;
using System.Collections.Generic;
using Gloom.CustomExceptions;
using Gloom.Data;
using Gloom.Model;
using Gloom.Model.Monsters;
using Xunit;
using Monster = Gloom.Model.Monster;

namespace Gloom.Tests
{
    public class MonsterTests
    {
        private static Monster SetupLevelOneBanditGuard()
        {
            var group = SetupBanditGuardGroup();
            Monster guard1 = new Monster(group, 1, 1, MonsterTier.Normal);
            return guard1;
        }

        private static MonsterGrouping SetupBanditGuardGroup()
        {
            string name = "Bandit Guard";
            MonsterStats stats = new MonsterStats(name);
            MonsterType banditGuard = new MonsterType
            {
                Name = name,
                AbilityDeck = new MonsterAbilityDeck(new List<MonsterAbilityCard>()),
                Stats = stats,
                MaxNumberOnBoard = 6
            };

            MonsterGrouping group = new MonsterGrouping(banditGuard);
            return group;
        }
        
        private static MonsterGrouping SetupFlameDemonGroup()
        {
            string name = "Flame Demon";
            MonsterStats stats = new MonsterStats(name);
            MonsterType flameDemon = new MonsterType
            {
                Name = name,
                AbilityDeck = new MonsterAbilityDeck(new List<MonsterAbilityCard>()),
                Stats = stats,
                MaxNumberOnBoard = 6
            };

            MonsterGrouping group = new MonsterGrouping(flameDemon);
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
            MonsterGrouping group = SetupBanditGuardGroup();
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
            var group = SetupBanditGuardGroup();
            var stats0N = group.Type.Stats.GetStatsByLevelAndTier(0, MonsterTier.Normal);
            var stats1N = group.Type.Stats.GetStatsByLevelAndTier(1, MonsterTier.Normal);
            var stats2N = group.Type.Stats.GetStatsByLevelAndTier(2, MonsterTier.Normal);
            var stats3N = group.Type.Stats.GetStatsByLevelAndTier(3, MonsterTier.Normal);
            var stats4N = group.Type.Stats.GetStatsByLevelAndTier(4, MonsterTier.Normal);
            var stats5N = group.Type.Stats.GetStatsByLevelAndTier(5, MonsterTier.Normal);
            var stats6N = group.Type.Stats.GetStatsByLevelAndTier(6, MonsterTier.Normal);
            var stats7N = group.Type.Stats.GetStatsByLevelAndTier(7, MonsterTier.Normal);
            
            var stats0E = group.Type.Stats.GetStatsByLevelAndTier(0, MonsterTier.Elite);
            var stats1E = group.Type.Stats.GetStatsByLevelAndTier(1, MonsterTier.Elite);
            var stats2E = group.Type.Stats.GetStatsByLevelAndTier(2, MonsterTier.Elite);
            var stats3E = group.Type.Stats.GetStatsByLevelAndTier(3, MonsterTier.Elite);
            var stats4E = group.Type.Stats.GetStatsByLevelAndTier(4, MonsterTier.Elite);
            var stats5E = group.Type.Stats.GetStatsByLevelAndTier(5, MonsterTier.Elite);
            var stats6E = group.Type.Stats.GetStatsByLevelAndTier(6, MonsterTier.Elite);
            var stats7E = group.Type.Stats.GetStatsByLevelAndTier(7, MonsterTier.Elite);
            
            // Level 0 Normal
            Assert.Equal(5, stats0N.HitPoints);
            Assert.Equal(2, stats0N.BaseMove);
            Assert.Equal(2, stats0N.BaseAttack);
            Assert.Equal(0, stats0N.BaseRange);
            Assert.Equal(0, stats0N.BaseShield);
            Assert.Equal(0, stats0N.BaseRetaliate);
            Assert.Equal(0, stats0N.BaseRetaliateRange);
            Assert.Equal(0, stats0N.BasePierce);
            Assert.Equal(1, stats0N.BaseTarget);
            
            // Level 0 Elite
            Assert.Equal(9, stats0E.HitPoints);
            Assert.Equal(2, stats0E.BaseMove);
            Assert.Equal(3, stats0E.BaseAttack);
            Assert.Equal(0, stats0E.BaseRange);
            Assert.Equal(0, stats0E.BaseShield);
            Assert.Equal(0, stats0E.BaseRetaliate);
            Assert.Equal(0, stats0E.BaseRetaliateRange);
            Assert.Equal(0, stats0E.BasePierce);
            Assert.Equal(1, stats0E.BaseTarget);
            
            // Level 1 Normal
            Assert.Equal(6, stats1N.HitPoints);
            Assert.Equal(3, stats1N.BaseMove);
            Assert.Equal(2, stats1N.BaseAttack);
            Assert.Equal(0, stats1N.BaseRange);
            Assert.Equal(0, stats1N.BaseShield);
            Assert.Equal(0, stats1N.BaseRetaliate);
            Assert.Equal(0, stats1N.BaseRetaliateRange);
            Assert.Equal(0, stats1N.BasePierce);
            Assert.Equal(1, stats1N.BaseTarget);
                                 
            // Level 1 Elite     
            Assert.Equal(9, stats1E.HitPoints);
            Assert.Equal(2, stats1E.BaseMove);
            Assert.Equal(3, stats1E.BaseAttack);
            Assert.Equal(0, stats1E.BaseRange);
            Assert.Equal(1, stats1E.BaseShield);
            Assert.Equal(0, stats1E.BaseRetaliate);
            Assert.Equal(0, stats1E.BaseRetaliateRange);
            Assert.Equal(0, stats1E.BasePierce);
            Assert.Equal(1, stats1E.BaseTarget);
            
            // Level 2 Normal
            Assert.Equal(6, stats2N.HitPoints);
            Assert.Equal(3, stats2N.BaseMove);
            Assert.Equal(3, stats2N.BaseAttack);
            Assert.Equal(0, stats2N.BaseRange);
            Assert.Equal(0, stats2N.BaseShield);
            Assert.Equal(0, stats2N.BaseRetaliate);
            Assert.Equal(0, stats2N.BaseRetaliateRange);
            Assert.Equal(0, stats2N.BasePierce);
            Assert.Equal(1, stats2N.BaseTarget);
                                 
            // Level 2 Elite     
            Assert.Equal(10, stats2E.HitPoints);
            Assert.Equal(2, stats2E.BaseMove);
            Assert.Equal(4, stats2E.BaseAttack);
            Assert.Equal(0, stats2E.BaseRange);
            Assert.Equal(1, stats2E.BaseShield);
            Assert.Equal(0, stats2E.BaseRetaliate);
            Assert.Equal(0, stats2E.BaseRetaliateRange);
            Assert.Equal(0, stats2E.BasePierce);
            Assert.Equal(1, stats2E.BaseTarget);
            
            // Level 3 Normal
            Assert.Equal(9, stats3N.HitPoints);
            Assert.Equal(3, stats3N.BaseMove);
            Assert.Equal(3, stats3N.BaseAttack);
            Assert.Equal(0, stats3N.BaseRange);
            Assert.Equal(0, stats3N.BaseShield);
            Assert.Equal(0, stats3N.BaseRetaliate);
            Assert.Equal(0, stats3N.BaseRetaliateRange);
            Assert.Equal(0, stats3N.BasePierce);
            Assert.Equal(1, stats3N.BaseTarget);
                                 
            // Level 3 Elite     
            Assert.Equal(10, stats3E.HitPoints);
            Assert.Equal(3, stats3E.BaseMove);
            Assert.Equal(4, stats3E.BaseAttack);
            Assert.Equal(0, stats3E.BaseRange);
            Assert.Equal(2, stats3E.BaseShield);
            Assert.Equal(0, stats3E.BaseRetaliate);
            Assert.Equal(0, stats3E.BaseRetaliateRange);
            Assert.Equal(0, stats3E.BasePierce);
            Assert.Equal(1, stats3E.BaseTarget);
            
            // Level 4 Normal
            Assert.Equal(10, stats4N.HitPoints);
            Assert.Equal(4, stats4N.BaseMove);
            Assert.Equal(3, stats4N.BaseAttack);
            Assert.Equal(0, stats4N.BaseRange);
            Assert.Equal(0, stats4N.BaseShield);
            Assert.Equal(0, stats4N.BaseRetaliate);
            Assert.Equal(0, stats4N.BaseRetaliateRange);
            Assert.Equal(0, stats4N.BasePierce);
            Assert.Equal(1, stats4N.BaseTarget);
                                 
            // Level 4 Elite     
            Assert.Equal(11, stats4E.HitPoints);
            Assert.Equal(3, stats4E.BaseMove);
            Assert.Equal(4, stats4E.BaseAttack);
            Assert.Equal(0, stats4E.BaseRange);
            Assert.Equal(2, stats4E.BaseShield);
            Assert.Equal(0, stats4E.BaseRetaliate);
            Assert.Equal(0, stats4E.BaseRetaliateRange);
            Assert.Equal(0, stats4E.BasePierce);
            Assert.Equal(1, stats4E.BaseTarget);
            Assert.Contains(StatusType.Muddle, stats4E.StatusesInflicted);
            
            // Level 5 Normal
            Assert.Equal(11, stats5N.HitPoints);
            Assert.Equal(4, stats5N.BaseMove);
            Assert.Equal(4, stats5N.BaseAttack);
            Assert.Equal(0, stats5N.BaseRange);
            Assert.Equal(0, stats5N.BaseShield);
            Assert.Equal(0, stats5N.BaseRetaliate);
            Assert.Equal(0, stats5N.BaseRetaliateRange);
            Assert.Equal(0, stats5N.BasePierce);
            Assert.Equal(1, stats5N.BaseTarget);
                                 
            // Level 5 Elite     
            Assert.Equal(12, stats5E.HitPoints);
            Assert.Equal(3, stats5E.BaseMove);
            Assert.Equal(5, stats5E.BaseAttack);
            Assert.Equal(0, stats5E.BaseRange);
            Assert.Equal(2, stats5E.BaseShield);
            Assert.Equal(0, stats5E.BaseRetaliate);
            Assert.Equal(0, stats5E.BaseRetaliateRange);
            Assert.Equal(0, stats5E.BasePierce);
            Assert.Equal(1, stats5E.BaseTarget);
            Assert.Contains(StatusType.Muddle, stats5E.StatusesInflicted);
            
            // Level 6 Normal
            Assert.Equal(14, stats6N.HitPoints);
            Assert.Equal(4, stats6N.BaseMove);
            Assert.Equal(4, stats6N.BaseAttack);
            Assert.Equal(0, stats6N.BaseRange);
            Assert.Equal(0, stats6N.BaseShield);
            Assert.Equal(0, stats6N.BaseRetaliate);
            Assert.Equal(0, stats6N.BaseRetaliateRange);
            Assert.Equal(0, stats6N.BasePierce);
            Assert.Equal(1, stats6N.BaseTarget);
                                 
            // Level 6 Elite     
            Assert.Equal(14, stats6E.HitPoints);
            Assert.Equal(3, stats6E.BaseMove);
            Assert.Equal(5, stats6E.BaseAttack);
            Assert.Equal(0, stats6E.BaseRange);
            Assert.Equal(2, stats6E.BaseShield);
            Assert.Equal(0, stats6E.BaseRetaliate);
            Assert.Equal(0, stats6E.BaseRetaliateRange);
            Assert.Equal(0, stats6E.BasePierce);
            Assert.Equal(1, stats6E.BaseTarget);
            Assert.Contains(StatusType.Muddle, stats6E.StatusesInflicted);
            
            // Level 7 Normal
            Assert.Equal(16, stats7N.HitPoints);
            Assert.Equal(5, stats7N.BaseMove);
            Assert.Equal(4, stats7N.BaseAttack);
            Assert.Equal(0, stats7N.BaseRange);
            Assert.Equal(0, stats7N.BaseShield);
            Assert.Equal(0, stats7N.BaseRetaliate);
            Assert.Equal(0, stats7N.BaseRetaliateRange);
            Assert.Equal(0, stats7N.BasePierce);
            Assert.Equal(1, stats7N.BaseTarget);
                                 
            // Level 7 Elite     
            Assert.Equal(14, stats7E.HitPoints);
            Assert.Equal(3, stats7E.BaseMove);
            Assert.Equal(5, stats7E.BaseAttack);
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
            var group = SetupFlameDemonGroup();
            var stats5E = group.Type.Stats.GetStatsByLevelAndTier(5, MonsterTier.Elite);
            
            Assert.Equal(6, stats5E.HitPoints);
            Assert.Equal(4, stats5E.BaseMove);
            Assert.Equal(4, stats5E.BaseAttack);
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
            MonsterStats stats = new MonsterStats(name);
            MonsterType blackImp = new MonsterType
            {
                Name = name,
                AbilityDeck = new MonsterAbilityDeck(new List<MonsterAbilityCard>()),
                Stats = stats,
                MaxNumberOnBoard = 10
            };

            MonsterGrouping group = new MonsterGrouping(blackImp);
            var stats7E = group.Type.Stats.GetStatsByLevelAndTier(7, MonsterTier.Elite);

                        
            Assert.Equal(17, stats7E.HitPoints);
            Assert.Equal(1, stats7E.BaseMove);
            Assert.Equal(4, stats7E.BaseAttack);
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
            MonsterStats stats = new MonsterStats(name);
            MonsterType lurker = new MonsterType
            {
                Name = name,
                AbilityDeck = new MonsterAbilityDeck(new List<MonsterAbilityCard>()),
                Stats = stats,
                MaxNumberOnBoard = 6
            };

            MonsterGrouping group = new MonsterGrouping(lurker);
            var stats1E = group.Type.Stats.GetStatsByLevelAndTier(1, MonsterTier.Elite);
            
            Assert.Equal(9, stats1E.HitPoints);
            Assert.Equal(2, stats1E.BaseMove);
            Assert.Equal(3, stats1E.BaseAttack);
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
            MonsterStats stats = new MonsterStats(name);
            MonsterType forestImp = new MonsterType
            {
                Name = name,
                AbilityDeck = new MonsterAbilityDeck(new List<MonsterAbilityCard>()),
                Stats = stats,
                MaxNumberOnBoard = 6
            };

            MonsterGrouping group = new MonsterGrouping(forestImp);
            var stats7E = group.Type.Stats.GetStatsByLevelAndTier(7, MonsterTier.Elite);

                        
            Assert.Equal(11, stats7E.HitPoints);
            Assert.Equal(4, stats7E.BaseMove);
            Assert.Equal(4, stats7E.BaseAttack);
            Assert.Equal(4, stats7E.BaseRange);
            Assert.Equal(2, stats7E.BaseShield);
            Assert.Equal(0, stats7E.BaseRetaliate);
            Assert.Equal(0, stats7E.BaseRetaliateRange);
            Assert.Equal(0, stats7E.BasePierce);
            Assert.Equal(1, stats7E.BaseTarget);
            Assert.True(stats7E.IsFlying);
            Assert.False(stats7E.DoAttackersGainDisadvantage);
            Assert.False(stats7E.HasAdvantage);
            Assert.True(stats7E.DoesCurse);
        }

        [Fact]
        public static void InvalidLevel()
        {
            var group = SetupBanditGuardGroup();
            Assert.Throws<IndexOutOfRangeException>(() =>
                group.Type.Stats.GetStatsByLevelAndTier(8, MonsterTier.Normal));
        }

        [Fact]
        public static void InvalidMonsterName()
        {
            string name = "Boogey Man";
            Assert.Throws<MonsterStatsNotFoundException>( () =>new MonsterStats(name));
        }
    }
}