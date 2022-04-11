namespace Gloom.Model.Monsters
{
    public class MonsterType
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public MonsterAbilityDeck AbilityDeck { get; set; }
        public MonsterStats Stats { get; set; }
        public int MaxNumberOnBoard { get; set; }
    }
}