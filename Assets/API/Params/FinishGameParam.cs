using System;
using System.Collections.Generic;

namespace API.Params
{
    [Serializable]
    public class FinishGameParam
    {
        public string gameId;
        public string cardSetId;
        
        public int cardsCompleted;
        public int timeInSeconds;

        public List<string> unwantedCards;
        public List<string> listOfRedrawnCards;
    }
}