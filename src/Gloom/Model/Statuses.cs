﻿namespace Gloom.Model
{
    public class Statuses
    {
        public static readonly int NUM_STATUSES = 7;
        public Statuses()
        {
            AllStatuses[(int) StatusType.Stun] = new Status(false, string.Empty, StatusType.Stun);
            AllStatuses[(int) StatusType.Disarm] = new Status(false, string.Empty, StatusType.Disarm);
            AllStatuses[(int) StatusType.Immobilize] = new Status(false, string.Empty, StatusType.Immobilize);
            AllStatuses[(int) StatusType.Poison] = new Status(false, string.Empty, StatusType.Poison);
            AllStatuses[(int) StatusType.Wound] = new Status(false, string.Empty, StatusType.Wound);
            AllStatuses[(int) StatusType.Strengthen] = new Status(false, string.Empty, StatusType.Strengthen);
            AllStatuses[(int) StatusType.Muddle] = new Status(false, string.Empty, StatusType.Muddle);
        }



        private Status[] AllStatuses = new Status[NUM_STATUSES];

        public Status Stun => AllStatuses[(int) StatusType.Stun];
        public Status Disarm => AllStatuses[(int) StatusType.Disarm];
        public Status Immobilize => AllStatuses[(int) StatusType.Immobilize];
        public Status Poison => AllStatuses[(int) StatusType.Poison];
        public Status Wound => AllStatuses[(int) StatusType.Wound];
        public Status Strengthen => AllStatuses[(int) StatusType.Strengthen];
        public Status Muddle => AllStatuses[(int) StatusType.Muddle];

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
            
            foreach (var s in AllStatuses)
            {
                s.SetDuringCurrentTurn = false;
            }
        }
    }

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