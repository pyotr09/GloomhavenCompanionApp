using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gloom.CustomExceptions;
using Gloom.Model.Actions;
using Gloom.Model.Interfaces;
using Newtonsoft.Json;

namespace Gloom.Model.Monsters
{
public class MonsterGrouping : IScenarioParticipantGroup
    {
        public MonsterGrouping()
        {
            _activeMonsterNumbers = new List<int>();
            _availableMonsterNumbers = new List<int>();
        }

        public MonsterGrouping(MonsterType type, int level)
        {
            Name = type.Name;
            DeckName = type.DeckName;
            var normalStats = type.Stats.GetStatsByLevelAndTier(level, MonsterTier.Normal);
            var eliteStats = type.Stats.GetStatsByLevelAndTier(level, MonsterTier.Elite);
            BaseStatsList = new List<BaseStats> {normalStats, eliteStats};
            Count = type.Stats.Count;
            Type = "Monster";
            Monsters = new List<Monster>();
            _activeMonsterNumbers = new List<int>(Count);
            _availableMonsterNumbers = new List<int>(Count);
            for (int i = 1; i <= Count; i++)
            {
                _availableMonsterNumbers.Add(i);
            }

            AbilityDeck = AbilityParser.Instance.ParseDeck(this);
        }

        public List<BaseStats> BaseStatsList { get; set; }
        public MonsterAbilityDeck AbilityDeck { get; set; }
        public string Type { get; set; }
        public int Count { get; set; }
        private static Random r = new Random();
        [JsonIgnore]
        public BaseMonsterStats NormalStats => (BaseMonsterStats) BaseStatsList.FirstOrDefault(s => s.Tier == MonsterTier.Normal);
        [JsonIgnore]
        public BaseMonsterStats EliteStats => (BaseMonsterStats) BaseStatsList.FirstOrDefault(s => s.Tier == MonsterTier.Elite);
        public int? Initiative => ActiveAbilityCard?.Initiative;
        public string Name { get; set; }
        [JsonIgnore]
        public string DeckName { get; set; }
        public List<Monster> Monsters { get; set; }
        public MonsterAbilityCard ActiveAbilityCard { get; set; }

        public void AddMonster(MonsterTier tier, int level, int? num = null)
        {
            if (num == null || num == -1)
                num = GetNewMonsterNumber();
            var stats = tier == MonsterTier.Elite ? EliteStats : NormalStats;
            Monsters.Add(new Monster(Name, level, num.Value, tier));
            _activeMonsterNumbers.Add(num.Value);
            _availableMonsterNumbers.Remove(num.Value);
        }

        public void RemoveMonster(int number)
        {
            Monsters.RemoveAll(m => m.MonsterNumber == number);
            _activeMonsterNumbers.Remove(number);
            _availableMonsterNumbers.Add(number);
        }

        private readonly List<int> _activeMonsterNumbers;
        private readonly List<int> _availableMonsterNumbers;
        private int GetNewMonsterNumber()
        {
            if (_activeMonsterNumbers.Count == Count)
                throw new AllMonsterNumbersUsedException("All Monster Numbers Used", Name); 
            int randomIndex = r.Next(1, _availableMonsterNumbers.Count);
            return _availableMonsterNumbers[randomIndex - 1];
        }

        public void Draw()
        {
            if (Monsters.Count > 0)
                DrawNewAbility();
        }

        private void DrawNewAbility()
        {
            ActiveAbilityCard = AbilityDeck.Draw();
        }

        public void RefreshForEndOfRound()
        {
            ActiveAbilityCard = null;
            if (AbilityDeck.DiscardPile.Any(c => c.ShuffleAfter))
            {
                AbilityDeck.ShuffleDiscardIntoDraw();
            }
            Monsters.ForEach(m => m.RefreshForEndOfRound());
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            s.Append(' ', 4).Append($"{InitString(Initiative)}: {Name}")
                .AppendLine(new string(' ', 93 - Name.Length));
            foreach (var m in Monsters.OrderBy(m => m.Tier).ThenBy(m => m.MonsterNumber))
            {
                s.Append(' ', 12).Append($"#{m.MonsterNumber} {TierString(m.Tier)}").Append(' ', 4)
                    .Append($"HP: {m.CurrentHitPoints}/{m.MaxHitPoints}; Atk: {m.BaseAttack}; Mv: {m.BaseMove}; Rng: {m.BaseRange}")
                    .AppendLine();
                var card = ActiveAbilityCard;
                if (card != null)
                {
                    var normalOrEliteText = 
                        m.Tier == MonsterTier.Normal 
                            ? card.Actions.Select(a => a.NormalActionText).ToList()
                            : card.Actions.Select(a => a.EliteActionText).ToList();
                    var actionsTextOneLine = string.Join(", ", normalOrEliteText.Select(s => s.Replace("\n", "")));
                    var numSpaces = (100 - actionsTextOneLine.Length) / 2;
                    s.Append(' ', numSpaces).Append(actionsTextOneLine).AppendLine();
                }
            }

            return s.ToString();
        }
        
        

        private string InitString(int? i)
        {
            if (i == null)
                return "NA";
            if (i.Value.ToString().Length == 1)
                return "0" + i.Value;
            return i.Value.ToString();
        }

        private string TierString(MonsterTier t)
        {
            return t == MonsterTier.Normal ? "normal" : "elite-";
        }
    }
}