namespace Gloom.Model
{
    public class Statuses
    {
        public static readonly int NUM_STATUSES = 8;
        public Statuses()
        {
            AllStatuses[(int) StatusType.Stun] = new Status(false, string.Empty, StatusType.Stun);
            AllStatuses[(int) StatusType.Disarm] = new Status(false, string.Empty, StatusType.Disarm);
            AllStatuses[(int) StatusType.Immobilize] = new Status(false, string.Empty, StatusType.Immobilize);
            AllStatuses[(int) StatusType.Poison] = new Status(false, string.Empty, StatusType.Poison);
            AllStatuses[(int) StatusType.Wound] = new Status(false, string.Empty, StatusType.Wound);
            AllStatuses[(int) StatusType.Strengthen] = new Status(false, string.Empty, StatusType.Strengthen);
            AllStatuses[(int) StatusType.Muddle] = new Status(false, string.Empty, StatusType.Muddle);
            AllStatuses[(int) StatusType.Regenerate] = new Status(false, string.Empty, StatusType.Regenerate);
            AllStatuses[(int) StatusType.Invisible] = new Status(false, string.Empty, StatusType.Invisible);

            // curse and bless not tracked the same way
        }

        private Status[] AllStatuses = new Status[NUM_STATUSES];

        public Status Stun => AllStatuses[(int) StatusType.Stun];
        public Status Disarm => AllStatuses[(int) StatusType.Disarm];
        public Status Immobilize => AllStatuses[(int) StatusType.Immobilize];
        public Status Poison => AllStatuses[(int) StatusType.Poison];
        public Status Wound => AllStatuses[(int) StatusType.Wound];
        public Status Strengthen => AllStatuses[(int) StatusType.Strengthen];
        public Status Muddle => AllStatuses[(int) StatusType.Muddle];
        public Status Regenerate => AllStatuses[(int) StatusType.Regenerate];
        public Status Invisible => AllStatuses[(int) StatusType.Invisible];

        public void SetStatus(StatusType type, bool active, bool currentTurn)
        {
            if (active)
                AllStatuses[(int) type].Enable(currentTurn);
            else
                AllStatuses[(int) type].Clear();
        }
        
        public void ClearForEndOfTurn()
        {
            Stun.ClearIfNotNew();
            Disarm.ClearIfNotNew();
            Immobilize.ClearIfNotNew();
            Strengthen.ClearIfNotNew();
            Muddle.ClearIfNotNew();
            Invisible.ClearIfNotNew();
            
            foreach (var s in AllStatuses)
            {
                s.SetDuringCurrentTurn = false;
            }
        }
    }
}