namespace Gloom.Models.Monsters
{
    public class MonsterType
    {
        public MonsterType(string name, string deckName = null)
        {
            Name = name;
            DeckName = deckName ?? name;
            Stats = new MonsterStats(name);
        }
        public string Name { get; set; }
        public string DeckName { get; set; }
        public string ImageUrl { get; set; }
        public MonsterStats Stats { get; set; }
    }
}