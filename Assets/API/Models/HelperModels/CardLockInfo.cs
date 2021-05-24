using System;

namespace API.Models.HelperModels
{
    [Serializable]
    public class CardLockInfo
    {
        public Card card;
        public string playerId;
        public bool locked;
    }
}