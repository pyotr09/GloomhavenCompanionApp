using System;
using System.Collections.Generic;
using System.Linq;
using Gloom.Model.Actions;
using Gloom.Model.Interfaces;
using Gloom.Model.Monsters;

namespace Gloom.Model.Bosses
{
    public class Boss : IScenarioParticipantGroup
    {
        public Boss()
        {
        }
        
        public Boss(BossType type, int level, int numberOfCharacters, int numberOfScouts = 0)
        {
            BaseBossStats stats = type.Stats.GetStatsByLevel(level);
            MaxHealth = stats.Health;
            BaseAttack = CalculateAttack(numberOfCharacters, stats.BaseAttackFormula, numberOfScouts);
            BaseRange = stats.BaseRange;
            BaseMove = stats.BaseMove;
            Immunities = stats.Immunities;
            Special1Actions = stats.Special1Actions;
            Special2Actions = stats.Special2Actions;
            Notes = stats.Notes;
            Name = type.Name;
            BaseStatsList= new List<BaseStats>{stats};
            AbilityDeck = AbilityParser.Instance.ParseDeck(this);
            IsActive = false;
        }
        
        public void Activate()
        {
            IsActive = true;
            CurrentHealth = MaxHealth;
        }

        public bool IsActive { get; set; }
        public string BaseAttack { get; set; }
        public int BaseMove { get; set; }
        public int CurrentHealth { get; set; }
        public int MaxHealth { get; set; }
        public int BaseRange { get; set; }
        public List<StatusType> Immunities { get; set; }
        public List<string> Special1Actions { get; set; }
        public List<string> Special2Actions { get; set; }
        public List<string> Notes { get; set; }
        public int? Initiative => ActiveAbilityCard?.Initiative;
        public string Name { get; set; }
        public MonsterAbilityDeck AbilityDeck { get; set; }
        public MonsterAbilityCard ActiveAbilityCard { get; set; }
        public string Type => "Boss";
        public string DeckName => "Boss";
        public List<BaseStats> BaseStatsList { get; set; }

        private string CalculateAttack(int numberOfCharacters, string attackFormula, int numberOfScouts)
        {
            if (attackFormula == 'V'.ToString() || attackFormula.Contains('X')) 
            {
                return attackFormula;
            }
            if (int.TryParse(attackFormula, out var basic))
            {
                return basic.ToString();
            }
            if (int.TryParse(attackFormula.Substring(0,attackFormula.IndexOf('+')), out var adder))
            {
                return (adder + numberOfCharacters).ToString();
            }
            if (!int.TryParse(attackFormula.Substring(0, attackFormula.IndexOf('x')), out var multiplier))
            {
                throw new Exception($"Failed to determine boss attack");
            }
            return (multiplier * numberOfCharacters).ToString();
        }

        public void RefreshForEndOfRound()
        {
            ActiveAbilityCard = null;
            if (AbilityDeck.DiscardPile.Any(c => c.ShuffleAfter))
            {
                AbilityDeck.ShuffleDiscardIntoDraw();
            }
        }

        public void SetStatus(StatusType type, bool active, bool currentTurn = false)
        {
            // todo: handle a new status ie. stun/disarm/strengthen/etc.
        }

        public void Draw()
        {
            if (IsActive)
                ActiveAbilityCard = AbilityDeck.Draw();
        }

    }
}