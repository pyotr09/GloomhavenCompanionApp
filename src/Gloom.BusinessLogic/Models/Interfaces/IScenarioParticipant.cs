namespace Gloom.Models.Interfaces
{
    public interface IScenarioParticipant
    {
        public int MaxHitPoints { get; set; }
        public int CurrentHitPoints { get; set; }
        public int CurrentShield { get; set; }
        public int CurrentRetaliate { get; set; }
        public Statuses Statuses { get; set; }
    }
}