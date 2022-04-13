using System;

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

        public static StatusType ParseStatusString(string statusString)
        {
            switch (statusString)
            {
                case "Stun": return StatusType.Stun;
                case "Disarm": return StatusType.Disarm;
                case "Immobilize": return StatusType.Immobilize;
                case "Poison": return StatusType.Poison;
                case "Wound": return StatusType.Wound;
                case "Strengthen": return StatusType.Strengthen;
                case "Muddle": return StatusType.Muddle;
                case "Curse": return StatusType.Curse;
                case "Bless": return StatusType.Bless;
                case "Regenerate": return StatusType.Regenerate;
                case "Invisible": return StatusType.Invisible;
                default: throw new Exception($"Status not found: {statusString}");
            }
        }
    }
    
    public enum StatusType
    {
        Stun, Disarm, Immobilize, Poison, Wound, Strengthen, Muddle, Regenerate, Invisible, Curse, Bless
    }
}