namespace Gloom.Model.Interfaces
{
    public interface IScenarioParticipant
    {
        public int? Initiative { get; set; }
        public int MaxHitPoints { get; set; }
        public int CurrentHitPoints { get; set; }
        public string Name { get; set; }
        public int CurrentShield { get; set; }
        public int CurrentRetaliate { get; set; }
        public Statuses Statuses { get; set; }
    }
}