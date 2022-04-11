namespace Gloom.Model
{
    public class Status
    {
        public Status(bool isActive, string imageUrl, StatusType type)
        {
            IsActive = isActive;
            ImageUrl = imageUrl;
            Type = type;
            SetDuringCurrentTurn = false;
        }
        public bool IsActive;
        public string ImageUrl;
        public bool SetDuringCurrentTurn;

        public void ClearIfNotNew()
        {
            if (!SetDuringCurrentTurn) 
                Clear();
        }
        
        public void Clear()
        {
            IsActive = false;
        }

        public void Enable(bool isCurrentTurn)
        {
            IsActive = true;
            if (isCurrentTurn)
                SetDuringCurrentTurn = true;
        }
        
        public StatusType Type { get; set; }
    }
    
    public enum StatusType
    {
        Stun, Disarm, Immobilize, Poison, Wound, Strengthen, Muddle
    }
}