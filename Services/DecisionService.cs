using CasinoSimulationApi.Models;
using System.Reflection.Metadata.Ecma335;

namespace CasinoSimulationApi.Data
{
    public class DecisionService
    {
        public Dictionary<string, Decision> DecisionDictionary { get; set; }

        public DecisionService()
        {
            DecisionDictionary = new Dictionary<string, Decision>
            {
                { "9-2", Decision.Hit },
                { "9-3", Decision.Double },
                { "9-4", Decision.Double },
                { "9-5", Decision.Double },
                { "9-6", Decision.Double },
                { "12-2", Decision.Hit },
                { "12-3", Decision.Hit },
                { "12-4", Decision.Stand },
                { "12-5", Decision.Stand },
                { "12-6", Decision.Stand },
            };
        }

        // string format: "PlayerTotal-DealerFaceUpCard"
        public Decision GetDecisionFromDictionary(int PlayerTotal, int DealerFaceUpCard)
        {
            string key = $"{PlayerTotal}-{DealerFaceUpCard}";
            return DecisionDictionary[key];
        }

        public Decision Decide(BlackjackGame game)
        {
            int PlayerTotal = game.PlayerCards.Sum();
            int DealerFaceUpCard = game.DealerFaceUpCard;
            if (DealerFaceUpCard == 1)
            {
                DealerFaceUpCard = 11;
            }

            if (PlayerTotal < 9)
            {
                return Decision.Hit;
            }

            if (PlayerTotal > 16)
            {
                return Decision.Stand;
            }

            if (PlayerTotal > 12)
            {
                if (DealerFaceUpCard < 7)
                {
                    return Decision.Stand;
                }
                else
                {
                    return Decision.Hit;
                }
            }

            if (PlayerTotal == 12)
            {
                if (DealerFaceUpCard > 6)
                {
                    return Decision.Hit;
                }
                else
                {
                    return GetDecisionFromDictionary(PlayerTotal, DealerFaceUpCard);
                }
            }

            if (PlayerTotal == 11)
            {
                return Decision.Double;
            }

            if (PlayerTotal == 10)
            {
                if (DealerFaceUpCard < 10)
                {
                    return Decision.Double;
                }
                else
                {
                    return Decision.Hit;
                }
            }

            if (PlayerTotal == 9)
            {
                if (DealerFaceUpCard > 6)
                {
                    return Decision.Hit;
                }
                else
                {
                    return GetDecisionFromDictionary(PlayerTotal, DealerFaceUpCard);
                }
            }
            return Decision.Error;
        }
    }
}
