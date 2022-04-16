using Gloom.Model.Bosses;
using Xunit;

namespace Gloom.Tests
{
    public class BossTests
    {
        [Fact]
        public static void BossHealthCalculationTest()
        {
            BossStats stats = new BossStats("Bandit Commander");
            BossType type = new BossType (stats);
            
            Boss b = new Boss(type, 3, 4);
            // Health is 13xC at level 3 for Bandit Commander
            
            Assert.Equal(52, b.MaxHealth);
        }
        [Fact]
        public static void BossAttackCalculationTest()
        {
            string bossName = "Bandit Commander";
            BossStats stats = new BossStats(bossName);
            BossType type = new BossType(stats);
            Boss b = new Boss(type, 3, 4);
            // Attack is 4 for Bandit Commander
            // Inox BodyGuard: 2+C, C is number of characters.
            // Merciless Overseer: V, V is number of scouts present.
            // Dark Rider: 4+X, X is hexes moved.
            Assert.Equal(4.ToString(),b.BaseAttack);
            
        }
    }
}