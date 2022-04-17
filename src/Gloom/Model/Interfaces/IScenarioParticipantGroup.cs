namespace Gloom.Model.Interfaces
{
    public interface IScenarioParticipantGroup
    {
        public int? Initiative { get; }
        public string Name { get; }
        public void Draw();
    }
}