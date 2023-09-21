using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Gloom.Models.Monsters;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Gloom.Tests
{
    public class AbilityJsonParseTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public AbilityJsonParseTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void TestMonsterAbilities()
        {
            MonsterGrouping group = new MonsterGrouping(
                new MonsterType("Forest Imp", "Imp"),
                7);

            group.AbilityDeck.Cards.ForEach(
                c =>
                {
                    _testOutputHelper.WriteLine(c.Name + " " + c.Initiative);
                    c.Actions.ForEach(a =>
                    {
                        _testOutputHelper.WriteLine(a.BaseActionText);
                        _testOutputHelper.WriteLine(a.NormalActionText);
                        _testOutputHelper.WriteLine(a.EliteActionText);
                    });
                }
            );
        }

        [Fact]
        public void TestForestImp5()
        {
            //gh-ma-im-5
            MonsterGrouping group = new MonsterGrouping(new MonsterType("Forest Imp", "Imp"), 4);
            var card = group.AbilityDeck.Cards.First(c => c.Name == "gh-ma-im-5");

            var expectedNormalActions = "Move 4\n - Flying\nAttack 1\n - Range 4\n - Target 2\n - Poison";
            var expectedEliteActions = "Move 4\n - Flying\nAttack 1\n - Range 4\n - Curse\n - Target 2\n - Poison";
            
            Assert.Equal(expectedNormalActions, string.Join('\n', card.Actions.Select(a => a.NormalActionText)));
            Assert.Equal(expectedEliteActions, string.Join('\n', card.Actions.Select(a => a.EliteActionText)));
        }
    }
}