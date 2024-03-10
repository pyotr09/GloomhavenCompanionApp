using System;
using Gloom.Common;

namespace Gloom.Models
{
    public class Statuses
    {
        public static readonly int NUM_STATUSES = 9;
        public Statuses()
        {
            _trackedStatuses[(int) StatusType.Stun] = new Status(false, string.Empty, StatusType.Stun);
            _trackedStatuses[(int) StatusType.Disarm] = new Status(false, string.Empty, StatusType.Disarm);
            _trackedStatuses[(int) StatusType.Immobilize] = new Status(false, string.Empty, StatusType.Immobilize);
            _trackedStatuses[(int) StatusType.Poison] = new Status(false, string.Empty, StatusType.Poison);
            _trackedStatuses[(int) StatusType.Wound] = new Status(false, string.Empty, StatusType.Wound);
            _trackedStatuses[(int) StatusType.Strengthen] = new Status(false, string.Empty, StatusType.Strengthen);
            _trackedStatuses[(int) StatusType.Muddle] = new Status(false, string.Empty, StatusType.Muddle);
            _trackedStatuses[(int) StatusType.Regenerate] = new Status(false, string.Empty, StatusType.Regenerate);
            _trackedStatuses[(int) StatusType.Invisible] = new Status(false, string.Empty, StatusType.Invisible);

            // curse and bless not tracked the same way
        }

        private Status[] _trackedStatuses = new Status[NUM_STATUSES];

        public Status Stun => _trackedStatuses[(int) StatusType.Stun];
        public Status Disarm => _trackedStatuses[(int) StatusType.Disarm];
        public Status Immobilize => _trackedStatuses[(int) StatusType.Immobilize];
        public Status Poison => _trackedStatuses[(int) StatusType.Poison];
        public Status Wound => _trackedStatuses[(int) StatusType.Wound];
        public Status Strengthen => _trackedStatuses[(int) StatusType.Strengthen];
        public Status Muddle => _trackedStatuses[(int) StatusType.Muddle];
        public Status Regenerate => _trackedStatuses[(int) StatusType.Regenerate];
        public Status Invisible => _trackedStatuses[(int) StatusType.Invisible];

        public void SetStatus(StatusType type, bool active, bool currentTurn)
        {
            if (active)
                _trackedStatuses[(int) type].Enable(currentTurn);
            else
                _trackedStatuses[(int) type].Clear();
        }
        
        public Status GetStatusByName(string statusString)
        {
            switch (statusString)
            {
                case "Stun": return Stun;
                case "Disarm": return Disarm;
                case "Immobilize": return Immobilize;
                case "Poison": return Poison;
                case "Wound": return Wound;
                case "Strengthen": return Strengthen;
                case "Muddle": return Muddle;
                case "Regenerate": return Regenerate;
                case "Invisible": return Invisible;
                default: throw new Exception($"Status not found: {statusString}");
            }
        }
        
        public void ClearForEndOfTurn()
        {
            Stun.ClearIfNotNew();
            Disarm.ClearIfNotNew();
            Immobilize.ClearIfNotNew();
            Strengthen.ClearIfNotNew();
            Muddle.ClearIfNotNew();
            Invisible.ClearIfNotNew();
            
            foreach (var s in _trackedStatuses)
            {
                s.SetDuringCurrentTurn = false;
            }
        }
    }
}