using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Gloom.Common;
using Gloom.Models.Interfaces;
using Gloom.Models.Monsters;
using Newtonsoft.Json.Linq;

namespace Gloom.Models.Actions
{
    public class AbilityParser
    {
        private static AbilityParser _parser;
        public static AbilityParser Instance => _parser ??= new AbilityParser();

        private AbilityParser()
        {
            StreamReader r = new StreamReader("Resources/MonsterAbilities.json");
            string jsonString = r.ReadToEnd();
            _rootArray = JArray.Parse(jsonString);
        }
             
        private readonly JArray _rootArray;

        public MonsterAbilityDeck ParseDeck(IScenarioParticipantGroup group)
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
                    card.Actions.Add(CombineStatsAndActionText(actionString, group.BaseStatsList));
                }
                cards.Add(card);
            }
            
            var deck = new MonsterAbilityDeck(cards);
            return deck;
        }

        private ActionSet CombineStatsAndActionText(string actionString, List<BaseStats> statsList)
        {
            var actionGroups = actionString.Split(',', StringSplitOptions.TrimEntries);

            var primaryAction = actionGroups[0];

            var moveMatch = Regex.Match(primaryAction, @"Move [+-]\d");
            if (moveMatch.Success)
            {
                return GenerateMoveActionSet(statsList, primaryAction, actionGroups, actionString);
            }

            var attackMatch = Regex.Match(primaryAction, @"^Attack [+-]\d");
            if (attackMatch.Success)
            {
                return GenerateAttackActionSet(statsList, primaryAction, actionGroups, actionString);
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

        private static ActionSet GenerateAttackActionSet( List<BaseStats> statsList,
            string primaryAction, string[] actionGroups, string actionString)
        {
            var attackModString = primaryAction.Substring(7, 2);
            int attackValue = int.Parse(attackModString);

            var actionTextBuilders = new Dictionary<MonsterTier, StringBuilder>();
            foreach (var stats in statsList)
            {
                StringBuilder textBuilder = new StringBuilder();
                if (int.TryParse(stats.BaseAttackFormula, out var atk))
                {
                    textBuilder.Append($"Attack {attackValue + atk}");
                }
                else
                {
                    // ie "Attack +1" for Dark Rider would be: "Attack 3+X+1"
                    textBuilder.Append($"Attack {stats.BaseAttackFormula}{attackModString}");
                }

                int range = stats.BaseRange;
                
                var toAddBuilder = new StringBuilder();
                
                // secondary effects from ability card
                for (int i = 1; i < actionGroups.Length; i++)
                {
                    var subActionText = actionGroups[i];
                    var rangeConstantMatch = Regex.Match(subActionText, @"Range \d");
                    var rangeModifierMatch = Regex.Match(subActionText, @"Range [+-]\d");

                    if (rangeConstantMatch.Success)
                    {
                        range = int.Parse(subActionText.Substring(6, 1));
                    }
                    else if (rangeModifierMatch.Success)
                    {
                        range += int.Parse(subActionText.Substring(6, 2));
                    }

                    if (Regex.Match(subActionText, "Target.*adjacent").Success
                        || Regex.Match(subActionText, "Area.*line|melee").Success)
                    {
                        range = 0;
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
                        toAddBuilder.Append($"\n - {subActionText}");
                    }
                }

                var textToAdd = toAddBuilder.ToString();
                if (range > 0)
                {
                    textBuilder.Append("\n - Range " + range);
                }

                // secondary effects from monster stats
                if (stats.StatusesInflicted != null)
                {
                    foreach (var statusType in stats.StatusesInflicted)
                    {
                        textBuilder.Append("\n - " + Status.GetStringForStatus(statusType));
                    }
                }

                if (stats.BaseTarget > 1 && !textToAdd.Contains("Target")) // could be overridden by target from ability
                {
                    textBuilder.Append("\n - Target " + stats.BaseTarget);
                }

                if (stats.HasAdvantage)
                    textBuilder.Append("\n - Advantage");

                textBuilder.Append(toAddBuilder);
                actionTextBuilders.Add(stats.Tier, textBuilder);
            }

            if (statsList.Count == 1) // boss
            {
                return new ActionSet
                {
                    BaseActionText = actionString,
                    NormalActionText = actionTextBuilders[MonsterTier.Boss].ToString(),
                    EliteActionText = actionTextBuilders[MonsterTier.Boss].ToString()
                };
            }

            return new ActionSet
            {
                BaseActionText = actionString,
                NormalActionText = actionTextBuilders[MonsterTier.Normal].ToString(),
                EliteActionText = actionTextBuilders[MonsterTier.Elite].ToString()
            };
        }

        private static ActionSet GenerateMoveActionSet( List<BaseStats> statsList,
            string primaryAction, string[] actionGroups, string actionString)
        {
            var actionTextBuilders = new Dictionary<MonsterTier, StringBuilder>();
            foreach (var stats in statsList)
            {
                int moveValue = int.Parse(primaryAction.Substring(5, 2));
                
                int moveTotal = moveValue + stats.BaseMove;
                var text = new StringBuilder("Move " + moveTotal);

                if (stats.IsFlying)
                    text.Append("\n - Flying");

                // Jump is only secondary effect on Move abilities so far
                if (actionGroups.Length == 2 && actionGroups[1] == "Jump")
                {
                    text.Append("\n - Jump");
                }
                actionTextBuilders.Add(stats.Tier, text);
            }

            if (statsList.Count == 1) // boss
            {
                return new ActionSet
                {
                    BaseActionText = actionString,
                    NormalActionText = actionTextBuilders[MonsterTier.Boss].ToString(),
                    EliteActionText = actionTextBuilders[MonsterTier.Boss].ToString()
                };
            }

            return new ActionSet
            {
                BaseActionText = actionString,
                NormalActionText = actionTextBuilders[MonsterTier.Normal].ToString(),
                EliteActionText = actionTextBuilders[MonsterTier.Elite].ToString()
            };
        }
    }
}