using System.Collections.Generic;
using Gloom.Model;
using Xunit;

namespace Gloom.Tests
{
    public class MonsterTests
    {
        private static Monster SetupLevelOneBanditGuard()
        {
            MonsterStats stats = new MonsterStats
            {
                StatsByLevel_Normal = new BaseStats[8],
                StatsByLevel_Elite = new BaseStats[8]
            };
            stats.StatsByLevel_Normal[1] = new BaseStats
            {
                HitPoints = 10,
                BaseAttack = 2,
                BaseMove = 2,
                BaseShield = 1,
                BaseRetaliate = 1
            };

            MonsterType banditGuard = new MonsterType
            {
                Name = "Bandit Guard",
                AbilityDeck = new MonsterAbilityDeck(new List<MonsterAbilityCard>()),
                Stats = stats,
                IsFlying = false,
                MaxNumberOnBoard = 6
            };

            MonsterGrouping group = new MonsterGrouping(banditGuard);
            Monster guard1 = new Monster(group, 1, 1, MonsterTier.Normal);
            return guard1;
        }
        
        [Fact]
        public void EndOfRound_RetaliateTest()
        {
            var guard1 = SetupLevelOneBanditGuard();

            guard1.CurrentRetaliate += 2;
            guard1.RefreshForEndOfRound();
            
            Assert.Equal(1, guard1.CurrentRetaliate);
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
    }
}