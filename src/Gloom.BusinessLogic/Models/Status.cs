using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Gloom.Models
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
        [JsonIgnore]
        public string ImageUrl;
        [JsonIgnore]
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

        private StatusType Type { get; set; }

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

        public static string GetStringForStatus(StatusType type)
        {
            switch (type)
            {
                case StatusType.Stun: return "Stun";
                case StatusType.Disarm: return "Disarm";
                case StatusType.Immobilize: return "Immobilize";
                case StatusType.Poison: return "Poison";
                case StatusType.Wound: return "Wound";
                case StatusType.Strengthen: return "Strengthen";
                case StatusType.Muddle: return "Muddle";
                case StatusType.Curse: return "Curse";
                case StatusType.Bless: return "Bless";
                case StatusType.Regenerate: return "Regenerate";
                case StatusType.Invisible: return "Invisible";
                default: throw new Exception($"Status not found: {type}");
            }
        }

        public static List<string> StatusStrings => new()
        {
           "Stun",
            "Disarm",
            "Immobilize",
            "Poison",
            "Wound",
            "Strengthen",
            "Muddle",
            "Curse",
            "Bless",
            "Regenerate",
            "Invisible",
        };
    }
    
    public enum StatusType
    {
        Stun, Disarm, Immobilize, Poison, Wound, Strengthen, Muddle, Regenerate, Invisible, Curse, Bless
    }
}