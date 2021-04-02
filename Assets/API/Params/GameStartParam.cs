using System;
using System.Collections.Generic;

namespace API.Params
{
    [Serializable]
    public class GameStartParam
    {
        public string gameId;
        public string cardSetId;
        public List<string> unwantedCards;
    }
}