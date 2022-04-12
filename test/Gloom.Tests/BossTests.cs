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
    }
}