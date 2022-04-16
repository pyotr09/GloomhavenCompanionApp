using Gloom.Model.Player_Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Gloom.Tests
{
    public class CharacterTests
    {
        [Fact]
        public void test()
        {
            int maxHealth = 58;
            int level = 482;
            string name = "Narge";
            Character c = new Character(maxHealth, level, name);

            c.DecreaseCurrentHealthBy(5);
            c.SetInitiative(50);
            int Init = c.Initiative.Value;
        }
    }
}
