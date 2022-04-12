using System;
using System.Collections.Generic;

namespace Gloom.Model.Monsters
{
    public class BaseMonsterStats
    {
        public BaseMonsterStats(List<string> attributes)
        {
            StatusesInflicted = new List<StatusType>();
            ParseAttributes(attributes);
        }

        private void ParseAttributes(List<string> attributes)
        {
            foreach (var attr in attributes)
            {
                if (attr == "Attackers gain Disadvantage")
                {
                    DoAttackersGainDisadvantage = true;
                    continue;
                }

                var actionWords = attr.Split(" ");
                switch (actionWords[0])
                {
                    case "Muddle": StatusesInflicted.Add(StatusType.Muddle);
                        break;
                    case "Disarm": StatusesInflicted.Add(StatusType.Disarm);
                        break;
                    case "Immobilize": StatusesInflicted.Add(StatusType.Immobilize);
                        break;
                    case "Poison": StatusesInflicted.Add(StatusType.Poison);
                        break;
                    case "Stun": StatusesInflicted.Add(StatusType.Stun);
                        break;
                    case "Wound": StatusesInflicted.Add(StatusType.Wound);
                        break;
                    case "Curse": DoesCurse = true;
                        break;
                    case "Shield": BaseShield = int.Parse(actionWords[1]);
                        break;
                    case "Target": BaseTarget = int.Parse(actionWords[1]);
                        break;
                    case "Pierce": BasePierce = int.Parse(actionWords[1]);
                        break;
                    case "Advantage": HasAdvantage = true;
                        break;
                    case "Flying": IsFlying = true;
                        break;
                    case "Retaliate": ParseRetaliateAttribute(actionWords);
                        break;
                    default: throw new Exception($"Action not found for attribute: {actionWords[0]}");
                }
            }
        }

        private void ParseRetaliateAttribute(string[] actionWords)
        {
            BaseRetaliate = int.Parse(actionWords[1].TrimEnd(','));
            if (actionWords.Length == 4)
            {
                BaseRetaliateRange = int.Parse(actionWords[3]);
            }
        }

        public int Health { get; set; }
        public int BaseAttack { get; set; }
        public int BaseTarget { get; set; } = 1;
        public int BaseRange { get; set; }
        public int BaseMove { get; set; }
        public int BaseShield { get; set; }
        public int BaseRetaliate { get; set; }
        public int BaseRetaliateRange { get; set; }
        public int BasePierce { get; set; }
        public bool IsFlying { get; set; }
        public List<StatusType> StatusesInflicted { get; set; }
        public bool DoAttackersGainDisadvantage { get; set; }
        public bool HasAdvantage { get; set; }
        public bool DoesCurse { get; set; }
    }
}