using System.Collections.Generic;

namespace Gloom.Model
{
    public class Action
    {
        public List<Element> ElementsRequiredToBeConsumedForAction { get; set; }
        public string Text { get; set; }
        public List<ActionModifier> Modifiers { get; set; }
    }
    public class AttackAction : Action
    {
        public int AttackValueModifier { get; set; }
        public List<AttackActionModifier> AttackEffectModifiers { get; set; }
    }
    public class MoveAction : Action
    {
        public int MoveValueModifier { get; set; }
        public List<MoveActionModifier> MoveEffectModifiers { get; set; }
    }
    public class ShieldAction : Action
    {
        public int ShieldValue { get; set; }
    }
    public class RetaliateAction : Action
    {
        public int RetaliateValue { get; set; }
    }
    public class HealAction : Action
    {
        public int HealValue { get; set; }
        public AffectType Affect { get; set; } 
        // Heal cards say "Target" for integer, but "Affect" for things like all allies, self, etc.
    }
    public class LootAction : Action
    {
        public int LootValue { get; set; }
    }
    public class InflictStatusAction : Action
    {
        public List<StatusType> StatusesToInflict { get; set; }
        public TargetType Target { get; set; } // negative conditions
        public AffectType Affect { get; set; } // positive conditions
    }
    public class CreateElementAction : Action
    {
        public Element Element { get; set; }
    }
    public class TriggerAction : Action
    {
        public TriggerType Trigger { get; set; }
        public Action ActionTriggered { get; set; }
    }
    public class BossSpecialAction : Action
    {
        public int SpecialNum { get; set; }
    }
    
    // composite action?
    // example: Push 1 and Poison, target all adj enemies
    // example: Wound and Poison, target all adj enemies



    public class ActionModifier
    {
        public List<Element> ElementsRequiredToBeConsumedForModifier { get; set; }
        public string Text { get; set; }
    }
    

    public class AttackActionModifier : ActionModifier
    {
    }
    public class AttackActionAttackValueModifier : AttackActionModifier
    {
        public int AttackValueModifier { get; set; }
    }
    public class AttackActionAreaModifier : AttackActionModifier
    {
        public string AreaType { get; set; }
    }
    public class AttackActionRangeModifier : AttackActionModifier
    {
        public string RangeValueModifier { get; set; }
    }
    public class AttackActionPierceModifier : AttackActionModifier
    {
        public string PierceValueModifier { get; set; }
    }
    public class AttackActionStatusModifier : AttackActionModifier
    {
        public StatusType StatusToInflict { get; set; }
    }
    public class AttackActionTargetModifier : AttackActionModifier
    {
        public TargetType TargetType { get; set; }
    }
    
    public class MoveActionModifier : ActionModifier
    {
        public bool Jump { get; set; }
    }
    public class RetaliateActionModifier : ActionModifier
    {
        public int RangeValue { get; set; }
    }
    
    public enum TargetType
    {
        SpecificNumber, AllAdjacent, AllInRange, AllEnemies
    }

    public enum AffectType
    {
        Self, AllAllies, AllEnemies, AllAdjacentAllies
    }

    public enum TriggerType
    {
        OnDeath
    }
}