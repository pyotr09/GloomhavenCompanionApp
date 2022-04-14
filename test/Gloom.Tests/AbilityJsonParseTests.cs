using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Gloom.Model;
using Gloom.Model.Monsters;
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
                new MonsterType("Living Bones"),
                7);

            group.AbilityDeck.Cards.ForEach(
                c =>
                    c.Actions.ForEach(a =>
                    {
                        _testOutputHelper.WriteLine(a.BaseActionText);
                        _testOutputHelper.WriteLine(a.NormalActionText);
                        _testOutputHelper.WriteLine(a.EliteActionText);
                    })
            );
        }
    }
}