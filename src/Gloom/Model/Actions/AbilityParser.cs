using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Gloom.Model.Monsters;
using Newtonsoft.Json.Linq;

namespace Gloom.Model.Actions
{
    public class AbilityParser
    {
        private static AbilityParser _parser;
        public static AbilityParser Instance => _parser ??= new AbilityParser();

        private AbilityParser()
        {
            StreamReader r = new StreamReader("MonsterAbilities.json");
            string jsonString = r.ReadToEnd();
            _rootArray = JArray.Parse(jsonString);
        }
             
        private readonly JArray _rootArray;

        public MonsterAbilityDeck ParseDeck(MonsterGrouping group)
        {
            var cards = new List<MonsterAbilityCard>();

            var abilityCardsToken = _rootArray.SelectToken($"$[?(@.Name == '{group.DeckName}')]")
                .SelectToken("AbilityCards").Where(j => j["Initiative"] != null);
            
            foreach (var abilityCardJson in abilityCardsToken)
            {
                var card = new MonsterAbilityCard();
                card.Initiative = (int) abilityCardJson["Initiative"];
                card.ShuffleAfter = (bool) abilityCardJson["Shuffle"];
                card.ImagePath = (string) abilityCardJson["Image"];
                card.Name = (string) abilityCardJson["Name"];
                card.Expansion = (string) abilityCardJson["Expansion"];
                card.Actions = new List<ActionSet>();
                foreach (string actionString in abilityCardJson["Actions"])
                {
                    card.Actions.Add(CombineStatsAndActionText(actionString, group.NormalStats, group.EliteStats));
                }
                cards.Add(card);
            }
            
            var deck = new MonsterAbilityDeck(cards);
            return deck;
        }

        private ActionSet CombineStatsAndActionText(string actionString, BaseMonsterStats normalStats,
            BaseMonsterStats eliteStats)
        {
            var actionGroups = actionString.Split(',', StringSplitOptions.TrimEntries);

            var primaryAction = actionGroups[0];

            var moveMatch = Regex.Match(primaryAction, @"Move [+-]\d");
            if (moveMatch.Success)
            {
                return GenerateMoveActionSet(normalStats, eliteStats, primaryAction, actionGroups, actionString);
            }

            var attackMatch = Regex.Match(primaryAction, @"Attack [+-]\d");
            if (attackMatch.Success)
            {
                return GenerateAttackActionSet(normalStats, eliteStats, primaryAction, actionGroups, actionString);
            }

            if (
                Status.StatusStrings.Any(s => primaryAction.StartsWith(s))
                || primaryAction.StartsWith("Heal")
                )
            {
                var builder = new StringBuilder(primaryAction);
                for (int i = 1; i < actionGroups.Length; i++)
                {
                    builder.Append($"\n - {actionGroups[i]}");
                }

                var s = builder.ToString();

                return new ActionSet
                {
                    BaseActionText = actionString,
                    NormalActionText = s,
                    EliteActionText = s
                };
            }

            return new ActionSet {
                BaseActionText = actionString, 
                NormalActionText = actionString, 
                EliteActionText = actionString 
            };
        }

        private static ActionSet GenerateAttackActionSet(BaseMonsterStats normalStats, BaseMonsterStats eliteStats,
            string primaryAction, string[] actionGroups, string actionString)
        {
            int attackValue = int.Parse(primaryAction.Substring(7, 2));

            int normalAttackTotal = attackValue + normalStats.BaseAttack;
            var normalTextBuilder = new StringBuilder("Attack " + normalAttackTotal);

            int eliteAttackTotal = attackValue + eliteStats.BaseAttack;
            var eliteTextBuilder = new StringBuilder("Attack " + eliteAttackTotal);

            int normalRange = normalStats.BaseRange;
            int eliteRange = eliteStats.BaseRange;

            var normalToAddBuilder = new StringBuilder();
            var eliteToAddBuilder = new StringBuilder();

            // secondary effects from ability card
            for (int i = 1; i < actionGroups.Length; i++)
            {
                var subActionText = actionGroups[i];
                var rangeConstantMatch = Regex.Match(subActionText, @"Range \d");
                var rangeModifierMatch = Regex.Match(subActionText, @"Range [+-]\d");

                if (rangeConstantMatch.Success)
                {
                    normalRange = int.Parse(subActionText.Substring(6, 1));
                    eliteRange = int.Parse(subActionText.Substring(6, 1));
                }
                else if (rangeModifierMatch.Success)
                {
                    normalRange += int.Parse(subActionText.Substring(6, 2));
                    eliteRange += int.Parse(subActionText.Substring(6, 2));
                }

                if (Regex.Match(subActionText, "Target.*adjacent").Success
                    || Regex.Match(subActionText, "Area.*line|melee").Success)
                {
                    normalRange = 0;
                    eliteRange = 0;
                }

                // Effects that don't modify anything, just display them
                if (
                    Status.StatusStrings.Contains(subActionText)
                    || subActionText.StartsWith("Target") // we assume this overrides any monster stat Target, rather than stacking
                    || subActionText.StartsWith("Consume")
                    || subActionText.StartsWith("Create")
                    || subActionText.StartsWith("Push")
                    || subActionText.StartsWith("Pull")
                    || subActionText.StartsWith("Pierce")
                    || subActionText.StartsWith("Area")
                    )
                {
                    normalToAddBuilder.Append("\n - " + subActionText);
                    eliteToAddBuilder.Append("\n - " + subActionText);
                }
            }

            var normalTextToAdd = normalToAddBuilder.ToString();
            var eliteTextToAdd = eliteToAddBuilder.ToString();

            if (normalRange > 0)
            {
                normalTextBuilder.Append("\n - Range " + normalRange);
            }
            if (eliteRange > 0)
            {
                eliteTextBuilder.Append("\n - Range " + eliteRange);
            }

            // secondary effects from monster stats
            foreach (var statusType in normalStats.StatusesInflicted)
            {
                normalTextBuilder.Append("\n - " + Status.GetStringForStatus(statusType));
            }
            foreach (var statusType in eliteStats.StatusesInflicted)
            {
                eliteTextBuilder.Append("\n - " + Status.GetStringForStatus(statusType));
            }

            if (normalStats.BaseTarget > 1 && !normalTextToAdd.Contains("Target")) // could be overridden by target from ability
            {
                normalTextBuilder.Append("\n - Target " + normalStats.BaseTarget);
            }
            if (eliteStats.BaseTarget > 1 && !eliteTextToAdd.Contains("Target")) 
            {
                eliteTextBuilder.Append("\n - Target " + eliteStats.BaseTarget);
            }

            if (normalStats.HasAdvantage)
                normalTextBuilder.Append("\n - Advantage");
            if (eliteStats.HasAdvantage) 
                eliteTextBuilder.Append("\n - Advantage");

            normalTextBuilder.Append(normalToAddBuilder);
            eliteTextBuilder.Append(eliteToAddBuilder);
            
            

            return new ActionSet
            {
                BaseActionText = actionString,
                NormalActionText = normalTextBuilder.ToString(),
                EliteActionText = eliteTextBuilder.ToString()
            };
        }

        private static ActionSet GenerateMoveActionSet(BaseMonsterStats normalStats, BaseMonsterStats eliteStats,
            string primaryAction, string[] actionGroups, string actionString)
        {
            int moveValue = int.Parse(primaryAction.Substring(5, 2));

            int normalMoveTotal = moveValue + normalStats.BaseMove;
            var normalText = new StringBuilder("Move " + normalMoveTotal);

            int eliteMoveTotal = moveValue + eliteStats.BaseMove;
            var eliteText = new StringBuilder("Move " + eliteMoveTotal);

            if (eliteStats.IsFlying)
                eliteText.Append("\n - Flying");
            if (normalStats.IsFlying)
                normalText.Append("\n - Flying");

            // Jump is only secondary effect on Move abilities so far
            if (actionGroups.Length == 2 && actionGroups[1] == "Jump")
            {
                eliteText.Append("\n - Jump");
                normalText.Append("\n - Jump");
            }

            return new ActionSet
            {
                BaseActionText = actionString,
                NormalActionText = normalText.ToString(),
                EliteActionText = eliteText.ToString()
            };
        }
    }
}